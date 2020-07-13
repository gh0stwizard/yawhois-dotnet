using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace YaWhois.Tests.QueryParser.FormatQuery
{
    public class Domains : BaseClass
    {
        [TestCase("mail.de")]
        public void Passed_denic_de(string value)
        {
            var qp = _parser.GuessServer(value).FormatQuery();
            var rex = $"\\s*-T dn(,ace)? {value}$";
            Assert.True(Regex.IsMatch(qp.ServerQuery, rex));
        }


        [TestCase("dk-hostmaster.dk")]
        public void Passed_dk_hostmaster_dk(string value)
        {
            var qp = _parser.GuessServer(value).FormatQuery();
            Assert.True(qp.ServerQuery.Contains("--show-handles"));
        }


        [TestCase("jprs.jp")]
        public void Passed_jprs_jp(string value)
        {
            var qp = _parser.GuessServer(value).FormatQuery();
            Assert.True(qp.ServerQuery.EndsWith("/e"));
        }
    }
}
