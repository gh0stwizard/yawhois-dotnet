using System;
using NUnit.Framework;

namespace YaWhois.Tests.QueryParser.GuessServer
{
    public class ASN : BaseClass
    {
        [TestCase("as3.55")]
        [TestCase("as1")]
        [TestCase("as131951")]
        public void Passed(string value)
        {
            var r = _parser.GuessServer(value);
            Assert.AreEqual(value, r.Query);
        }


        [TestCase("as0")]
        [TestCase("as65535")]
        public void UnknownNetworkException(string value)
        {
            Assert.Throws<YaWhois.QueryParser.UnknownNetworkException>(delegate {
                _parser.GuessServer(value);
            });
        }
    }
}
