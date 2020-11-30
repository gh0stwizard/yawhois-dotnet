using System;
using System.Linq;
using NUnit.Framework;

namespace YaWhois.Tests.QueryParser.GuessServer
{
    public class Domains : BaseClass
    {
        [TestCase("com")]
        [TestCase("net")]
        [TestCase("org")]
        [TestCase("pro")]
        [TestCase("company")]
        public void TLD_iana(string value)
        {
            var qp = _parser.GuessServer(value);
            Assert.AreEqual("whois.iana.org", qp.Server);
            Assert.AreEqual(value, qp.Query);
        }


        [TestCase("redmond.company")]
        [TestCase("dig.watch")]
        public void gTLD(string value)
        {
            var qp = _parser.GuessServer(value);
            var domain = value.Split('.').Last();
            Assert.AreEqual($"whois.nic.{domain}", qp.Server);
            Assert.AreEqual(value, qp.Query);
        }


        [TestCase("испытание.рф", Description = "Russian test domain")]
        public void Punycode_RU_01(string value)
        {
            var qp = _parser.GuessServer(value);
            Assert.AreEqual("whois.tcinet.ru", qp.Server);
            Assert.AreEqual("xn--80akhbyknj4f.xn--p1ai", qp.Query);
        }


        [TestCase("한국인터넷정보센터.한국", Description = "Korean")]
        public void Punycode_KR_01(string value)
        {
            var qp = _parser.GuessServer(value);
            Assert.AreEqual("whois.kr", qp.Server);
            Assert.AreEqual("xn--3e0bx5euxnjje69i70af08bea817g.xn--3e0b707e", qp.Query);
        }


        [TestCase("ଭାରତ.com")]
        public void Punycode_dot_com_01(string value)
        {
            var qp = _parser.GuessServer(value);
            Assert.AreEqual("whois.verisign-grs.com", qp.Server);
            Assert.AreEqual("xn--3hcrj9c.com", qp.Query);
        }


        [TestCase(".jp")]
        [TestCase("a")]
        public void NoServerException(string value)
        {
            Assert.Throws<YaWhois.NoServerException>(delegate
            {
                _parser.GuessServer(value);
            });
        }


        [TestCase("mail.az")]
        public void ExternalWhoisException(string value)
        {
            Assert.Throws<YaWhois.ExternalWhoisException>(delegate
            {
                _parser.GuessServer(value);
            });
        }
    }
}
