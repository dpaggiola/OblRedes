using System.Threading.Tasks;

namespace ICommon
{
    public interface IFrameHandler
    {
        int WordLength { get; }
        Task<byte[]> ReadDataAsync();
        Task SendMessageAsync(byte[] data);
    }
}