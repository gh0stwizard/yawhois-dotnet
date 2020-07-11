using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace YaWhois.Clients
{
    class AfiliasParser : IDataParser
    {
        private static readonly string Hint = "Registrar WHOIS Server:";


        public string GetReferral(in string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            using (var r = new StringReader(text))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    if (line.StartsWith(Hint))
                        return line.Substring(Hint.Length + 1).Trim();
                }
            }

            return null;
        }
    }
}
