namespace Common
{
    public class HeaderStructure
    {
        public HeaderStructure()
        {
        }

        public HeaderStructure(FlagType flagType, CommandType commandType, int actualLength)
        {
            Command = new Command();
            Flag = new Flag();
            HeaderLength = new HeaderLength();

            FlagType = flagType;
            CommandType = commandType;
            ActualLength = actualLength;
        }

        public Command Command { get; set; }
        public Flag Flag { get; set; }
        public HeaderLength HeaderLength { get; set; }

        public FlagType FlagType { get; set; }
        public CommandType CommandType { get; set; }

        public int ActualLength { get; set; }
    }
}