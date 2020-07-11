using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace YaWhois.Clients
{
    public abstract class WhoisClient
    {
        public int ConnectTimeout = 15;
        public int ReadTimeout = 15;
        public int WriteTimeout = 15;
        public int ReadBufferSize = 8192;
        internal Encoding ReadEncoding = Encoding.ASCII;
        internal string Host;
        internal int Port;


        internal string Fetch(string server, string query)
        {
            if (string.IsNullOrEmpty(server))
                throw new ArgumentNullException("server");

            if (string.IsNullOrEmpty(query))
                throw new ArgumentNullException("query");

            var srvPair = server.Split(new char[] { ':' }, 1);
            Host = srvPair[0];
            Port = srvPair.Length == 2 ? Convert.ToInt32(srvPair[1]) : 43;
            var sock = new TcpClient();
            var task = sock.ConnectAsync(Host, Port);
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
                        sb.Append(ReadEncoding.GetString(buff, 0, read));
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
    }
}
