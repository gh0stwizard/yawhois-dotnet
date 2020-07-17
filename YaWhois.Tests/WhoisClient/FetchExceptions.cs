using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using NUnit.Framework;

namespace YaWhois.Tests.WhoisClient
{
    public class FetchExceptions
    {
        protected YaWhoisClient _whois;


        private const string TestServer = "127.0.0.1:7";
        private const string BadPortServer = "127.0.0.1:123456";


        [SetUp]
        public void Setup()
        {
            _whois = new YaWhoisClient();
        }


        [Test]
        public void BadServerPort()
        {
            Assert.Throws<ArgumentOutOfRangeException>(delegate
            {
                _whois.Query("example.com", BadPortServer);
            });
        }


        [Test]
        public async Task BadServerPort_Async()
        {
            bool gotexception = false;

            _whois.WhenExceptionThrown += (o, e) =>
            {
                if (e.Exception is ArgumentOutOfRangeException)
                    gotexception = true;
            };

            await _whois.QueryAsync("example.com", BadPortServer);

            Assert.IsTrue(gotexception);
        }



        [Test(Description = "Server does not respond.")]
        public void SocketException()
        {
            bool gotexception = false;

            try
            {
                _whois.Query("example.com", TestServer);
            }
            catch (AggregateException ae)
            {
                gotexception = ae.InnerExceptions.Any(o => o is SocketException);
            }

            Assert.IsTrue(gotexception);
        }


        [Test(Description = "Server does not respond.")]
        public async Task SocketException_Async()
        {
            bool gotexception = false;

            _whois.WhenExceptionThrown += (o, args) =>
            {
                if (args.Exception is AggregateException)
                {
                    var ae = (AggregateException)args.Exception;
                    gotexception = ae.InnerExceptions.Any(e => e is SocketException);
                }
            };

            await _whois.QueryAsync("example.com", TestServer);

            Assert.IsTrue(gotexception);
        }


        // This works on Win10. Nothing is known about other cases.
        [TestCase(IncludePlatform = "Win")]
        public void ConnectionTimeout()
        {
            bool gotexception = false;

            try
            {
                _whois.ConnectTimeout = 1;
                _whois.Query("example.com", TestServer);
            }
            catch (TimeoutException)
            {
                gotexception = true;
            }

            Assert.IsTrue(gotexception);
        }


        [TestCase(IncludePlatform = "Win")]
        public async Task ConnectionTimeout_Async()
        {
            bool gotexception = false;

            _whois.WhenExceptionThrown += (o, args) =>
            {
                gotexception = args.Exception is TimeoutException;
            };

            _whois.ConnectTimeout = 1;
            await _whois.QueryAsync("example.com", TestServer);

            Assert.IsTrue(gotexception);
        }
    }
}
