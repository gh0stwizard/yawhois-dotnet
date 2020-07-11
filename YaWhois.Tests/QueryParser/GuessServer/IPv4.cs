using System;
using NUnit.Framework;

namespace YaWhois.Tests.QueryParser.GuessServer
{
    public class IPv4 : BaseClass
    {
        [TestCase("198.17.79.5")]
        [TestCase("1.0.1.1")]
        public void Passed(string value)
        {
            var r = _parser.GuessServer(value);
            Assert.AreEqual(value, r.Query);
        }


        [TestCase("0.0.1.1")]
        [TestCase("255.255.255.255")]
        [TestCase("300.1.2.3")]
        public void NoServerException(string value)
        {
            Assert.Throws<YaWhois.QueryParser.NoServerException>(delegate {
                _parser.GuessServer(value);
            });
        }
    }
}
