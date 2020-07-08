using System;
using System.Linq;
using System.Text.RegularExpressions;
using Nunycode;
using YaWhois.Utils;
using YaWhois.Data;
using System.Text;

namespace YaWhois
{
    public class QueryParser
    {
        /// <summary>
        /// Normalized query request.
        /// </summary>
        public string Query { get; internal set; }
        /// <summary>
        /// Selected server for Query.
        /// </summary>
        public string Server { get; internal set; }
        /// <summary>
        /// Set of server hints.
        /// </summary>
        public ServerHints ServerHint { get; internal set; }

        #region FormatQuery fields
        /// <summary>
        /// A query which will be passed to a server.
        /// </summary>
        public string ServerQuery { get; internal set; }
        public Encoding ServerEncoding { get; internal set; }
        /// <summary>
        /// Optional encoding parameters for ServerQuery.
        /// </summary>
        public string EncodingParameter { get; internal set; }
        #endregion


        public enum ServerHints
        {
            NONE = 0,
            GTLD = 1,
            CRSNIC = 2,
            AFILIAS = 4,
            RIPE = 8,
            QUERY_AS = 16,
            QUERY_IP = 32
        }


        public QueryParser GuessServer(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                throw new ArgumentException("Empty query.");

            Query = Punycode.ToAscii(s.Trim().TrimEnd(new char[] { '.' }));
            Server = null;
            ServerEncoding = null;
            ServerHint = ServerHints.NONE;
            EncodingParameter = null;

            // IPv6 address
            if (Query.Contains(':'))
            {
                // RPSL hierarchical objects
                if (Query.StartsWith("as", StringComparison.InvariantCultureIgnoreCase))
                    return FindAS(stdlib.strtoul(s.Substring(2), 10));

                return ParseIPv6(Query);
            }

            // email
            if (Query.Contains('@'))
                throw new NoServerException();

            // TLD domains -> try find, but don't throw error yet
            if (!Query.Contains('.'))
            {
                /* if it is a TLD or a new gTLD then ask IANA */
                var low = Query.ToLowerInvariant();
                var tld = Assignments.TLD.FirstOrDefault(a => a.Item1 == low);

                if (tld != null || Assignments.GTLD.Any(a => a == Query))
                {
                    Server = "whois.iana.org";
                    return this;
                }
            }

            // no dot and no hyphen means it's a NSI NIC handle or ASN (?)
            if (!(Query.Contains('.') || Query.Contains('-')))
            {
                if (Regex.IsMatch(Query, "^as[0-9a-zA-Z\\ ]", RegexOptions.IgnoreCase))
                    return FindAS(stdlib.strtoul(Query.Substring(2), 10));

                if (Query[0] == '!') /* NSI NIC handle */
                {
                    Server = "whois.networksolutions.com";
                    return this;
                }

                throw new NoServerException();
            }

            // ASN32
            if (Query.StartsWith("as", StringComparison.InvariantCultureIgnoreCase)
                && Query.Length >= 3
                && TryParseASN32(Query.Substring(2), out uint asn32))
            {
                return FindAS32(asn32);
            }

            // IPv4
            if (TryParseIPv4(Query, out uint ip))
                return FindIPv4(ip);

            // TLD
            if (IsTLD(Query))
                return this;

            // new gTLD, e.g. they have whois.nic.[TLD]
            var gtld_result = FindByNewTLD(Query);
            if (gtld_result != null)
            {
                Server = gtld_result;
                ServerHint |= ServerHints.GTLD;
                return this;
            }

            // no dot but hyphen -> NIC
            if (!Query.Contains('.'))
            {
                var low = Query.ToLowerInvariant();
                var nicPrefix = Assignments.NicHandlePrefixes
                    .Where(a => low.StartsWith(a.Key))
                    .FirstOrDefault();

                if (nicPrefix.Key != null)
                {
                    Server = nicPrefix.Value;
                    return this;
                }

                var nicSuffix = Assignments.NicHandleSuffixes
                    .Where(a => low.EndsWith(a.Key))
                    .FirstOrDefault();

                if (nicSuffix.Key != null)
                {
                    Server = nicSuffix.Value;
                    return this;
                }

                Server = "whois.arin.net";
                return this;
            }

            // done
            throw new NoServerException();
        }


