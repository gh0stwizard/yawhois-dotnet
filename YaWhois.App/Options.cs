using System;
using CommandLine;

namespace YaWhois.App
{
    class Options
    {
        [Option('s', "server")]
        public string Server { get; set; }

        [Option('I', "iana", Default = false)]
        public bool UseIANA { get; set; }

        [Option('v', "verbose", Default = false)]
        public bool Verbose { get; set; }

        [Value(0, Required = true)]
        public string Object { get; set; }
    }
}
