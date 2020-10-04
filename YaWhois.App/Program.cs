using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using CommandLine;

namespace YaWhois.App
{
    class Program
    {
        static int Main(string[] args)
        {
            var ci = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture =
            CultureInfo.DefaultThreadCurrentUICulture = ci;
            Console.OutputEncoding = Encoding.UTF8;

            var clp = new Parser(o =>
            {
                o.HelpWriter = null;
                o.ParsingCulture = ci;
                o.AutoHelp = true;
                o.AutoVersion = true;
            });

            return clp.ParseArguments<Options>(args)
                .MapResult((opts) => RunWhois(opts), errs => PrintHelp(errs));
        }


        private static int PrintHelp(IEnumerable<Error> errs)
        {
            if (errs.Any(e => e is VersionRequestedError))
            {
                var a = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
                var version = a.GetName().Version.ToString();
                Console.WriteLine($"Version {version}");
                return 0;
            }

            const string optfmt = "  {0,-22} {1}";

            Console.WriteLine("Usage: yawhois [OPTION] OBJECT");
            Console.WriteLine();
            Console.WriteLine(optfmt, "OBJECT", "an object to query");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine(optfmt, "-s, --server SERVER", "connect to SERVER");
            Console.WriteLine(optfmt, "-I, --iana", "query whois.iana.org and follow its referral");
            Console.WriteLine(optfmt, "-v, --verbose", "print additional information");
            Console.WriteLine(optfmt, "--help", "print this help");
            Console.WriteLine(optfmt, "--version", "print program version");

            return 1;
        }


        private static int RunWhois(Options o)
        {
            var whois = new YaWhoisClient();
            whois.WhenResponseParsed += Whois_ResponseParsed;

            if (o.UseIANA)
                o.Server = "whois.iana.org";

            try
            {
                whois.Query(o.Object, o.Server, o.Verbose);
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 2;
            }
        }


        private static void Whois_ResponseParsed(object sender, YaWhoisClientEventArgs e)
        {
            var verbose = (bool)e.Value;

            if (verbose)
            {
                Console.WriteLine($"[server: {e.Server}]");
                Console.WriteLine($"[query: {e.Query}]");
                Console.WriteLine($"[encoding: {e.Encoding.WebName}]");

                if (!string.IsNullOrEmpty(e.Referral))
                    Console.WriteLine($"[referral: {e.Referral}]");

                Console.WriteLine();
            }

            Console.WriteLine(e.Response.TrimEnd(new char[] { '\r', '\n' }));
            Console.WriteLine();
        }
    }
}
