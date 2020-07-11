using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace YaWhois.Clients
{
    class DefaultParser : IDataParser
    {
        private static readonly string ArinHint = "ReferralServer:";
        private static readonly string ReferToHint = "% referto:";


        public string GetReferral(in string text)
        {
            using (var r = new StringReader(text))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    // TODO: need to find a real example of this case.
                    if (line.StartsWith(ReferToHint))
                    {
                        var r1 = new Regex("^% referto: whois -h ([^\\s]{1,255}) -p ([^\\s]{1,15})");
                        var m1 = r1.Match(line);
                        if (m1.Success)
                            return m1.Groups[1].Value + ":" + m1.Groups[2].Value;
                    }

                    if (line.StartsWith(ArinHint))
                    {
                        var refstr = line.Substring(ArinHint.Length + 1).Trim();
                        return Regex.Replace(refstr, "^r?whois://", "").TrimEnd(new char[] { '/' });
                    }
                }
            }

            return null;
        }
    }
}
