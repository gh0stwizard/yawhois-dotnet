using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace YaWhois.Tests.WhoisClient
{
    public class Responses : BaseClass
    {
        [TestCase("1.0.0.1")]
        public void Query(string query)
        {
            var resp = _whois.Query(query);
            Assert.IsTrue(resp.Length > 0);
        }


        [TestCase("ya.ru", "whois.iana.org")]
        public void Referrals(string query, string server)
        {
            int referrals = 0;

            // this will be called once when there are no referrals.
            _whois.ResponseParsed += (o, e) =>
            {
                if (e.Referral != null && e.Referral.Length > 0)
                    referrals++;
            };

            _whois.Query(query, server);

            Assert.IsTrue(referrals > 0);
        }
    }
}
