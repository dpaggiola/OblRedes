using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using ICommon;

namespace Common
{
    public class FrameHandler : IFrameHandler
    {
        private readonly NetworkStream networkStream;

        public FrameHandler(NetworkStream aNetworkStream)
        {
            WordLength = 4;
            networkStream = aNetworkStream;
        }

        public int WordLength { get; }

        public async Task SendMessageAsync(byte[] message)
        {
            var dataLength = BitConverter.GetBytes(message.Length);
            await networkStream.WriteAsync(dataLength, 0, WordLength);
            await networkStream.WriteAsync(message, 0, message.Length);
        }

        public async Task<byte[]> ReadDataAsync()
        {
            var dataLength = new byte[WordLength];
            var totalReceived = 0;
            while (totalReceived < WordLength)
            {
                var received = await networkStream.ReadAsync(dataLength, totalReceived, WordLength - totalReceived)
                    .ConfigureAwait(false);
                if (received == 0) throw new SocketException();
                totalReceived += received;
            }

            var actualLength = BitConverter.ToInt32(dataLength, 0);
            var data = new byte[actualLength];
            totalReceived = 0;
            while (totalReceived < actualLength)
            {
                var received = await networkStream.ReadAsync(data, totalReceived, actualLength - totalReceived)
                    .ConfigureAwait(false);
                if (received == 0) throw new SocketException();
                totalReceived += received;
            }

            return data;
        }
    }
}