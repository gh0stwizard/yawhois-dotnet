using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace YaWhois.Tests.WhoisClient
{
    public class Events : BaseClass
    {
        [Test]
        public void Query ()
        {
            bool
                BeforeParseResponse_called = false,
                BeforeSendRequest_called = false,
                ResponseParsed_called = false;

            _whois.BeforeSendRequest += (o, e) => BeforeSendRequest_called = true;
            _whois.BeforeParseResponse += (o, e) => BeforeParseResponse_called = true;
            _whois.ResponseParsed += (o, e) => ResponseParsed_called = true;
            _whois.Query("example.com");

            Assert.IsTrue(BeforeSendRequest_called);
            Assert.IsTrue(BeforeParseResponse_called);
            Assert.IsTrue(ResponseParsed_called);
        }


        [Test]
        public async Task QueryAsync()
        {
            bool
                BeforeParseResponse_called = false,
                BeforeSendRequest_called = false,
                ResponseParsed_called = false;

            _whois.BeforeSendRequest += (o, e) => BeforeSendRequest_called = true;
            _whois.BeforeParseResponse += (o, e) => BeforeParseResponse_called = true;
            _whois.ResponseParsed += (o, e) => ResponseParsed_called = true;
            await _whois.QueryAsync("example.com");

            Assert.IsTrue(BeforeSendRequest_called);
            Assert.IsTrue(BeforeParseResponse_called);
            Assert.IsTrue(ResponseParsed_called);
        }


        [Test]
        public async Task QueryAsync_Exception()
        {
            bool called = false;

            _whois.ExceptionThrown += (o, e) => called = true;
            await _whois.QueryAsync("exception.com");

            Assert.IsTrue(called);
        }
    }
}
