using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace YaWhois.Tests.WhoisClient
{
    public class Threads
    {
        protected YaWhoisClient _whois;
        private const string LocalServer = "127.0.0.1:8043";


        [SetUp]
        public void Setup()
        {
            _whois = new YaWhoisClient();
        }


        [TestCase(5, 2, 10)]
        public async Task TimeoutsRace(int serverDelay, int timeout1, int timeout2)
        {
            bool c1_result = false;
            bool c2_result = false;

            var cts = new CancellationTokenSource();
            var ct = cts.Token;

            var server_t = Task.Run(async () => {
                var server = new TestServer();

                server.WhenRequestReceived += (o, args) =>
                {
                    args.DelayResponse = serverDelay;
                    args.Response = args.Request;
                };

                var pair = LocalServer.Split(new char[] { ':' }, 2);
                var addr = pair[0];
                var port = int.Parse(pair[1]);
                await server.StartListening(ct, addr, port);
            });

            _whois.WhenRequestReady += (o, e) =>
            {
                var whois = (YaWhoisClient)o;
                whois.ReadWriteTimeout = e.Query.Contains("example1")
                    ? timeout1
                    : timeout2;
            };

            _whois.WhenExceptionThrown += (o, e) =>
            {
                if (e.Query.Contains("example1"))
                    c1_result = true;
                if (e.Query.Contains("example2"))
                    c2_result = true;
            };

            var c1 = Task.Run(async () =>
            {
                await Task.Delay(500);
                await _whois.QueryAsync("example1.com", LocalServer);
            });

            var c2 = Task.Run(async () =>
            {
                await Task.Delay(500);
                await _whois.QueryAsync("example2.com", LocalServer);
            });

            await Task.WhenAll(c1, c2);
            cts.Cancel();
            await server_t;

            Assert.IsTrue(c1_result);
            Assert.IsFalse(c2_result);
        }
    }
}
