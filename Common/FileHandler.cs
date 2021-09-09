using System;
using System.IO;
using System.Net.Sockets;
using ICommon;

namespace Common
{
    public class FileHandler : IFileHandler
    {
        private readonly NetworkStream networkStream;

        public FileHandler(NetworkStream aNetworkStream)
        {
            networkStream = aNetworkStream;
        }

        public byte[] ReadData(string path, long offset, int length)
        {
            var data = new byte[length];

            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                fileStream.Position = offset;
                var bytesRead = 0;
                while (bytesRead < length)
                {
                    var read = fileStream.Read(data, bytesRead, length - bytesRead);
                    if (read == 0) throw new Exception("No se pudo leer el archivo");
                    bytesRead += read;
                }
            }

            return data;
        }

        public void WriteFrame(byte[] data)
        {
            networkStream.Write(data, 0, data.Length);
        }

        public void Write(string fileName, byte[] data)
        {
            if (File.Exists(fileName))
                using (var fileStream = new FileStream(fileName, FileMode.Append))
                {
                    fileStream.Write(data, 0, data.Length);
                }
            else
                using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    fileStream.Write(data, 0, data.Length);
                }
        }

        public byte[] ReadFrame(int length)
        {
            var dataReceived = 0;
            var data = new byte[length];
            while (dataReceived < length)
            {
                var received = networkStream.Read(data, dataReceived, length - dataReceived);
                if (received == 0) throw new SocketException();
                dataReceived += received;
            }

            return data;
        }
    }
}