using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Threading;
using System.IO;

namespace YaWhois.Tests.WhoisClient
{
    public class FetchExceptions
    {
        protected YaWhoisClient _whois;


        private const string UnavailableServer = "127.0.0.1:7";
        private const string LocalServer = "127.0.0.1:8043";
        private const string BadPortServer = "127.0.0.1:123456";
        private const string InvalidPortServer = "127.0.0.1:abc";


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


        [Test]
        public void InvalidServerPort()
        {
            Assert.Throws<FormatException>(delegate
            {
                _whois.Query("example.com", InvalidPortServer);
            });
        }


        [Test]
        public async Task InvalidServerPort_Async()
        {
            bool gotexception = false;

            _whois.WhenExceptionThrown += (o, e) =>
            {
                if (e.Exception is FormatException)
                    gotexception = true;
            };

            await _whois.QueryAsync("example.com", InvalidPortServer);

            Assert.IsTrue(gotexception);
        }


        [Test(Description = "Server does not respond.")]
        public void SocketException()
        {
            bool gotexception = false;

            try
            {
                _whois.Query("example.com", UnavailableServer);
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
                else if (args.Exception is SocketException)
                {
                    gotexception = true;
                }
            };

            await _whois.QueryAsync("example.com", UnavailableServer);

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
                _whois.Query("example.com", UnavailableServer);
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
            await _whois.QueryAsync("example.com", UnavailableServer);

            Assert.IsTrue(gotexception);
        }


        [Test]
        public async Task LocalServerCheck()
        {
            bool success = false;

            var cts = new CancellationTokenSource();
            var ct = cts.Token;

            var server_t = Task.Run(async () => {
                var server = new TestServer();

                server.WhenRequestReceived += (o, args) =>
                {
                    args.Response = args.Request == "hello.com"
                        ? "pass\r\n"
                        : "fail\r\n";
                };

                var pair = LocalServer.Split(new char[] { ':' }, 2);
                var addr = pair[0];
                var port = int.Parse(pair[1]);
                await server.StartListening(ct, addr, port);
            });

            var client_t = Task.Run(async () =>
            {
                await Task.Delay(500);

                try
                {
                    var response = _whois.Query("hello.com", LocalServer);
                    success = response == "pass\r\n";
                }
                finally
                {
                    cts.Cancel();
                }
            });

            await Task.WhenAll(server_t, client_t);
            Assert.IsTrue(success);
        }


        [Test]
        public async Task ReadTimeout()
        {
            bool success = false;

            var cts = new CancellationTokenSource();
            var ct = cts.Token;

            var server_t = Task.Run(async () => {
                var server = new TestServer();

                server.WhenRequestReceived += (o, args) =>
                {
                    args.DelayResponse = 5;
                    args.Response = "client must raise timeout exception";
                };

                var pair = LocalServer.Split(new char[] { ':' }, 2);
                var addr = pair[0];
                var port = int.Parse(pair[1]);
                await server.StartListening(ct, addr, port);
            });

            var client_t = Task.Run(async () =>
            {
                await Task.Delay(500);

                try
                {
                    _whois.ReadWriteTimeout = 2;
                    _whois.Query("example.com", LocalServer);
                }
                catch (IOException)
                {
                    success = true;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    cts.Cancel();
                }
            });

            await Task.WhenAll(server_t, client_t);
            Assert.IsTrue(success);
        }


        [Test]
        public async Task ReadTimeout_Async()
        {
            bool success = false;

            var cts = new CancellationTokenSource();
            var ct = cts.Token;

            var server_t = Task.Run(async () => {
                var server = new TestServer();

                server.WhenRequestReceived += (o, args) =>
                {
                    args.DelayResponse = 5;
                    args.Response = "client must raise timeout exception";
                };

                var pair = LocalServer.Split(new char[] { ':' }, 2);
                var addr = pair[0];
                var port = int.Parse(pair[1]);
                await server.StartListening(ct, addr, port);
            });

            var client_t = Task.Run(async () =>
            {
                await Task.Delay(500);

                _whois.WhenExceptionThrown += (o, e) =>
                {
                    if (e.Exception is TimeoutException)
                        success = true;
                };

                _whois.ReadWriteTimeout = 2;
                await _whois.QueryAsync("example.com", LocalServer);
                cts.Cancel();
            });

            await Task.WhenAll(server_t, client_t);
            Assert.IsTrue(success);
        }
    }
}
