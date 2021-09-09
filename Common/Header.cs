using System.Linq;
using ICommon;

namespace Common
{
    public class Header : ICodification<HeaderStructure>
    {
        public static int HEADER_LENGTH = 9;

        private readonly Command command;
        private readonly CommandType commandType;
        private readonly Flag flag;

        private readonly FlagType flagType;
        private readonly HeaderLength headerLength;
        private int length;

        public Header()
        {
            command = new Command();
            flag = new Flag();
            headerLength = new HeaderLength();
        }


        public Header(HeaderStructure headerStructure)
        {
            command = headerStructure.Command;
            flag = headerStructure.Flag;
            headerLength = headerStructure.HeaderLength;
            commandType = headerStructure.CommandType;
            flagType = headerStructure.FlagType;
        }

        public HeaderStructure Decode(byte[] message)
        {
            var read = 0;
            var flagByteInfo = ReadFrame(message, read, Flag.ASSIGNED_BYTES);
            read += Flag.ASSIGNED_BYTES;
            var commandByteInfo = ReadFrame(message, read, Command.ASSIGNED_BYTES);
            read += Command.ASSIGNED_BYTES;
            var lengthBytesInfo = ReadFrame(message, read, HeaderLength.ASSIGNED_BYTES);
            //read += HeaderLength.ASSIGNED_BYTES;

            var headerType = flag.Decode(flagByteInfo);
            var cmdType = command.Decode(commandByteInfo);
            var length = headerLength.Decode(lengthBytesInfo);

            return new HeaderStructure(headerType, cmdType, length);
        }

        public byte[] Encode(HeaderStructure header)
        {
            var encodedFlag = header.Flag.Encode(flagType);
            var encodedCommand = header.Command.Encode(commandType);
            var encodedLength = header.HeaderLength.Encode(length);

            var resultEncoded = encodedFlag
                .Concat(encodedCommand)
                .Concat(encodedLength)
                .ToArray();
            return resultEncoded;
        }

        private byte[] ReadFrame(byte[] message, int skip, int take)
        {
            return message
                .Skip(skip)
                .Take(take)
                .ToArray();
        }
    }
}