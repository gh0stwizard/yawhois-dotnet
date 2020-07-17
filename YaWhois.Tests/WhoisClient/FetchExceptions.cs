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
                _whois.Query("example.com", "127.0.0.1:123456");
            });
        }


        [TestCase("127.0.0.1:7")]
        public void SocketException(string server)
        {
            bool gotexception = false;

            try
            {
                _whois.Query("example.com", server);
            }
            catch (AggregateException ae)
            {
                gotexception = ae.InnerExceptions.Any(o => o is SocketException);
            }

            Assert.IsTrue(gotexception);
        }


        [TestCase("127.0.0.1:7")]
        public void ConnectionTimeout(string server)
        {
            bool gotexception = false;

            try
            {
                // This works on Win10. Nothing is known about other cases.
                _whois.ConnectTimeout = 1;
                _whois.Query("example.com", server);
            }
            catch (TimeoutException)
            {
                gotexception = true;
            }

            Assert.IsTrue(gotexception);
        }


        #region QueryAsync

        [Test]
        public async Task BadServerPort_Async()
        {
            bool gotexception = false;

            _whois.WhenExceptionThrown += (o, e) =>
            {
                if (e.Exception is ArgumentOutOfRangeException)
                    gotexception = true;
            };

            await _whois.QueryAsync("example.com", "127.0.0.1:123456");

            Assert.IsTrue(gotexception);
        }


        [TestCase("127.0.0.1:7")]
        public async Task SocketException_Async(string server)
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

            await _whois.QueryAsync("example.com", server);

            Assert.IsTrue(gotexception);
        }


        [TestCase("127.0.0.1:7")]
        public async Task ConnectionTimeout_Async(string server)
        {
            bool gotexception = false;

            _whois.WhenExceptionThrown += (o, args) =>
            {
                gotexception = args.Exception is TimeoutException;
            };

            // This works on Win10. Nothing is known about other cases.
            _whois.ConnectTimeout = 1;
            await _whois.QueryAsync("example.com", server);

            Assert.IsTrue(gotexception);
        }

        #endregion
    }
}
