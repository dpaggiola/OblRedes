namespace IServices
{
    public interface IFileService
    {
        void SendFile(string path);
        int GetLength();
        void ReceiveFile();
    }
}