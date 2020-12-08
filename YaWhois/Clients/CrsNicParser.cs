using System;
using System.IO;

namespace YaWhois.Clients
{
    public class CrsNicParser : IDataParser
    {
        private static readonly string DomainHint = "   Domain Name:";
        private static readonly string ServerHint = "   Server Name:";
        private static readonly string WhoisHint = "   Registrar WHOIS Server:";


        public string GetReferral(in string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            int state = 0;
            using (var r = new StringReader(text))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    if (state == 0)
                    {
                        if (line.StartsWith(DomainHint))
                        {
                            state = 1;
                            continue;
                        }

                        if (line.StartsWith(ServerHint))
                            break; // return null
                    }
                    else if (state == 1)
                    {
                        if (line.StartsWith(WhoisHint))
                            return line.Substring(WhoisHint.Length + 1).Trim();
                    }
                }
            }

            return null;
        }
    }
}
