using System;
using System.Collections.Generic;
using System.Text;
using YaWhois.Clients;

namespace YaWhois
{
    public class YaWhoisClient : WhoisClient
    {
        QueryParser QueryParser { get; set; }
        YaWhoisClientEventArgs EventArgs { get; set; }


        public string Server { get { return QueryParser.Server; } }
        public string ServerQuery { get { return QueryParser.ServerQuery; } }


        // TODO: add function to register custom parsers.
        Dictionary<string, Type> DataParsersByServer = new Dictionary<string, Type>()
        {
            { "whois.iana.org", typeof(IanaParser) },
        };


        public YaWhoisClient()
        {
            QueryParser = new QueryParser();
            EventArgs = new YaWhoisClientEventArgs();
        }


        public string Query(string obj, string server = null)
        {
            return Query(obj, server, true);
        }


        private string Query(string obj, string server, bool clearHints)
        {
            PrepareQuery(obj, server, clearHints);

            OnBeforeSendRequest(EventArgs);

            EventArgs.Response = Fetch(QueryParser.Server, QueryParser.ServerQuery);
            EventArgs.Parser = GetDataParser();

            OnBeforeParseResponse(EventArgs);

            EventArgs.Referral = EventArgs.Parser.GetReferral(EventArgs.Response);

            OnResponseParsed(EventArgs);

            if (!string.IsNullOrEmpty(EventArgs.Referral))
                return Query(obj, EventArgs.Referral, false);

            return EventArgs.Response;
        }


        private void PrepareQuery(string obj, string server, bool clearHints)
        {
            // FIXME: do we need really call this again in recursion?
            QueryParser.GuessServer(obj);

            if (!string.IsNullOrEmpty(server))
            {
                QueryParser.Server = server;
                QueryParser.Query = obj;

                // Clear server hints for GetDataParser().
                if (clearHints)
                {
                    QueryParser.ServerHint &= ~QueryParser.ServerHints.AFILIAS;
                    QueryParser.ServerHint &= ~QueryParser.ServerHints.CRSNIC;
                    QueryParser.ServerHint &= ~QueryParser.ServerHints.IANA;
                }
            }

            ReadEncoding = QueryParser.ServerEncoding;

            // Generate QueryParser.ServerQuery.
            // Especially this is needed when QueryParser.Query was changed above.
            QueryParser.FormatQuery();
        }


        private IDataParser GetDataParser()
        {
            IDataParser parser;

            switch (QueryParser.ServerHint)
            {
                case QueryParser.ServerHints.IANA:
                    parser = new IanaParser();
                    break;

                case QueryParser.ServerHints.AFILIAS:
                    parser = new AfiliasParser();
                    break;

                case QueryParser.ServerHints.CRSNIC:
                    parser = new CrsNicParser();
                    break;

                default:
                    if (DataParsersByServer.TryGetValue(QueryParser.Server, out Type datatype))
                        parser = (IDataParser)Activator.CreateInstance(datatype);
                    else
                        parser = new DefaultParser();
                    break;
            }

            return parser;
        }


        protected virtual void OnBeforeSendRequest(YaWhoisClientEventArgs e)
        {
            BeforeSendRequest?.Invoke(this, e);
        }


        protected virtual void OnBeforeParseResponse(YaWhoisClientEventArgs e)
        {
            BeforeParseResponse?.Invoke(this, e);
        }


        protected virtual void OnResponseParsed(YaWhoisClientEventArgs e)
        {
            ResponseParsed?.Invoke(this, e);
        }


        public event EventHandler<YaWhoisClientEventArgs> BeforeSendRequest;
        public event EventHandler<YaWhoisClientEventArgs> BeforeParseResponse;
        public event EventHandler<YaWhoisClientEventArgs> ResponseParsed;
    }


    public class YaWhoisClientEventArgs : EventArgs
    {
        public IDataParser Parser { get; set; }
        public string Response { get; internal set; }
        public string Referral { get; internal set; }
    }
}
