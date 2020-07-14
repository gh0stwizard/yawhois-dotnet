using System;
using System.Text;
using YaWhois.Clients;

namespace YaWhois
{
    public class YaWhoisClientEventArgs : EventArgs
    {
        /// <summary>
        /// An user object.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// The parser used to retrieve a referral information.
        /// </summary>
        public IDataParser Parser { get; set; }

        /// <summary>
        /// The WHOIS server used for this query.
        /// </summary>
        public string Server { get; internal set; }

        /// <summary>
        /// The server query, probably re-formatted.
        /// </summary>
        public string Query { get; internal set; }

        /// <summary>
        /// The encoding used by this server.
        /// </summary>
        public Encoding Encoding { get; internal set; }

        /// <summary>
        /// The server response.
        /// </summary>
        public string Response { get; internal set; }

        /// <summary>
        /// The referral server if found.
        /// </summary>
        public string Referral { get; internal set; }

        /// <summary>
        /// The exception thrown by QueryAsync() method.
        /// </summary>
        public Exception Exception { get; internal set; }
    }
}
