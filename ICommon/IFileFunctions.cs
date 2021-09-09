namespace ICommon
{
    public interface IFileFunctions
    {
        bool FileExists(string path);
        string GetFileName(string path);
        long GetFileSize(string path);
    }
}