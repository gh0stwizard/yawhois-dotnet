using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YaWhois.Extensions;

namespace YaWhois.Clients
{
    public abstract class WhoisClient
    {
        public int ConnectTimeout = 15;
        public int ReadTimeout = 15;
        public int WriteTimeout = 15;
        public int ReadBufferSize = 8192;


        internal string Fetch(string server, string query, Encoding readEncoding)
        {
            if (string.IsNullOrEmpty(server))
                throw new ArgumentNullException("server");

            if (string.IsNullOrEmpty(query))
                throw new ArgumentNullException("query");

            var srvPair = server.Split(new char[] { ':' }, 1);
            var host = srvPair[0];
            var port = srvPair.Length == 2 ? Convert.ToInt32(srvPair[1]) : 43;
            var sock = new TcpClient();
            var task = sock.ConnectAsync(host, port);
            task.ConfigureAwait(false);

            if (!task.Wait(TimeSpan.FromSeconds(ConnectTimeout)))
                return null;

            try
            {
                using (var s = sock.GetStream())
                {
                    s.WriteTimeout = 1000 * WriteTimeout;
                    s.ReadTimeout = 1000 * ReadTimeout;
                    var queryBytes = Encoding.ASCII.GetBytes(query + "\r\n");
                    s.Write(queryBytes, 0, queryBytes.Length);
                    s.Flush();

                    var sb = new StringBuilder();
                    var buff = new byte[ReadBufferSize];

                    while (true)
                    {
                        var read = s.Read(buff, 0, buff.Length);
                        if (read == 0) break;
                        sb.Append(readEncoding.GetString(buff, 0, read));
                    }

                    return sb.ToString();
                }
            }
            catch
            {
                sock.Close();
                return null;
            }
            finally
            {
                sock.Close();
            }
        }


        internal async Task<string> FetchAsync(
            string server, string query, Encoding readEncoding, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(server))
                throw new ArgumentNullException("server");

            if (string.IsNullOrEmpty(query))
                throw new ArgumentNullException("query");

            var srvPair = server.Split(new char[] { ':' }, 1);
            var host = srvPair[0];
            var port = srvPair.Length == 2 ? Convert.ToInt32(srvPair[1]) : 43;
            var sock = new TcpClient();
            var conn_t =  sock.ConnectAsync(host, port);
            var time_t = Task.Delay(1000 * ConnectTimeout, ct);

            await Task.WhenAny(conn_t, time_t);

            if (!conn_t.IsCompleted)
            {
                if (time_t.IsCanceled)
                    throw new TaskCanceledException();

                throw new TimeoutException("connection timeout");
            }

            // TODO: localized exceptions... force to english?
            if (conn_t.IsFaulted)
                throw conn_t.Exception;

            try
            {
                using (var s = sock.GetStream())
                {
                    s.WriteTimeout = 1000 * WriteTimeout;
                    s.ReadTimeout = 1000 * ReadTimeout;

                    var queryBytes = Encoding.ASCII.GetBytes(query + "\r\n");

                    await s.WriteAsyncWithTimeout(queryBytes, 0, queryBytes.Length);
                    await s.FlushAsync();

                    var sb = new StringBuilder();
                    var buff = new byte[ReadBufferSize];

                    while (!ct.IsCancellationRequested)
                    {
                        var read = await s.ReadAsyncWithTimeout(buff, 0, buff.Length);
                        if (read == 0) break;
                        sb.Append(readEncoding.GetString(buff, 0, read));
                    }

                    if (ct.IsCancellationRequested)
                        throw new TaskCanceledException();

                    return sb.ToString();
                }
            }
            catch (Exception e)
            {
                sock.Close();
                throw e;
            }
            finally
            {
                sock.Close();
            }
        }
    }
}
