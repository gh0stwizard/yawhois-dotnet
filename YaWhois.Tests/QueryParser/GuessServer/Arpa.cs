using System;
using NUnit.Framework;

namespace YaWhois.Tests.QueryParser.GuessServer
{
    public class Arpa : BaseClass
    {
        [TestCase("1.10.78.in-addr.arpa")]
        [TestCase("10.78.in-addr.arpa")]
        [TestCase("78.in-addr.arpa")]
        public void Passed(string value)
        {
            var qp = _parser.GuessServer(value);
            Assert.AreEqual(value, qp.Query);
        }


        [TestCase("0.0.10.78.in-addr.arpa")]
        public void NoServerException(string value)
        {
            Assert.Throws<YaWhois.QueryParser.NoServerException>(delegate {
                _parser.GuessServer(value);
            });
        }
    }
}
