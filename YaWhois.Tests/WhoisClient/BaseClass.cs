using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace YaWhois.Tests.WhoisClient
{
    public class BaseClass
    {
        protected YaWhoisClient _whois;


        [SetUp]
        public void Setup()
        {
            _whois = new TestClient();
        }
    }
}
