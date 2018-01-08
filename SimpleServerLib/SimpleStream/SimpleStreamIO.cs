using System;
using System.IO;
using System.Threading.Tasks;
using SimpleServerLib.SimpleStream.SimpleSerializers;

namespace SimpleServerLib.SimpleStream
{
    public class SimpleStreamIO
    {
        private readonly Stream _stream;
        private readonly ISimpleSerializer _serializer;
        private readonly int _readPatienceMs;
        private const int ReadSize = 1024;

        public SimpleStreamIO(Stream stream, ISimpleSerializer serializer, int readPatienceMs = 25)
        {
            _stream = stream;
            _serializer = serializer;
            _readPatienceMs = readPatienceMs;
        }

        public async Task<T> Read<T>()
        {
            int bodyByteCount = await ReadHeader();
            T deserialzedObj = await ReadBody<T>(bodyByteCount);
            return deserialzedObj;
        }

        public void Write(object obj)
        {
            byte[] serializedObj = _serializer.Serialize(obj);
            _stream.Write(intToByteArray(serializedObj.Length), 0, sizeof(Int32));
            _stream.Write(serializedObj, 0, serializedObj.Length);
        }

        private async Task<int> ReadHeader()
        {
            int headerBytesRead = 0;
            byte[] headerBytes = new byte[sizeof(Int32)];

            while (true)
            {
                headerBytesRead += _stream.Read(headerBytes, headerBytesRead, sizeof(Int32) - headerBytesRead);
                if (headerBytesRead == sizeof(Int32))
                    break;
                await Task.Delay(_readPatienceMs);
            }
            return byteArrayToInt(headerBytes);
        }

        private async Task<T> ReadBody<T>(int bodyByteCount)
        {
            byte[] bodyBuffer = new byte[ReadSize];
            MemoryStream bodyMemoryStream = new MemoryStream();
            while (true)
            {
                int bytesRead = _stream.Read(bodyBuffer, 0, Math.Min(ReadSize, (int)(bodyByteCount - bodyMemoryStream.Length)) );
                bodyMemoryStream.Write(bodyBuffer, 0, bytesRead);
                if (bodyMemoryStream.Length == bodyByteCount)
                    break;
                await Task.Delay(_readPatienceMs);
            }
            return _serializer.Deserialize<T>(bodyMemoryStream.ToArray());
        }

        private int byteArrayToInt(byte[] toConvert)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(toConvert);
            return BitConverter.ToInt32(toConvert, 0);
        }

        private byte[] intToByteArray(int toConvert)
        {
            byte[] bytes = BitConverter.GetBytes(toConvert);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return bytes;
        }

    }
}
