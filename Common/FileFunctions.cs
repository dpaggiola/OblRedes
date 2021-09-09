using System.IO;
using ICommon;

namespace Common
{
    public class FileFunctions : IFileFunctions
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public string GetFileName(string path)
        {
            if (!FileExists(path))
                throw new FileNotFoundException("The specified file does not exist, please verify the given path");

            return new FileInfo(path).Name;
        }

        public long GetFileSize(string path)
        {
            if (!FileExists(path))
                throw new FileNotFoundException("The specified file does not exist, please verify the given path");

            return new FileInfo(path).Length;
        }
    }
}