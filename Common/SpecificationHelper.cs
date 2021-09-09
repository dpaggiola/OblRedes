namespace Common
{
    public static class SpecificationHelper
    {
        public static long GetParts(long fileSize)
        {
            var parts = fileSize / ConnectionConstants.MaxPacketSize;
            return parts * ConnectionConstants.MaxPacketSize == fileSize ? parts : parts + 1;
        }
    }
}