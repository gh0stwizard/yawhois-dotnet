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
            DoTest(ipv6);


            var asn = new string[]
            {
                "as3.55",
                "as32000",
                "as65536",
                "as65336"
            };
            DoTest(asn);


            var puretld = new string[]
            {
                "in",
                "im",
                "pro"
            };
            DoTest(puretld);


            var tld = new string[]
            {
                "ଭାରତ.ଭାରତ",
                ".ଭାରତ",
                "central.no.com",
                "casual.com",
                "испытание.рф",
                ".рф"
            };
            DoTest(tld);

            var gtld = new string[]
            {
                "test.bank"
            };
            DoTest(gtld);

            var ip4 = new string[]
            {
                "198.17.79.5",
                "0.0.1.1",
                "1.0.1.1",
            };
            DoTest(ip4);


            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }


        static void DoTest(IEnumerable<string> list)
        {
            foreach (var el in list)
            {
                try
                {
                    var s = QueryParser.GuessServer(el);
                    Console.WriteLine($"{el} >>> {s}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{el} ERROR: {e.Message}");
                }
            }
        }
    }
}