        public QueryParser FormatQuery()
        {
            if (string.IsNullOrEmpty(Server))
                throw new ArgumentNullException(Server);

            if (Assignments.RipeServers.Contains(Server))
            {
                ServerHint |= ServerHints.RIPE;
            }

            var encoding = Assignments.ServerEncodings
                .FirstOrDefault(a => a.Item1 == Server);

            if (encoding != null)
            {
                // According to docs:
                // If the same encoding provider is used in multiple calls to the RegisterProvider method,
                // only the first method call registers the provider. Subsequent calls are ignored.
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                // TODO: Encoding.GetEncoding() is slow, the value must be cached.
                ServerEncoding = Encoding.GetEncoding(encoding.Item2);
                EncodingParameter = encoding.Item3;
            }

            if (Server == "whois.denic.de" && IsQueryDomain("de"))
                ServerQuery = "-T dn,ace " + Query;
            else if (Server == "whois.dk-hostmaster.dk" && IsQueryDomain("dk"))
                ServerQuery = "--show-handles " + Query;

            var isripe = ServerHint.HasFlag(ServerHints.RIPE);
            var isasn = ServerHint.HasFlag(ServerHints.QUERY_AS);

            if (!isripe && isasn && Server == "whois.nic.ad.jp")
            {
                ServerQuery = "AS " + Query.Substring(2);
            }
            else if (!isripe && Server == "whois.arin.net" && !Query.Contains(' '))
            {
                if (isasn)
                    ServerQuery = "a " + Query.Substring(2);
                else if (ServerHint.HasFlag(ServerHints.QUERY_IP))
                    ServerQuery = "n + " + Query;
                else
                    ServerQuery = Query;
            }
            else
            {
                ServerQuery = Query;
            }

            // TODO: add japanese support by request.
            if (!isripe && (Server == "whois.nic.ad.jp" || Server == "whois.jprs.jp"))
                ServerQuery += "/e";

            return this;
        }


        QueryParser ParseIPv6(string s)
        {
            var p1 = stdlib.strtoul(s, 16);

            if (p1 == 0 || !s.Contains(':'))
                throw new NoServerException();

            var net = (p1 << 16) + stdlib.strtoul(s.Substring(s.IndexOf(':') + 1), 16);
            var assign = Assignments.IPv6
                .Where(x => x.Item1 == (net & (~0UL << (32 - x.Item2))))
                .FirstOrDefault();

            if (assign == null)
                throw new UnknownNetworkException();

            ServerHint |= ServerHints.QUERY_IP;

            switch (assign.Item3[0])
            {
                case '\x0A':
                    Query = Convert6to4(s);
                    if (TryParseIPv4(Query, out uint ip))
                        return FindIPv4(ip);
                    else
                        throw new NoServerException();

                case '\x0B':
                    Query = ConvertTeredo(s);
                    if (TryParseIPv4(Query, out uint teredoIp))
                        return FindIPv4(teredoIp);
                    else
                        throw new NoServerException();

                case '\x06':
                    throw new UnknownNetworkException();

                default:
                    Server = assign.Item3;
                    return this;
            }
        }


        QueryParser FindIPv4(uint ip)
        {
            var assign = Assignments.IPv4
                .Where(a => (ip & a.Item2) == a.Item1)
                .FirstOrDefault();

            if (assign != null)
            {
                ServerHint |= ServerHints.QUERY_IP;

                switch (assign.Item3[0])
                {
                    case '\x05':
                        throw new NoServerException();

                    default:
                        Server = assign.Item3;
                        return this;
                }
            }

            throw new NoServerException();
        }


