using System;
using System.Text;
using ICommon;

namespace Common
{
    public class Command : ICodification<CommandType>
    {
        public static int ASSIGNED_BYTES = 2;

        public CommandType Decode(byte[] message)
        {
            var decoded = Encoding.ASCII.GetString(message);
            var parsed = (CommandType) Enum.Parse(typeof(CommandType), decoded);
            return parsed;
        }

        public byte[] Encode(CommandType type)
        {
            var command = Encoding.ASCII.GetBytes(type.ToString());
            Array.Resize(ref command, ASSIGNED_BYTES);
            return command;
        }
    }
}