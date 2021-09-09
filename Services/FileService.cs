using System;
using System.Text;
using Common;
using ICommon;
using IServices;

namespace Services
{
    public class FileService : IFileService
    {
        public IFileHandler fileHandler;
        public IFileFunctions fileHelper;

        public FileService(IFileFunctions iFileHelper, IFileHandler iFileHandler)
        {
            fileHelper = iFileHelper;
            fileHandler = iFileHandler;
        }

        public int GetLength()
        {
            return ConnectionConstants.FixedFileNameLength + ConnectionConstants.FixedFileSizeLength;
        }

        public void ReceiveFile()
        {
            var stream = fileHandler.ReadFrame(GetLength());
            var fileNameSize = BitConverter.ToInt32(stream, 0);
            var fileSize = BitConverter.ToInt64(stream, ConnectionConstants.FixedFileNameLength);

            var fileName = Encoding.UTF8.GetString(fileHandler.ReadFrame(fileNameSize));

            var parts = SpecificationHelper.GetParts(fileSize);
            long offset = 0;
            long currentPart = 1;

            while (fileSize > offset)
            {
                byte[] data;
                if (currentPart == parts)
                {
                    var lastPartSize = (int) (fileSize - offset);
                    data = fileHandler.ReadFrame(lastPartSize);
                    offset += lastPartSize;
                }
                else
                {
                    data = fileHandler.ReadFrame(ConnectionConstants.MaxPacketSize);
                    offset += ConnectionConstants.MaxPacketSize;
                    currentPart++;
                }

                fileHandler.Write(fileName, data);
            }
        }

        public void SendFile(string path)
        {
            var fileSize = fileHelper.GetFileSize(path);
            var fileName = fileHelper.GetFileName(path);
            if (fileSize > 104857600)
                throw new System.Exception("Archivo mas grande de lo definido.");
            var stream = Create(fileName, fileSize);

            fileHandler.WriteFrame(stream);

            fileHandler.WriteFrame(Encoding.UTF8.GetBytes(fileName));

            var parts = SpecificationHelper.GetParts(fileSize);
            long offset = 0;
            long currentPart = 1;

            while (fileSize > offset)
            {
                byte[] data;
                if (currentPart == parts)
                {
                    var lastPartSize = (int) (fileSize - offset);
                    data = fileHandler.ReadData(path, offset, lastPartSize);
                    offset += lastPartSize;
                }
                else
                {
                    data = fileHandler.ReadData(path, offset, ConnectionConstants.MaxPacketSize);
                    offset += ConnectionConstants.MaxPacketSize;
                }

                fileHandler.WriteFrame(data);

                currentPart++;
            }
        }

        public byte[] Create(string fileName, long fileSize)
        {
            var stream = new byte[GetLength()];
            var fileNameData = BitConverter.GetBytes(Encoding.UTF8.GetBytes(fileName).Length);
            if (fileNameData.Length != ConnectionConstants.FixedFileNameLength)
                throw new System.Exception("Hay un problema con el nombre del archivo");
            var fileSizeData = BitConverter.GetBytes(fileSize);

            Array.Copy(fileNameData, 0, stream, 0, ConnectionConstants.FixedFileNameLength);
            Array.Copy(fileSizeData, 0, stream, ConnectionConstants.FixedFileNameLength,
                ConnectionConstants.FixedFileSizeLength);

            return stream;
        }
    }
}