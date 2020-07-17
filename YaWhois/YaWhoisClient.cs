using System;
using System.Collections.Generic;
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


        /// <summary>
        /// Make WHOIS request synchronously.
        /// </summary>
        /// <param name="obj">A query object.</param>
        /// <param name="server">A server to connect.</param>
        /// <param name="value">An user object.</param>
        /// <returns>WHOIS server response.</returns>
        public string Query(string obj, string server = null, object value = null)
        {
            return Query(obj, server, true, value);
        }


        /// <summary>
        /// Make WHOIS request asynchronously.
        /// </summary>
        /// <param name="obj">A query object.</param>
        /// <param name="server">A server to connect.</param>
        /// <param name="token">A cancellation token.</param>
        /// <param name="value">An user object.</param>
        /// <returns>WHOIS server response or null.</returns>
        public Task<string> QueryAsync(
            string obj, string server = null, CancellationToken token = default, object value = null)
        {
            return QueryAsync(obj, server, true, token, value);
        }


        private string Query(string obj, string server, bool clearHints, object value)
        {
            var args = new YaWhoisClientEventArgs() { Value = value };

            lock (QueryParser)
            {
                PrepareQuery(obj, server, clearHints);
                args.Parser = GetDataParser();
                args.Server = QueryParser.Server;
                args.Query = QueryParser.ServerQuery;
                args.Encoding = QueryParser.ServerEncoding;
            }

            int conn_timeout, rdwr_timeout;
            lock (__locker)
            {
                OnRequestReady(args);
                conn_timeout = ConnectTimeout;
                rdwr_timeout = ReadWriteTimeout;
            }

            args.Response = Fetch(args.Server, args.Query, args.Encoding, conn_timeout, rdwr_timeout);

            OnResponseReceived(args);

            args.Referral = args.Parser.GetReferral(args.Response);

            OnResponseParsed(args);

            if (!string.IsNullOrEmpty(args.Referral))
                return Query(obj, args.Referral, false, value);

            return args.Response;
        }


        private async Task<string> QueryAsync(
            string obj, string server, bool clearHints, CancellationToken ct, object value = null)
        {
            var args = new YaWhoisClientEventArgs() { Value = value };

            try
            {
                lock (QueryParser)
                {
                    PrepareQuery(obj, server, clearHints);
                    args.Parser = GetDataParser();
                    args.Server = QueryParser.Server;
                    args.Query = QueryParser.ServerQuery;
                    args.Encoding = QueryParser.ServerEncoding;
                }

                ct.ThrowIfCancellationRequested();

                int conn_timeout, rdwr_timeout;
                lock (__locker)
                {
                    OnRequestReady(args);
                    conn_timeout = ConnectTimeout;
                    rdwr_timeout = ReadWriteTimeout;
                }

                args.Response = await FetchAsync(args.Server, args.Query, args.Encoding, ct, conn_timeout, rdwr_timeout);

                OnResponseReceived(args);

                args.Referral = args.Parser.GetReferral(args.Response);

                OnResponseParsed(args);

                if (!string.IsNullOrEmpty(args.Referral))
                    return await QueryAsync(obj, args.Referral, false, ct, value);

                return args.Response;
            }
            catch (Exception e)
            {
                args.Exception = e;
                OnExceptionThrown(args);
                return null;
            }
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


        #region Events

        protected virtual void OnRequestReady(YaWhoisClientEventArgs e)
        {
            BeforeSendRequest?.Invoke(this, e);
            WhenRequestReady?.Invoke(this, e);
        }


        protected virtual void OnResponseReceived(YaWhoisClientEventArgs e)
        {
            BeforeParseResponse?.Invoke(this, e);
            WhenResponseReceived?.Invoke(this, e);
        }


        protected virtual void OnResponseParsed(YaWhoisClientEventArgs e)
        {
            ResponseParsed?.Invoke(this, e);
            WhenResponseParsed?.Invoke(this, e);
        }


        protected virtual void OnExceptionThrown(YaWhoisClientEventArgs e)
        {
            ExceptionThrown?.Invoke(this, e);
            WhenExceptionThrown?.Invoke(this, e);
        }


        /// <summary>
        /// Called before connecting to a server.
        /// </summary>
        [Obsolete("Use WhenRequestReady instead.")]
        public event EventHandler<YaWhoisClientEventArgs> BeforeSendRequest;
        /// <summary>
        /// Called before connecting to a server.
        /// </summary>
        public event EventHandler<YaWhoisClientEventArgs> WhenRequestReady;

        /// <summary>
        /// Called when response from the server has been received,
        /// but before retrieving referral.
        /// </summary>
        [Obsolete("Use WhenResponseReceived instead.")]
        public event EventHandler<YaWhoisClientEventArgs> BeforeParseResponse;
        /// <summary>
        /// Called when response from the server has been received,
        /// but before retrieving referral.
        /// </summary>
        public event EventHandler<YaWhoisClientEventArgs> WhenResponseReceived;

        /// <summary>
        /// Called when the server response has been parsed and the referral
        /// filled in.
        /// </summary>
        [Obsolete("Use WhenResponseParsed instead.")]
        public event EventHandler<YaWhoisClientEventArgs> ResponseParsed;
        /// <summary>
        /// Called when the server response has been parsed and the referral
        /// filled in.
        /// </summary>
        public event EventHandler<YaWhoisClientEventArgs> WhenResponseParsed;

        /// <summary>
        /// Called only by QueryAsync() method when something goes wrong.
        /// </summary>
        [Obsolete("Use WhenExceptionThrown instead.")]
        public event EventHandler<YaWhoisClientEventArgs> ExceptionThrown;
        /// <summary>
        /// Called only by QueryAsync() method when something goes wrong.
        /// </summary>
        public event EventHandler<YaWhoisClientEventArgs> WhenExceptionThrown;

        #endregion
    }
}
