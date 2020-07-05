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
                "as65336",
                "aS3356"
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
                ".рф",
                "ibm.com",
                "test.bz"
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

            var nic = new string[]
            {
                "CREW-RIPE",
                "RIPE-NCC-HM-MNT",
                "NET-148-59-244-0-1"
            };
            DoTest(nic);


            var teredo = new string[]
            {
                "2001:0000:4136:e378:8000:63bf:3fff:fdd2",
            };
            DoTest(teredo);

            var arpa = new string[]
            {
                "in-addr.arpa",
                "ip6.arpa",
                "1.10.78.in-addr.arpa",
                "0.10.78.in-addr.arpa",
                "0.0.10.78.in-addr.arpa",
                "b.a.9.8.7.6.5.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.8.b.d.0.1.0.0.2.ip6.arpa",
                "0.0.a.2.ip6.arpa"
            };
            DoTest(arpa);


            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }


        static void DoTest(IEnumerable<string> list)
        {
            var parser = new QueryParser();

            foreach (var el in list)
            {
                try
                {
                    var s = parser.GuessServer(el);
                    Console.WriteLine($"{el} >>> {s.Server} :: {s.Query}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{el} ERROR: {e.Message}");
                }
            }
        }
    }
}
