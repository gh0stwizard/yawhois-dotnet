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
        private readonly static Dictionary<string, Type> ResourceMap = new Dictionary<string, Type>()
        {
            {  "whois.iana.org", typeof(Resources.IanaOrg) },
        };


        private static string GetResponse(string server, string query)
        {
            ResourceManager rm;

            if (ResourceMap.TryGetValue(server, out Type rsxType))
                rm = new ResourceManager(rsxType);
            else
                rm = Resources.General.ResourceManager;

            return rm.GetString(query) ?? string.Empty;
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
