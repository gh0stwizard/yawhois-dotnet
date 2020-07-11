using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace YaWhois.Tests.QueryParser.FormatQuery
{
    public class ASN : BaseClass
    {
        [TestCase("as1")]
        public void Passed16(string value)
        {
            var r = _parser.GuessServer(value).FormatQuery();
            var n = value.Substring(2);
            Assert.AreEqual("a " + n, r.ServerQuery);
        }


        [TestCase("as3.55")]
        public void Passed32(string value)
        {
            var r = _parser.GuessServer(value).FormatQuery();
            Assert.AreEqual(value, r.ServerQuery);
        }
    }
}
