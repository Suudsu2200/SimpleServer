using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SimpleServerLib.SimpleStream.SimpleSerializers
{
    public class BinarySerializer : ISimpleSerializer
    {
        private BinaryFormatter _formatter;

        public BinarySerializer()
        {
            _formatter = new BinaryFormatter();
        }

        public byte[] Serialize(Object obj)
        {
            MemoryStream memStream = new MemoryStream();
            _formatter.Serialize(memStream, obj);
            return memStream.ToArray();
        }

        public T Deserialize<T>(byte[] serializedObj)
        {
            return (T)_formatter.Deserialize(new MemoryStream(serializedObj));
        } 
    }
}
