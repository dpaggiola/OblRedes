namespace ICommon
{
    public interface ICodification<T>
    {
        byte[] Encode(T data);
        T Decode(byte[] message);
    }
}