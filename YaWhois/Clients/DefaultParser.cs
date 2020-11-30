using System;
using System.IO;
using System.Text.RegularExpressions;

namespace YaWhois.Clients
{
    class DefaultParser : IDataParser
    {
        private static readonly string ArinHint = "ReferralServer:";
        private static readonly string ReferToHint = "% referto:";

        private static readonly Regex GetReferral_Regex = new Regex(
            "^% referto: whois -h ([^\\s]{1,255}) -p ([^\\s]{1,15})",
            RegexOptions.Compiled);


        public string GetReferral(in string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            using (var r = new StringReader(text))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    // TODO: need to find a real example of this case.
                    if (line.StartsWith(ReferToHint))
                    {
                        var m = GetReferral_Regex.Match(line);
                        if (m.Success)
                            return m.Groups[1].Value + ":" + m.Groups[2].Value;
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
