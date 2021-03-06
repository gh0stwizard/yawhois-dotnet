﻿using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace YaWhois.Tests.QueryParser.FormatQuery
{
    public class Teredo : BaseClass
    {
        [TestCase("2001:0000:4136:e378:8000:63bf:3fff:fdd2")]
        [TestCase("2001:0000:4136:e378:8000:63bf:3fff:fdd2:x",
            Ignore = "do we really need this?",
            IgnoreReason = "Invalid IPv6 format string")]
        public void Passed_01(string value)
        {
            var qp = _parser.GuessServer(value).FormatQuery();
            Assert.AreEqual("n + 192.0.2.45", qp.ServerQuery);
        }


        [TestCase("2001:0000:4136:e378:8000:63bf:3fff:")]
        [TestCase("2001:0000:4136:e378:8000:63bf:3fff::")]
        [TestCase("2001:0000:4136:e378:8000:63bf:3fff")]
        [TestCase("2001:0000:4136:e378:8000:63bf:3fff:x")]
        [TestCase("2001:0000:4136:e378:8000:63bf:3fff:fdd22")]
        public void NoServerException(string value)
        {
            Assert.Throws<YaWhois.NoServerException>(delegate {
                _parser.GuessServer(value).FormatQuery();
            });
        }
    }
}
