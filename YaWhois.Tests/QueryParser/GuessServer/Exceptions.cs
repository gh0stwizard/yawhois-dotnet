using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;

namespace YaWhois.Tests.QueryParser.GuessServer
{
    public class Exceptions : BaseClass
    {
        [TestCase(null)]
        [TestCase("")]
        public void NullEmptyException(string value)
        {
            Assert.Throws<ArgumentException>(delegate
            {
                _parser.GuessServer(value);
            });
        }


        [TestCase("test@example.com", Description = "Email addresses not allowed")]
        [TestCase("32000", Description = "No 'AS' prefix")]
        public void NoServerExceptionTests(string value)
        {
            Assert.Throws<YaWhois.NoServerException>(delegate
            {
                _parser.GuessServer(value);
            });
        }
    }
}
