using System;
using System.IO;
using System.Text.RegularExpressions;

namespace YaWhois.Clients
{
    public class DefaultParser : IDataParser
    {
        private static readonly string ArinHint = "ReferralServer:";
        private static readonly string ReferToHint = "% referto:";

        private static readonly Regex GetReferral_Regex01 = new Regex(
            "^% referto: whois -h ([^\\s]{1,255}) -p ([^\\s]{1,15})",
            RegexOptions.Compiled);
        private static readonly Regex GetReferral_Regex02 = new Regex(
            "^r?whois://",
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
                        var m = GetReferral_Regex01.Match(line);
                        if (m.Success)
                            return m.Groups[1].Value + ":" + m.Groups[2].Value;
                    }

                    if (line.StartsWith(ArinHint))
                    {
                        var refstr = line.Substring(ArinHint.Length + 1).Trim();
                        return GetReferral_Regex02.Replace(refstr, "").TrimEnd(new char[] { '/' });
                    }
                }
            }

            return null;
        }
    }
}
