using System.Text;
using ICommon;

namespace Common
{
    public class Data : ICodification<string>
    {
        public byte[] Encode(string data)
        {
            return Encoding.ASCII.GetBytes(data);
        }

        public string Decode(byte[] message)
        {
            var decoded = Encoding.ASCII.GetString(message);
            return decoded;
        }
    }
}