using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace YaWhois.App
{
    class Program
    {
        static void Main(string[] args)
        {

            var ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            Console.OutputEncoding = Encoding.UTF8;


            var whois = new YaWhoisClient();
            whois.BeforeSendRequest += Whois_BeforeSendRequest;
            //whois.BeforeParseResponse += Whois_BeforeParseResponse;
            whois.ResponseParsed += Whois_ResponseParsed;

            //whois.Query("in-addr.arpa");
            //whois.Query("mail.ru", "whois.iana.org");
            //whois.Query("dk-hostmaster.dk");

            // TODO: create parser??
            //whois.Query("mail.kr"); // restricted, no information
            //whois.Query("한국인터넷정보센터.한국");

            //whois.Query("1.0.0.1", "whois.iana.org");

            //whois.Query("2001:0000:4136:e378:8000:63bf:3fff:fdd2", "whois.iana.org");
            //whois.Query("2001:0000:4136:e378:8000:63bf:3fff:fdd2");

            //whois.Query("mail.bz"); // afilias

            //whois.Query("while.cc"); // crsnic

            //whois.Query("1.0.0.1", "whois.arin.net");
            //whois.Query("as3300", "whois.arin.net");


            TestParser();
        }

        private static void Whois_ResponseParsed(object sender, YaWhoisClientEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Referral))
                Console.WriteLine("[refer: {0}]", e.Referral);

            Console.WriteLine();
            Console.WriteLine(e.Response);
        }

        private static void Whois_BeforeParseResponse(object sender, YaWhoisClientEventArgs e)
        {
            //Console.WriteLine(e.Response);
        }

        private static void Whois_BeforeSendRequest(object sender, EventArgs e)
        {
            YaWhoisClient client = (YaWhoisClient)sender;
            Console.WriteLine("[server: {0}]", client.Server);
            Console.WriteLine("[query: {0}]", client.ServerQuery);
        }


        static void TestParser()
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
                "aS3356",   //Query string: "a 3356"
                "as63779",  //Query string: "-V Md5.5.6 as63779"
                "as131951", //Query string: "AS 131951/e"
            };
            DoTest(asn);


            var puretld = new string[]
            {
                "in",
                "im",
                "pro",
                "de"
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
                "test.bz",
                "mail.de",
                "mail.ru",
                "dk-hostmaster.dk",
                "한국인터넷정보센터.한국"
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
                "1.0.1.1",      // Query string: "-V Md5.5.6 1.0.1.1"
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
        }


        static void DoTest(IEnumerable<string> list)
        {
            var parser = new QueryParser();

            foreach (var el in list)
            {
                try
                {
                    var s = parser.GuessServer(el);
                    Console.WriteLine($"{el} >>> {s.Query}");
                    Console.WriteLine($"    server: {s.Server}");

                    s.FormatQuery();

                    Console.WriteLine($"    server query: {s.ServerQuery}");
                    Console.WriteLine($"    encoding: {s.ServerEncoding.WebName}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{el} ERROR: {e.Message}");
                }

                Console.WriteLine();
            }

            Console.WriteLine(new string('-', 72));
        }
    }
}
