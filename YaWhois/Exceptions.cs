using System;
using System.Collections.Generic;
using System.Text;

namespace YaWhois
{
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


    public class ExternalWhoisException : Exception
    {
        /// <summary>
        /// You can access the whois database at {source}.
        /// </summary>
        /// <param name="source"></param>
        public ExternalWhoisException(string source)
            : base(string.Format("You can access the whois database at {0}", source))
        { }
    }
}
