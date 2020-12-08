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
        Dictionary<string, IDataParser> DataParserByServer { get; set; }

        public YaWhoisClient()
        {
            QueryParser = new QueryParser();
            DataParserByServer = new Dictionary<string, IDataParser>()
            {
                // Probably this is not the best way for this case.
                // As an alternative, set the ServerHint to IANA (in FormatQuery?).
                { "whois.iana.org", new IanaParser() }
            };
        }

        #region public methods

        /// <summary>
        /// Make WHOIS request synchronously.
        /// </summary>
        /// <param name="obj">A query object.</param>
        /// <param name="server">A server to connect.</param>
        /// <param name="value">An user object.</param>
        /// <returns>WHOIS server response or null.
        /// If there was a recursion then returns last response.
        /// </returns>
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
        /// <returns>WHOIS server response or null.
        /// If there was a recursion then returns last response.
        /// </returns>
        public Task<string> QueryAsync(
            string obj, string server = null, CancellationToken token = default, object value = null)
        {
            return QueryAsync(obj, server, true, token, value);
        }


        /// <summary>
        /// Registers a new <see cref="IDataParser"/> parser for the specified server.
        /// </summary>
        /// <param name="server">A whois server.</param>
        /// <param name="dataParser">A <see cref="IDataParser"/> object.
        /// The value could be null, in that case the step to find a referral would be skipped.
        /// </param>
        /// <param name="replace">When true replaces existing parser for the server.
        /// Otherwise the <see cref="ArgumentException"/> would be thrown if the server exists.
        /// </param>
        public void RegisterParserByServer(string server, IDataParser dataParser, bool replace = true)
        {
            if (!string.IsNullOrWhiteSpace(server))
            {
                if (replace && DataParserByServer.ContainsKey(server))
                    DataParserByServer.Remove(server);

                DataParserByServer.Add(server, dataParser);
            }
        }


        /// <summary>
        /// Removes a parser to the specified server.
        /// </summary>
        /// <param name="server">A whois server.</param>
        /// <returns>True when a parser was found.</returns>
        public bool UnregisterParserByServer(string server)
        {
            if (!string.IsNullOrWhiteSpace(server))
                return DataParserByServer.Remove(server);

            return false;
        }

        #endregion


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

            if (args.Parser != null)
                args.Referral = args.Parser.GetReferral(args.Response);
            else
                args.Referral = null;

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

                if (args.Parser != null)
                    args.Referral = args.Parser.GetReferral(args.Response);
                else
                    args.Referral = null;

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
            // The QueryParser object is shared across all threads.
            // Skip unnecassary parsing again when possible.
            if (QueryParser.OriginalQuery == null || QueryParser.OriginalQuery != obj)
                QueryParser.GuessServer(obj);

            if (!string.IsNullOrEmpty(server))
            {
                QueryParser.Server = server;

                // Clear server hints for GetDataParser().
                if (clearHints)
                {
                    QueryParser.ServerHint &= ~QueryParser.ServerHints.AFILIAS;
                    QueryParser.ServerHint &= ~QueryParser.ServerHints.CRSNIC;
                    QueryParser.ServerHint &= ~QueryParser.ServerHints.IANA;
                }
            }

            // Generate a new value of QueryParser.ServerQuery.
            // Unlikely that we repeat the same job until there are two or more Query()
            // requests with the same object.
            QueryParser.FormatQuery();
        }


        private static readonly Dictionary<QueryParser.ServerHints, IDataParser> DataParserByHint =
            new Dictionary<QueryParser.ServerHints, IDataParser>()
        {
            { QueryParser.ServerHints.AFILIAS, new AfiliasParser() },
            { QueryParser.ServerHints.CRSNIC, new CrsNicParser() },
            { QueryParser.ServerHints.IANA, new IanaParser() },
            { QueryParser.ServerHints.NONE, new DefaultParser() }
        };

        private IDataParser GetDataParser()
        {
            IDataParser parser;

            switch (QueryParser.ServerHint)
            {
                case QueryParser.ServerHints.IANA:
                    parser = DataParserByHint[QueryParser.ServerHints.IANA];
                    break;

                case QueryParser.ServerHints.AFILIAS:
                    parser = DataParserByHint[QueryParser.ServerHints.AFILIAS];
                    break;

                case QueryParser.ServerHints.CRSNIC:
                    parser = DataParserByHint[QueryParser.ServerHints.CRSNIC];
                    break;

                default:
                    if (DataParserByServer.TryGetValue(QueryParser.Server, out IDataParser dataParser))
                        parser = dataParser;
                    else
                        parser = DataParserByHint[QueryParser.ServerHints.NONE];
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