        QueryParser FindAS(uint asn)
        {
            if (asn > 65535)
                return FindAS32(asn);

            var assing = Assignments.AS16
                .Where(a => asn >= a.Item1 && asn <= a.Item2)
                .FirstOrDefault();

            if (assing != null)
            {
                Server = assing.Item3;
                ServerHint |= ServerHints.QUERY_AS;
                return this;
            }

            throw new UnknownNetworkException();
        }


        QueryParser FindAS32(uint asn)
        {
            var assing = Assignments.AS32
                .Where(a => asn >= a.Item1 && asn <= a.Item2)
                .FirstOrDefault();

            if (assing != null)
            {
                Server = assing.Item3;
                ServerHint |= ServerHints.QUERY_AS;
                return this;
            }

            throw new UnknownNetworkException();
        }


        static string Convert6to4(string s)
        {
            uint a, b;
            var m = Regex.Match(s, "^2002:([\\da-fA-F]{1,4}):(:|(([\\da-fA-F]{1,4}):))");

            if (!m.Success)
                return "0.0.0.0";

            a = Convert.ToUInt32(m.Groups[1].Value, 16);
            b = m.Groups[4].Success ? Convert.ToUInt32(m.Groups[4].Value, 16) : 0;

            return string.Format("{0}.{1}.{2}.{3}", a >> 8, a & 0xFF, b >> 8, b & 0xFF);
        }


        static string ConvertTeredo(string s)
        {
            uint a, b;
            var m = Regex.Match(s, "^2001:([\\da-fA-F]{1,4}:){5}([\\da-fA-F]{1,4}):([\\da-fA-F]{1,4})$");

            if (!m.Success)
                return "0.0.0.0";

            a = Convert.ToUInt32(m.Groups[2].Value, 16) ^ 0xFFFF;
            b = Convert.ToUInt32(m.Groups[3].Value, 16) ^ 0xFFFF;

            return string.Format("{0}.{1}.{2}.{3}", a >> 8, a & 0xFF, b >> 8, b & 0xFF);
        }


        /// <summary>
        /// Returns true if specified string is valid ASN32, e.g. 123.768.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="asn32"></param>
        /// <returns></returns>
        static bool TryParseASN32(string s, out uint asn32)
        {
            asn32 = 0;

            if (string.IsNullOrWhiteSpace(s))
                return false;

            var r = new Regex("^(\\d+)\\.(\\d+)$");
            var m = r.Match(s);

            if (m.Success)
            {
                var a = uint.Parse(m.Groups[1].Value);
                var b = uint.Parse(m.Groups[2].Value);

                if (a <= 65535 && b <= 65535)
                {
                    asn32 = (a << 16) + b;
                    return true;
                }
            }

            return false;
        }


        bool IsTLD(string fqdn, bool throwError = false)
        {
            var dom = fqdn.ToLowerInvariant();
            var domlen = dom.Length;
            var tld_result = Assignments.TLD
                .Where(a => a.Item1.Length <= domlen - 1) // dot + tld
                .Where(a => dom[domlen - a.Item1.Length - 1] == '.')
                .Where(a => dom.EndsWith(a.Item1))
                .FirstOrDefault();

            if (tld_result != null)
            {
                switch (tld_result.Item2)
                {
                    case Assignments.Hints.EXTERNAL:
                        throw new ExternalWhoisException(tld_result.Item3);

                    case Assignments.Hints.NOSERVER:
                        throw new NoServerException();

                    case Assignments.Hints.IPv4:
                        var ip4str = ConvertInArpa(Query);
                        TryParseIPv4(ip4str, out uint ip4);
                        FindIPv4(ip4);
                        return true;

                    case Assignments.Hints.IPv6:
                        var ip6str = ConvertInArpa6(Query);
                        ParseIPv6(ip6str);
                        return true;

                    case Assignments.Hints.AFILIAS:
                        Server = tld_result.Item3;
                        ServerHint |= ServerHints.AFILIAS;
                        return true;

                    case Assignments.Hints.CSRNIC:
                        Server = tld_result.Item3;
                        ServerHint |= ServerHints.CRSNIC;
                        return true;

                    default:
                        Server = tld_result.Item3;
                        return true;
                }
            }

            if (throwError)
                throw new NoServerException();

            return false;
        }


