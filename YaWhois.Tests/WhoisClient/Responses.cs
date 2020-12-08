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


        [TestCase("ya.ru", "whois.iana.org", Description = "IanaParser")]
        [TestCase("67.227.191.5", null, Description = "DefaultParser")]
        [TestCase("mail.bz", null, Description = "AfiliasParser")]
        [TestCase("mail.com", null, Description = "CrsNicParser")]
        public void Referrals(string query, string server)
        {
            int referrals = 0;

            // this will be called once when there are no referrals.
            _whois.WhenResponseParsed += (o, e) =>
            {
                if (e.Referral != null && e.Referral.Length > 0)
                    referrals++;
            };

            _whois.Query(query, server);

            Assert.IsTrue(referrals > 0);
        }


        [TestCase("baddomain", Description = "QueryAsync() must not throw any exceptions.")]
        public void QueryAsync_InvalidObject(string query)
        {
            var isError = false;

            _whois.WhenExceptionThrown += (o, e) =>
            {
                if (e.Exception is YaWhois.NoServerException ||
                    e.Exception is YaWhois.UnknownNetworkException)
                {
                    isError = true;
                }
            };

            _whois.QueryAsync(query).Wait();
            Assert.IsTrue(isError);
        }


        [TestCase("ya.ru", "whois.iana.org")]
        [TestCase("mail.bz", null, Description = "AFILIAS")]
        [TestCase("67.227.191.5", null, Description = "ARIN / DefaultParser")]
        public void AddRemoveParsers01(string query, string server)
        {
            int referrals = 0;

            // the standard case
            _whois.WhenResponseParsed += (o, e) =>
            {
                if (e.Referral != null && e.Referral.Length > 0)
                    referrals++;
            };
            _whois.Query(query, server);

            // register dumb parsers
            _whois.RegisterParserByServer("whois.iana.org", null);
            _whois.RegisterParserByServer("whois.afilias-grs.info", null);
            _whois.RegisterParserByServer("whois.arin.net", null);

            // now decrement the referrals value
            _whois.WhenResponseParsed += (o, e) =>
            {
                if (e.Referral == null)
                    referrals--;
            };
            _whois.Query(query, server);

            Assert.AreEqual(0, referrals);
        }


        // The alternative way for IANA. This will force to use DefaultParser.
        [TestCase("ya.ru", "whois.iana.org")]
        public void AddRemoveParsers02(string query, string server)
        {
            int referrals = 0;

            // the standard case
            _whois.WhenResponseParsed += (o, e) =>
            {
                if (e.Referral != null && e.Referral.Length > 0)
                    referrals++;
            };
            _whois.Query(query, server);

            // Register dumb parsers.
            _whois.UnregisterParserByServer("whois.iana.org");

            // now decrement the referrals value
            _whois.WhenResponseParsed += (o, e) =>
            {
                if (e.Referral == null)
                    referrals--;
            };
            _whois.Query(query, server);

            Assert.AreEqual(0, referrals);
        }
    }
}
