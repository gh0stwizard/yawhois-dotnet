using System;

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

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
