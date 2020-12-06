using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace YaWhois.Tests.WhoisClient
{
    public class TestServer
    {
        public class TestServerEventArgs
        {
            public string Request { get; internal set; }
            public string Response { get; set; }
            public int DelayResponse { get; set; }
            public bool SplitResponse { get; set; }
            public int SplitResponseChunks { get; set; }
            public int SplitResponseDelay { get; set; }
        }


        protected virtual void OnRequestReceived(Socket handler, TestServerEventArgs e)
        {
            WhenRequestReceived?.Invoke(this, e);

            if (e.DelayResponse > 0)
                Thread.Sleep(1000 * e.DelayResponse);

            if (e.SplitResponse)
            {
                SendChunks(handler, e.Response ?? string.Empty, e.SplitResponseChunks, e.SplitResponseDelay);
            }
            else
            {
                Send(handler, e.Response ?? string.Empty);
            }
        }


        public event EventHandler<TestServerEventArgs> WhenRequestReceived;


        #region adopted from: https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-server-socket-example


        private bool IsListening = false;


        // State object for reading client data asynchronously
        public class StateObject
        {
            // Client  socket.
            public Socket workSocket = null;
            // Size of receive buffer.
            public const int BufferSize = 1024;
            // Receive buffer.
            public byte[] buffer = new byte[BufferSize];
            // Received data string.
            public StringBuilder sb = new StringBuilder();
        }


        // Thread signal.
        public static ManualResetEvent allDone = new ManualResetEvent(false);


        public async Task StartListening(CancellationToken token, string localIP = "127.0.0.1", int localPort = 8043)
        {
            IPAddress ipAddress = IPAddress.Parse(localIP);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, localPort);

            // Create a TCP/IP socket.
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            listener.Bind(localEndPoint);
            listener.Listen(100);

            IsListening = true;

            while (!token.IsCancellationRequested)
            {
                // Set the event to nonsignaled state.
                allDone.Reset();

                // Start an asynchronous socket to listen for connections.
                listener.BeginAccept(
                    new AsyncCallback(AcceptCallback),
                    listener);

                // Wait until a connection is made before continuing.
                var allDone_t = Task.Run(() => allDone.WaitOne());
                var cancel_t = Task.Run(() => Task.Delay(-1, token));
                await Task.WhenAny(allDone_t, cancel_t);
            }

            IsListening = false;
            listener.Close();
            listener.Dispose();
        }


        void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();

            if (!IsListening) return;

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }


        void ReadCallback(IAsyncResult ar)
        {
            string content = string.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket.
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read
                // more data.
                content = state.sb.ToString();
                if (content.IndexOf("\r\n") > -1)
                {
                    var args = new TestServerEventArgs()
                    {
                        Request = content.TrimEnd(new char[] { '\r', '\n' })
                    };
                    OnRequestReceived(handler, args);
                }
                else
                {
                    // Not all data received. Get more.
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private static void Send(Socket handler, string data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            // Retrieve the socket from the state object.
            Socket handler = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.
            int bytesSent = handler.EndSend(ar);

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }


        private class ChunkResponseState
        {
            public Socket Handler;
            public string Data;
            public int Current;
            public int Total;
            public int SleepMilliseconds;
        }

        private static void SendChunks(Socket handler, string data, int chunks, int sleepMs)
        {
            var state = new ChunkResponseState
            {
                Handler = handler,
                Data = data,
                Current = 1,
                SleepMilliseconds = sleepMs
            };

            byte[] byteData = Encoding.ASCII.GetBytes(data);

            if (chunks <= 1)
            {
                handler.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendChunkCallback), state);
            }
            else
            {
                if (byteData.Length < chunks)
                {
                    handler.BeginSend(byteData, 0, byteData.Length, 0,
                        new AsyncCallback(SendChunkCallback), state);
                }
                else
                {
                    state.Total = chunks;
                    handler.BeginSend(byteData, 0, byteData.Length / chunks, 0,
                        new AsyncCallback(SendChunkCallback), state);
                }
            }
        }

        private static void SendChunkCallback(IAsyncResult ar)
        {
            ChunkResponseState state = (ChunkResponseState)ar.AsyncState;
            Socket handler = state.Handler;
            int bytesSent = handler.EndSend(ar);

            if (state.Current < state.Total)
            {
                byte[] byteData = Encoding.ASCII.GetBytes(state.Data);
                var offset = byteData.Length / state.Total;
                var length = offset * state.Current;
                if (state.Total - state.Current == 1)
                    length = byteData.Length - length;
                state.Current++;

                if (state.SleepMilliseconds > 0)
                    Thread.Sleep(state.SleepMilliseconds);

                handler.BeginSend(byteData, offset, length, 0,
                    new AsyncCallback(SendChunkCallback), state);
            }
            else
            {
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }
    }

    #endregion
}
