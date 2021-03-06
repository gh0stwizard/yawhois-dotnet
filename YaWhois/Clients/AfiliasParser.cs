﻿using System;
using System.IO;

namespace YaWhois.Clients
{
    public class AfiliasParser : IDataParser
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
