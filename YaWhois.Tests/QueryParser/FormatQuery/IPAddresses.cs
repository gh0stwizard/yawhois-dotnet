using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace YaWhois.Tests.QueryParser.FormatQuery
{
    public class IPAddresses : BaseClass
    {
        [TestCase("3.0.0.0")]
        [TestCase("45.72.0.0")]
        public void ARIN_ipv4(string value)
        {
            var qp = _parser.GuessServer(value).FormatQuery();
            Assert.AreEqual("whois.arin.net", qp.Server);
            Assert.AreEqual($"n + {value}", qp.ServerQuery);
        }


        [TestCase("1.0.0.0")]
        [TestCase("150.0.0.0")]
        public void APNIC_ipv4(string value)
        {
            var qp = _parser.GuessServer(value).FormatQuery();
            Assert.AreEqual("whois.apnic.net", qp.Server);
            Assert.AreEqual(value, qp.ServerQuery);
        }


        [TestCase("2.0.0.0")]
        [TestCase("139.20.0.0")]
        public void RIPE_ipv4(string value)
        {
            var qp = _parser.GuessServer(value).FormatQuery();
            Assert.AreEqual("whois.ripe.net", qp.Server);
            Assert.AreEqual(value, qp.ServerQuery);
        }


        [TestCase("41.0.0.0")]
        [TestCase("154.0.0.0")]
        public void AFRINIC_ipv4(string value)
        {
            var qp = _parser.GuessServer(value).FormatQuery();
            Assert.AreEqual("whois.afrinic.net", qp.Server);
            Assert.AreEqual(value, qp.ServerQuery);
        }


        [TestCase("61.112.0.0")]
        [TestCase("153.128.0.0")]
        public void Nic_ad_jp_ipv4(string value)
        {
            var qp = _parser.GuessServer(value).FormatQuery();
            Assert.AreEqual("whois.nic.ad.jp", qp.Server);
            Assert.AreEqual($"{value}/e", qp.ServerQuery);
        }


        [TestCase("10.0.0.0/8")]
        [TestCase("8.0.0.0/8")]
        [TestCase("8.0.0.0/9")]
        public void CIDR_ipv4(string value)
        {
            var qp = _parser.GuessServer(value).FormatQuery();
            Assert.AreEqual("whois.arin.net", qp.Server);
            Assert.AreEqual("r + = " + value, qp.ServerQuery);
        }
    }
}
