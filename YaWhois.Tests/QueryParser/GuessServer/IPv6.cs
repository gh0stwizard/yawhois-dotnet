using System;
using NUnit.Framework;

namespace YaWhois.Tests.QueryParser.GuessServer
{
    public class IPv6 : BaseClass
    {
        [TestCase("2a00:1450:4010:c01::65")]
        [TestCase("2a00:")]
        public void Passed(string value)
        {
            var qp = _parser.GuessServer(value);
            Assert.AreEqual(value, qp.Query);
        }


        [TestCase("FF01::101")]
        [TestCase("::")]
        public void ArgumentException(string value)
        {
            Assert.Throws<ArgumentException>(delegate {
                _parser.GuessServer(value);
            });
        }


        [TestCase("2a00")]
        public void NoServerException(string value)
        {
            Assert.Throws<YaWhois.QueryParser.NoServerException>(delegate {
                _parser.GuessServer(value);
            });
        }
    }
}