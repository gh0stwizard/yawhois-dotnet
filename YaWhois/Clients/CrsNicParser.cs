using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace YaWhois.Clients
{
    class CrsNicParser : IDataParser
    {
        private static readonly string DomainHint = "   Domain Name:";
        private static readonly string ServerHint = "   Server Name:";
        private static readonly string WhoisHint = "   Registrar WHOIS Server:";


        public string GetReferral(in string text)
        {
            int state = 0;

            using (var r = new StringReader(text))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    if (state == 0 && line.StartsWith(DomainHint))
                    {
                        state = 1;
                        continue;
                    }
                    else if (state == 0 && line.StartsWith(ServerHint))
                    {
                        state = 2;
                        return string.Empty;
                    }

                    if (state == 1 && line.StartsWith(WhoisHint))
                        return line.Substring(WhoisHint.Length + 1).Trim();
                }
            }

            return null;
        }
    }
}
