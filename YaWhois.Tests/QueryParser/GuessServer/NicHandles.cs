using System;
using NUnit.Framework;

namespace YaWhois.Tests.QueryParser.GuessServer
{
    public class NicHandles : BaseClass
    {
        [TestCase("NET-148-59-244-0-1")]
        public void NoDotButHyphen_ARIN(string value)
        {
            var qp = _parser.GuessServer(value);
            Assert.AreEqual(value, qp.Query);
            Assert.AreEqual("whois.arin.net", qp.Server);
        }


        // Assignments.NicHandleSuffixes
        [TestCase("CREW-RIPE")]
        [TestCase("RIPE-NCC-HM-MNT")]
        public void NoDotButHyphen_RIPE(string value)
        {
            var qp = _parser.GuessServer(value);
            Assert.AreEqual(value, qp.Query);
            Assert.AreEqual("whois.ripe.net", qp.Server);
        }


        // NSI NIC handle
        [TestCase("!nsi")]
        public void ExclamationMark(string value)
        {
            var qp = _parser.GuessServer(value);
            Assert.AreEqual("whois.networksolutions.com", qp.Server);
        }
    }
}
