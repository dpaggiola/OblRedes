namespace ICommon
{
    public interface IFileHandler
    {
        byte[] ReadData(string path, long offset, int length);
        void Write(string fileName, byte[] data);
        void WriteFrame(byte[] data);
        byte[] ReadFrame(int length);
    }
}