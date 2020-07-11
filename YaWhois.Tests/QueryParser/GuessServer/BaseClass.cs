using System;
using NUnit.Framework;

namespace YaWhois.Tests.QueryParser.GuessServer
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
