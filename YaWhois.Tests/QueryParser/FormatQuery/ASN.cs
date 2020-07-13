using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace YaWhois.Tests.QueryParser.FormatQuery
{
    public class ASN : BaseClass
    {
        [TestCase("as1")]
        public void Passed_arin(string value)
        {
            var qp = _parser.GuessServer(value).FormatQuery();
            var n = value.Substring(2);
            Assert.AreEqual("a " + n, qp.ServerQuery);
        }


        [TestCase("as3.55")]
        public void Passed_ripe(string value)
        {
            var qp = _parser.GuessServer(value).FormatQuery();
            Assert.AreEqual(value, qp.ServerQuery);
        }


        [TestCase("as131951")]
        public void Passed_nic_ad_jp(string value)
        {
            var qp = _parser.GuessServer(value).FormatQuery();
            var n = value.Substring(2);
            Assert.True(qp.ServerQuery.EndsWith($"{n}/e"));
        }
    }
}
