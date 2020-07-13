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


        protected override string Fetch(string server, string query, Encoding readEncoding)
        {
            if (query == "exception.com")
                throw new Exception("test exception");

            return GetResponse(server, query);
        }


        protected override Task<string> FetchAsync(string server, string query, Encoding readEncoding, CancellationToken ct)
        {
            return Task.Run(() =>
            {
                if (query == "exception.com")
                    throw new Exception("test exception");

                return string.Empty;
            });
        }
    }
}
