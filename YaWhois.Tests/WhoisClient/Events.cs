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
        public void Query()
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
        public async Task QueryAsync_ExceptionThrown()
        {
            bool called = false;

            _whois.ExceptionThrown += (o, e) => called = true;
            await _whois.QueryAsync("exception.com");

            Assert.IsTrue(called);
        }


        [Test]
        public async Task QueryAsync_Delegate_Exception()
        {
            bool called = false;

            _whois.ResponseParsed += (o, e) =>
            {
                throw new SuccessException("passed");
            };

            _whois.ExceptionThrown += (o, e) =>
            {
                if (e.Exception is SuccessException)
                    called = true;
            };

            await _whois.QueryAsync("ya.ru");

            Assert.IsTrue(called);
        }


        [Test]
        public async Task QueryAsync_Canceled_Before_Query()
        {
            bool called = false;

            _whois.ExceptionThrown += (o, e) =>
            {
                if (e.Exception is OperationCanceledException)
                    called = true;
            };

            var cts = new CancellationTokenSource();
            cts.Cancel();

            await _whois.QueryAsync("ya.ru", token: cts.Token);

            Assert.IsTrue(called);
        }


        [Test]
        public async Task QueryAsync_Canceled_When_Fetch()
        {
            bool called = false;

            _whois.ExceptionThrown += (o, e) =>
            {
                if (e.Exception is OperationCanceledException)
                    called = true;
            };

            var cts = new CancellationTokenSource();
            var qtask = _whois.QueryAsync("delay10s.com", token: cts.Token);
            var delay = Task.Run(async () => {
                await Task.Delay(1000);
                cts.Cancel();
            });

            await Task.WhenAll(qtask, delay);

            Assert.IsTrue(called);
        }
    }
}
