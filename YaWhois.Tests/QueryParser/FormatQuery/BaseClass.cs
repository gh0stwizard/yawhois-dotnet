using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace YaWhois.Tests.QueryParser.FormatQuery
{
    public class BaseClass
    {
        protected YaWhois.QueryParser _parser;


        [SetUp]
        public void Setup()
        {
            _parser = new YaWhois.QueryParser();
        }
    }
}
