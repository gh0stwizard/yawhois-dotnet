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
                var tld = Assignments.TLD.FirstOrDefault(a => a.Item1 == str);

                if (tld != null || Assignments.GTLD.ContainsKey(str))
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

            // TLD

            // gTLD

            // no dot but hyphen -> NIC

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
                    return ParseIPv4(ipv4);

                case '\x0B':
                    var t = ConvertTeredo(s);
                    return ParseTeredo(t);

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


        static string ParseIPv4(string s)
        {
            throw new NotImplementedException();
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

        #endregion
    }
}
