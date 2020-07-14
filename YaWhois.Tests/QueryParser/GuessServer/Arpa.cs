using System;
using NUnit.Framework;

namespace YaWhois.Tests.QueryParser.GuessServer
{
    public class Arpa : BaseClass
    {
        [TestCase("1.10.78.in-addr.arpa")]
        [TestCase("10.78.in-addr.arpa")]
        [TestCase("78.in-addr.arpa")]
        public void Passed4(string value)
        {
            var qp = _parser.GuessServer(value);
            Assert.AreEqual(value, qp.Query);
        }


        [TestCase("b.a.9.8.7.6.5.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.8.b.d.0.1.0.0.2.ip6.arpa")]
        public void Passed6(string value)
        {
            var qp = _parser.GuessServer(value);
            Assert.AreEqual(value, qp.Query);
        }


        [TestCase("0.0.10.78.in-addr.arpa")]
        [TestCase("0.a.2.ip6.arpa")]
        [TestCase("0.0.a.2.ip6.arpa")]
        [TestCase("1.0.0.2.ip6.arpa")]
        [TestCase("0.1.0.0.2.ip6.arpa", Description = "Teredo 0.0.0.0")]
        public void NoServerException(string value)
        {
            Assert.Throws<YaWhois.NoServerException>(delegate {
                _parser.GuessServer(value);
            });
        }
    }
}
