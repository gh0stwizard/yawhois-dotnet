using System;
using Nunycode;
using YaWhois.Utils;
using YaWhois.Data;
using System.Linq;

namespace YaWhois
{
    public static class QueryParser
    {
        public static string GuessServer(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Empty query.");

            var str = Punycode.ToAscii(query.Trim().TrimEnd(new char[] { '.' }));
            str.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            // IPv6 address
            if (str.Contains(":"))
            {
                // RPSL hierarchical objects
                if (str.StartsWith("as", StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new NotImplementedException();
                }

                return ParseIPv6(str);
            }

            return null;
        }


        static string ParseIPv6(string s)
        {
            var p1 = stdlib.strtoul(s, 16);
            //Console.WriteLine(p1);

            if (p1 == 0)
                throw new NoServerException();

            var net = (p1 << 16) + stdlib.strtoul(s.Substring(s.IndexOf(':') + 1), 16);
            var assign = Assignments.IPv6
                .Where(x => x.Item1 == (net & (~0UL << (32 - x.Item2))))
                .FirstOrDefault();

            if (assign != null)
                return assign.Item3;

            throw new UnknownNetworkException();
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
