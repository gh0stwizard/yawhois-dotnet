using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace YaWhois.Tests.WhoisClient
{
    class Events_SetQueryFlags : BaseClass
    {
        [TestCase("80.80.80.80")]
        public void Ripe_ObjectFiltering(string obj)
        {
            bool filtered = true, passed = false;

            _whois.WhenRequestReady += (o, e) =>
            {
                if (!filtered)
                    e.SetRipeFlags(YaWhoisRipeFlags.NO_FILTERING);
            };

            _whois.WhenResponseParsed += (o, e) =>
            {
                if (string.IsNullOrEmpty(e.Response))
                    return;

                if (e.Response.Contains("filtered@example.com"))
                {
                    // catch case for second Query() call
                    if (!filtered)
                        passed = true;
                }
                else if (filtered)
                {
                    // switch to different db on first Query() call
                    filtered = false;
                }
            };

            _whois.Query(obj);
            ((TestClient)_whois).SetResourceMap(typeof(Resources.RIPE_NotFiltered));
            _whois.Query(obj);

            // reset for next tests
            ((TestClient)_whois).ResetResourceMap();

            Assert.IsFalse(filtered);
            Assert.IsTrue(passed);
        }
    }
}
