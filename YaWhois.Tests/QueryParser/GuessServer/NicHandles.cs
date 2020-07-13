using System;
using NUnit.Framework;

namespace YaWhois.Tests.QueryParser.GuessServer
{
    public class NicHandles : BaseClass
    {
        [TestCase("CREW-RIPE")]
        [TestCase("RIPE-NCC-HM-MNT")]
        [TestCase("NET-148-59-244-0-1")]
        public void Passed(string value)
        {
            var qp = _parser.GuessServer(value);
            Assert.AreEqual(value, qp.Query);
        }
    }
}
