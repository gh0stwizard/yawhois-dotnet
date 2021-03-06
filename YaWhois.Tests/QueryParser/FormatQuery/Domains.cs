﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace YaWhois.Tests.QueryParser.FormatQuery
{
    public class Domains : BaseClass
    {
        [Test(Description = "Dumb case: call FormatQuery() before GuessServer().")]
        public void NullEmptyException()
        {
            Assert.Throws<ArgumentNullException>(delegate
            {
                _parser.FormatQuery();
            });
        }


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


        [TestCase("b.a.9.8.7.6.5.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.8.b.d.0.1.0.0.2.ip6.arpa")]
        public void Passed_ip6_arpa(string value)
        {
            var qp = _parser.GuessServer(value).FormatQuery();
            Assert.AreEqual(value, qp.ServerQuery);
        }


        [TestCase("unusual-case")]
        public void ARIN_NotRipe_NotASN_NotIP(string value)
        {
            var qp = _parser.GuessServer(value).FormatQuery();
            Assert.AreEqual(value, qp.ServerQuery);
            Assert.AreEqual("whois.arin.net", qp.Server);
        }
    }
}
