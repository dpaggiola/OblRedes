using System;
using System.Text;
using ICommon;

namespace Common
{
    public class HeaderLength : ICodification<int>
    {
        public static int ASSIGNED_BYTES = 4;

        public byte[] Encode(int length)
        {
            var encodedDataLength = Encoding.ASCII.GetBytes(length.ToString());
            Array.Resize(ref encodedDataLength, ASSIGNED_BYTES);
            return encodedDataLength;
        }

        public int Decode(byte[] message)
        {
            var decoded = Encoding.ASCII.GetString(message);
            return int.Parse(decoded);
        }
    }
}