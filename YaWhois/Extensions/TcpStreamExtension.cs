using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace YaWhois.Extensions
{
    public static class TcpStreamExtension
    {
        public static async Task<int> ReadAsyncWithTimeout(this NetworkStream stream, byte[] buffer, int offset, int count)
        {
            var read_t = stream.ReadAsync(buffer, offset, count);
            var wait_t = Task.Delay(stream.ReadTimeout);
            await Task.WhenAny(read_t, wait_t);

            if (read_t.IsCompleted)
                return await read_t;

            throw new TimeoutException("read timeout");
        }


        public static async Task WriteAsyncWithTimeout(this NetworkStream stream, byte[] buffer, int offset, int count)
        {
            var write_t = stream.WriteAsync(buffer, offset, count);
            var wait_t = Task.Delay(stream.WriteTimeout);
            await Task.WhenAny(write_t, wait_t);

            if (write_t.IsCompleted)
                return;

            throw new TimeoutException("write timeout");
        }
    }
}
