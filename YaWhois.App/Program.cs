using System;
using System.Collections.Generic;

namespace YaWhois.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var ipv6 = new string[]
            {
                "FF01::101",
                "2a00:1450:4010:c01::65",
                "2a00:",
            };

            foreach (var ip in ipv6)
            {
                try
                {
                    var s = QueryParser.GuessServer(ip);
                    Console.WriteLine($"{ip} >>> {s}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{ip} ERROR: {e.Message}");
                }
            }


            var asn = new string[]
            {
                "as3.55",
                "as32000",
                "as65536",
                "as65336"
            };

            foreach (var @as in asn)
            {
                try
                {
                    var s = QueryParser.GuessServer(@as);
                    Console.WriteLine($"{@as} >>> {s}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{@as} ERROR: {e.Message}");
                }
            }

            var t = new Dictionary<string, DictionaryOptions>()
            {
                { "test", new DictionaryOptions {Server = "" } }
            };


            var tldlist = new string[]
            {
                "in",
                "im",
                "pro"
            };

            foreach (var tld in tldlist)
            {
                try
                {
                    var s = QueryParser.GuessServer(tld);
                    Console.WriteLine($"{tld} >>> {s}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{tld} ERROR: {e.Message}");
                }
            }


            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public struct DictionaryOptions
        {
            public string Server;
        }
    }
}
