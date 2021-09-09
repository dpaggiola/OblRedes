using System;
using System.Text;
using ICommon;

namespace Common
{
    public class Flag : ICodification<FlagType>
    {
        public static int ASSIGNED_BYTES = 3;

        public byte[] Encode(FlagType type)
        {
            var encodedFlag = Encoding.ASCII.GetBytes(type.ToString());
            Array.Resize(ref encodedFlag, ASSIGNED_BYTES);
            return encodedFlag;
        }

        public FlagType Decode(byte[] message)
        {
            var decoded = Encoding.ASCII.GetString(message);
            var parsed = (FlagType) Enum.Parse(typeof(FlagType), decoded);
            return parsed;
        }
    }
}