        bool IsQueryDomain(string domain)
        {
            if (string.IsNullOrEmpty(Query) || string.IsNullOrEmpty(domain))
                return false;

            if (domain.Length >= Query.Length - 1)
                return false;

            var end = Query.Substring(Query.Length - domain.Length);

            if (end == domain && end[0] != '.')
                return true;

            return false;
        }

        static string FindByNewTLD(string fqdn, bool throwError = false)
        {
            var dom = fqdn.ToLowerInvariant();
            var domlen = dom.Length;
            var gtld = Assignments.GTLD
                .Where(tld => domlen >= tld.Length - 1) // dot + tld
                .Where(tld => dom.EndsWith(tld))
                .Where(tld => dom[domlen - tld.Length - 1] == '.')
                .FirstOrDefault();

            if (gtld != null)
                return "whois.nic." + gtld;

            if (throwError)
                throw new NoServerException();

            return null;
        }


        static bool TryParseIPv4(string s, out uint ip)
        {
            ip = 0;
            var r = new Regex("^(\\d{1,3})\\.(\\d{1,3})\\.(\\d{1,3})\\.(\\d{1,3})(.)?");
            var m = r.Match(s);

            if (!m.Success || (m.Groups[5].Success && m.Groups[5].Value != "/"))
                return false;

            for (var i = 1; i <= 4; i++)
            {
                var oct = uint.Parse(m.Groups[i].Value);
                if (oct > 255)
                    return false;
                ip += oct << (32 - 8 * i);
            }

            return true;
        }


        static string ConvertInArpa(string str)
        {
            if (!Regex.IsMatch(str, "^(\\d{1,3}\\.){1,3}in-addr\\.arpa$"))
                return "0.0.0.0";

            long[] abc = new long[] { 0, 0, 0 };
            var s = str;
            for (var i = 0; i < abc.Length; i++) {
                if (i >= 4)
                    return "0.0.0.0";

                // XXX: end is a byte position, but used as a character ones.
                var oct = stdlib.strtol(s, out long end, 10);
                if (oct < 0 || oct > 255 || s[(int)end] != '.')
                    return "0.0.0.0";

                abc[i] = oct;
                s = s.Substring((int)end + 1);
            }

            return string.Join(".", abc.Reverse()) + ".0";
        }


        static string ConvertInArpa6(string str)
        {
            var low = str.ToLowerInvariant();

            if (!Regex.IsMatch(low, "^([0-9a-f]\\.){1,39}ip6\\.arpa$"))
                return "";

            var nibbles = low.Split(new char[] { '.' }).Reverse().ToList();
            nibbles.RemoveRange(0, 2);

            var result = "";
            for (int i = 0, digits = 1; i < nibbles.Count && i < 40; i++)
            {
                result += nibbles[i];

                if ((digits++ % 4) == 0)
                    result += ":";
            }

            if (result.Last() == ':')
                return result.Remove(result.Length - 1);

            return result;
        }


        #region Exceptions

        public class NoServerException : Exception
        {
            /// <summary>
            /// No whois server is known for target object.
            /// </summary>
            public NoServerException()
                : base("No whois server is known.")
            { }
        }


        public class UnknownNetworkException : Exception
        {
            /// <summary>
            /// Unknown AS number or IP network.
            /// </summary>
            public UnknownNetworkException()
                : base("Unknown AS number or IP network.")
            { }
        }


        public class ExternalWhoisException: Exception
        {
            /// <summary>
            /// You can access the whois database at {source}.
            /// </summary>
            /// <param name="source"></param>
            public ExternalWhoisException(string source)
                : base(string.Format("You can access the whois database at {0}", source))
            { }
        }

        #endregion
    }
}
