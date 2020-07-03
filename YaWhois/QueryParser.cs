using System;
using System.Linq;
using System.Text.RegularExpressions;
using Nunycode;
using YaWhois.Utils;
using YaWhois.Data;

namespace YaWhois
{
    public static class QueryParser
    {
        public static string GuessServer(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Empty query.");

            var str = Punycode.ToAscii(query.Trim().TrimEnd(new char[] { '.' }));

            // IPv6 address
            if (str.Contains(':'))
            {
                // RPSL hierarchical objects
                if (str.StartsWith("as", StringComparison.InvariantCultureIgnoreCase))
                    return FindAS(stdlib.strtoul(str.Substring(2), 10));

                return ParseIPv6(str);
            }

            // email
            if (str.Contains('@'))
                throw new NoServerException();

            // TLD domains -> try find, but don't throw error yet
            if (!str.Contains('.'))
            {
                /* if it is a TLD or a new gTLD then ask IANA */
                var low = str.ToLowerInvariant();
                var tld = Assignments.TLD.FirstOrDefault(a => a.Item1 == low);

                if (tld != null || Assignments.GTLD.Any(a => a == str))
                    return "whois.iana.org";
            }

            // no dot and no hyphen means it's a NSI NIC handle or ASN (?)
            if (!(str.Contains('.') || str.Contains('-')))
            {
                if (Regex.IsMatch(str, "^as[0-9a-zA-Z\\ ]", RegexOptions.IgnoreCase))
                    return FindAS(stdlib.strtoul(str.Substring(2), 10));

                if (str[0] == '!') /* NSI NIC handle */
                    return "whois.networksolutions.com";
                else
                    throw new NoServerException();
            }

            // ASN32
            if (str.StartsWith("as", StringComparison.InvariantCultureIgnoreCase)
                && str.Length >= 3
                && TryParseASN32(str.Substring(2), out uint asn32))
            {
                return FindAS32(asn32);
            }

            // IPv4
            if (TryParseIPv4(str, out uint ip))
                return FindIPv4(ip);

            // TLD
            var tld_result = FindByTLD(str);
            if (tld_result != null)
                return tld_result;

            // new gTLD, e.g. they have whois.nic.[TLD]
            var gtld_result = FindByNewTLD(str);
            if (gtld_result != null)
                return gtld_result;

            // no dot but hyphen -> NIC
            if (!str.Contains('.'))
            {
                var nicPrefix = Assignments.NicHandlePrefixes
                    .Where(a => str.StartsWith(a.Key))
                    .FirstOrDefault();

                if (nicPrefix.Key != null)
                    return nicPrefix.Value;

                var nicSuffix = Assignments.NicHandleSuffixes
                    .Where(a => str.EndsWith(a.Key))
                    .FirstOrDefault();

                if (nicSuffix.Key != null)
                    return nicSuffix.Value;

                return "whois.arin.net";
            }

            // done
            throw new NoServerException();
        }


        static string ParseIPv6(string s)
        {
            var p1 = stdlib.strtoul(s, 16);

            if (p1 == 0)
                throw new NoServerException();

            var net = (p1 << 16) + stdlib.strtoul(s.Substring(s.IndexOf(':') + 1), 16);
            var assign = Assignments.IPv6
                .Where(x => x.Item1 == (net & (~0UL << (32 - x.Item2))))
                .FirstOrDefault();

            if (assign == null)
                throw new UnknownNetworkException();

            switch (assign.Item3[0])
            {
                case '\x0A':
                    var ipv4 = Convert6to4(s);
                    if (TryParseIPv4(ipv4, out uint ip))
                        return FindIPv4(ip);
                    else
                        throw new NoServerException();

                case '\x0B':
                    var ip4teredo = ConvertTeredo(s);
                    if (TryParseIPv4(ip4teredo, out uint teredoIp))
                        return FindIPv4(teredoIp);
                    else
                        throw new NoServerException();

                case '\x06':
                    throw new UnknownNetworkException();

                default:
                    return assign.Item3;
            }
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


        static string FindIPv4(uint ip)
        {
            var assign = Assignments.IPv4
                .Where(a => (ip & a.Item2) == a.Item1)
                .FirstOrDefault();

            if (assign != null)
            {
                switch (assign.Item3[0])
                {
                    case '\x05':
                        throw new NoServerException();

                    default:
                        return assign.Item3;
                }
            }

            throw new NoServerException();
        }


        static string ParseTeredo(string s)
        {
            throw new NotImplementedException();
        }


        static string FindAS(uint asn)
        {
            if (asn > 65535)
                return FindAS32(asn);

            var assing = Assignments.AS16
                .Where(a => asn >= a.Item1 && asn <= a.Item2)
                .FirstOrDefault();

            if (assing != null)
                return assing.Item3;

            throw new UnknownNetworkException();
        }


        static string FindAS32(uint asn)
        {
            var assing = Assignments.AS32
                .Where(a => asn >= a.Item1 && asn <= a.Item2)
                .FirstOrDefault();

            if (assing != null)
                return assing.Item3;

            throw new UnknownNetworkException();
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


        static string FindByTLD(string fqdn, bool throwError = false)
        {
            var dom = fqdn.ToLowerInvariant();
            var domlen = dom.Length;
            var tld_result = Assignments.TLD
                .Where(a => domlen >= a.Item1.Length - 1) // dot + tld
                .Where(a => dom.EndsWith(a.Item1))
                .Where(a => dom[domlen - a.Item1.Length - 1] == '.')
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
                    case Assignments.Hints.IPv6:
                        throw new NotImplementedException();

                    default:
                        return tld_result.Item3;
                }
            }

            if (throwError)
                throw new NoServerException();

            return null;
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
            var r = new Regex("^(\\d{1,3})\\.(\\d{1,3})\\.(\\d{1,3})\\.(\\d{1,3})\\/?");
            var m = r.Match(s);

            if (!m.Success)
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
