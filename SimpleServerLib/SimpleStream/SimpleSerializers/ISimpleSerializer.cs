namespace SimpleServerLib.SimpleStream.SimpleSerializers
{
    public interface ISimpleSerializer
    {
        byte[] Serialize(object obj);
        T Deserialize<T>(byte[] serializedObj);
    }
}
