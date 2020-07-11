﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YaWhois.Clients;

namespace YaWhois
{
    public class YaWhoisClient : WhoisClient
    {
        QueryParser QueryParser { get; set; }


        // TODO: add function to register custom parsers.
        Dictionary<string, Type> DataParsersByServer = new Dictionary<string, Type>()
        {
            { "whois.iana.org", typeof(IanaParser) },
        };


        public YaWhoisClient()
        {
            QueryParser = new QueryParser();
        }


        public string Query(string obj, string server = null)
        {
            return Query(obj, server, true);
        }


        public Task<string> QueryAsync(string obj, string server = null, CancellationToken ct = default)
        {
            return QueryAsync(obj, server, true, ct);
        }


        private string Query(string obj, string server, bool clearHints)
        {
            var args = new YaWhoisClientEventArgs();

            lock (QueryParser)
            {
                PrepareQuery(obj, server, clearHints);
                args.Parser = GetDataParser();
                args.Server = QueryParser.Server;
                args.Query = QueryParser.ServerQuery;
                args.Encoding = QueryParser.ServerEncoding;
            }

            OnBeforeSendRequest(args);

            args.Response = Fetch(args.Server, args.Query, args.Encoding);

            OnBeforeParseResponse(args);

            args.Referral = args.Parser.GetReferral(args.Response);

            OnResponseParsed(args);

            if (!string.IsNullOrEmpty(args.Referral))
                return Query(obj, args.Referral, false);

            return args.Response;
        }


        private Task<string> QueryAsync(string obj, string server, bool clearHints, CancellationToken ct)
        {
            return Task.Run(async () =>
            {
                var args = new YaWhoisClientEventArgs();

                lock (QueryParser)
                {
                    PrepareQuery(obj, server, clearHints);
                    args.Parser = GetDataParser();
                    args.Server = QueryParser.Server;
                    args.Query = QueryParser.ServerQuery;
                    args.Encoding = QueryParser.ServerEncoding;
                }

                OnBeforeSendRequest(args);

                try
                {
                    args.Response = await FetchAsync(args.Server, args.Query, args.Encoding, ct);

                    OnBeforeParseResponse(args);

                    args.Referral = args.Parser.GetReferral(args.Response);

                    OnResponseParsed(args);

                    if (!string.IsNullOrEmpty(args.Referral))
                        return await QueryAsync(obj, args.Referral, false, ct);

                    return args.Response;
                }
                catch (Exception e)
                {
                    args.Exception = e;
                    OnExceptionThrown(args);
                    return null;
                }
            }, ct);
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

        protected virtual void OnExceptionThrown(YaWhoisClientEventArgs e)
        {
            ExceptionThrown?.Invoke(this, e);
        }

        public event EventHandler<YaWhoisClientEventArgs> BeforeSendRequest;
        public event EventHandler<YaWhoisClientEventArgs> BeforeParseResponse;
        public event EventHandler<YaWhoisClientEventArgs> ResponseParsed;
        public event EventHandler<YaWhoisClientEventArgs> ExceptionThrown;
    }


    public class YaWhoisClientEventArgs : EventArgs
    {
        public object Value { get; set; }
        public IDataParser Parser { get; set; }
        public string Server { get; internal set; }
        public string Query { get; internal set; }
        public Encoding Encoding { get; internal set; }
        public string Response { get; internal set; }
        public string Referral { get; internal set; }
        public Exception Exception { get; internal set; }
    }
}
