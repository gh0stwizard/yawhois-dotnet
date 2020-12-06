using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace YaWhois.Tests.WhoisClient
{
    public class WhoisClientTests : Clients.WhoisClient
    {
        private WhoisClientTests _client;
        private const string UnavailableServer = "127.0.0.1:7";
        private const string LocalServer = "127.0.0.1:8043";


        [SetUp]
        public void Setup()
        {
            _client = new WhoisClientTests();
        }


        [TestCase(null, "github.com")]
        [TestCase("", "github.com")]
        [TestCase("whois.iana.org", null)]
        [TestCase("whois.iana.org", "")]
        public void NullOrEmptyServerOrQuery(string server, string query)
        {
            Assert.Throws<ArgumentNullException>(delegate
            {
                _client.Fetch(server, query, Encoding.ASCII);
            });
        }


        [TestCase(null, "github.com")]
        [TestCase("", "github.com")]
        [TestCase("whois.iana.org", null)]
        [TestCase("whois.iana.org", "")]
        public async Task NullOrEmptyServerOrQueryAsync(string server, string query)
        {
            var success = false;

            try
            {
                var ct = new CancellationToken();
                await _client.FetchAsync(server, query, Encoding.ASCII, ct);
            }
            catch (ArgumentNullException)
            {
                success = true;
            }

            Assert.IsTrue(success);
        }


        [TestCase("whois.iana.org:abc")]
        public void InvalidServerPort(string server)
        {
            Assert.Throws<FormatException>(delegate
            {
                _client.Fetch(server, "github.com", Encoding.ASCII);
            });
        }


        [TestCase("whois.iana.org:abc")]
        public async Task InvalidServerPortAsync(string server)
        {
            var success = false;

            try
            {
                var ct = new CancellationToken();
                await _client.FetchAsync(server, "github.com", Encoding.ASCII, ct);
            }
            catch (FormatException)
            {
                success = true;
            }

            Assert.IsTrue(success);
        }


        // This works on Win10. Nothing is known about other cases.
        [TestCase(IncludePlatform = "Win")]
        public async Task ConnectionCanceledAsync()
        {
            bool gotexception = false;
            var cts = new CancellationTokenSource();
            var ct = cts.Token;

            try
            {
                var cancel_t = Task.Run(async () => {
                    await Task.Delay(100);
                    cts.Cancel();
                });

                var fetch_t = Task.Run(async () => {
                    await Task.Delay(500);
                    await _client.FetchAsync(UnavailableServer, "github.com", Encoding.ASCII, ct);
                });

                await Task.WhenAll(cancel_t, fetch_t);
            }
            catch (TaskCanceledException)
            {
                gotexception = true;
            }

            Assert.IsTrue(gotexception);
        }


        [Test]
        public async Task ReadCanceledAsync()
        {
            var success = false;
            var server_cts = new CancellationTokenSource();
            var server_ct = server_cts.Token;

            var server_t = Task.Run(async () =>
            {
                var server = new TestServer();

                server.WhenRequestReceived += (o, e) =>
                {
                    e.SplitResponse = true;
                    e.SplitResponseChunks = 3;
                    e.SplitResponseDelay = 1000;
                    e.Response = "string response longer than amount of chunks above";
                };

                var pair = LocalServer.Split(new char[] { ':' }, 2);
                var addr = pair[0];
                var port = int.Parse(pair[1]);
                await server.StartListening(server_ct, addr, port);
            });

            var client_cts = new CancellationTokenSource();
            var client_ct = client_cts.Token;

            var client_t = Task.Run(async () =>
            {
                await Task.Delay(250); // wait a bit for the server

                try
                {
                    await _client.FetchAsync(LocalServer, "ya.ru", Encoding.UTF8, client_ct);
                }
                catch (TaskCanceledException)
                {
                    success = true;
                }
                finally
                {
                    // shutdown the server
                    server_cts.Cancel();
                }
            });

            var cancel_t = Task.Run(async () =>
            {
                // this delay must be greater than SplitResponseDelay
                await Task.Delay(1500);
                client_cts.Cancel();
            });

            await Task.WhenAll(server_t, client_t);
            Assert.IsTrue(success);
        }
    }
}
