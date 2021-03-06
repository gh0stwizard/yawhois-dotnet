using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YaWhois.Tests.WhoisClient
{
    public class TestClient : YaWhoisClient
    {
        private ResourceManager _rm;
        private bool _use_custom_rm;

        private readonly static Dictionary<string, Type> ResourceMap = new Dictionary<string, Type>()
        {
            { "whois.iana.org", typeof(Resources.IanaOrg) },
            { "whois.arin.net", typeof(Resources.ARIN) },
            { "whois.afilias-grs.info", typeof(Resources.Afilias) },
            { "whois.verisign-grs.com", typeof(Resources.CrsNic) },
            { "whois.ripe.net", typeof(Resources.RIPE) }
        };


        // XXX: no critic
        public void SetResourceMap(Type resType)
        {
            _rm = new ResourceManager(resType);
            _use_custom_rm = true;
        }


        public void ResetResourceMap()
        {
            _use_custom_rm = false;
        }


        private string GetResponse(string server, string query)
        {
            if (!_use_custom_rm)
            {
                if (ResourceMap.TryGetValue(server, out Type rsxType))
                    _rm = new ResourceManager(rsxType);
                else
                    _rm = Resources.General.ResourceManager;
            }

            return _rm.GetString(query) ?? string.Empty;
        }


        protected override string Fetch(string server, string query, Encoding readEncoding,
            int connectTimeout = 15, int readWriteTimeout = 15)
        {
            if (query == "exception.com")
                throw new Exception("test exception");

            return GetResponse(server, query);
        }


        protected override Task<string> FetchAsync(
            string server, string query, Encoding readEncoding, CancellationToken ct,
            int connectTimeout = 15, int readWriteTimeout = 15)
        {
            return Task.Run(async () =>
            {
                if (query == "exception.com")
                    throw new Exception("test exception");

                if (query == "delay10s.com")
                    await Task.Delay(10 * 1000, ct);

                return GetResponse(server, query);
            }, ct);
        }
    }
}
