using System;
using NUnit.Framework;

namespace YaWhois.Tests.QueryParser.GuessServer
{
    public class DomainsTLD : BaseClass
    {
        [TestCase("com")]
        public void Passed(string value)
        {
            var qp = _parser.GuessServer(value);
            Assert.AreEqual(value, qp.Query);
        }


        [TestCase(".jp")]
        public void NoServerException(string value)
        {
            Assert.Throws<YaWhois.QueryParser.NoServerException>(delegate {
                _parser.GuessServer(value);
            });
        }
    }
}
