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


        /// <summary>
        /// Sets flags supported by RIPE-like servers.
        /// </summary>
        /// <param name="obj"></param>
        public void SetRipeFlags(YaWhoisRipeFlags obj)
        {
            if (Query.Length == 0)
                return;

            if (obj.HasFlag(YaWhoisRipeFlags.BRIEF))
                Query = "-b " + Query;

            if (obj.HasFlag(YaWhoisRipeFlags.EXACT))
                Query = "-x " + Query;

            if (obj.HasFlag(YaWhoisRipeFlags.REVERSE_DOMAIN))
                Query = "-d " + Query;

            if (obj.HasFlag(YaWhoisRipeFlags.NO_FILTERING))
                Query = "-B " + Query;

            if (obj.HasFlag(YaWhoisRipeFlags.NO_GROUPING))
                Query = "-G " + Query;
        }
    }
}
