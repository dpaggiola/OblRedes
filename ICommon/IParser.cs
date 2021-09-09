using System.Collections.Generic;

namespace ICommon
{
    public interface IParser
    {
        string[] GetDataObject(byte[] stream);
        List<string> GetList(string v);
    }
}