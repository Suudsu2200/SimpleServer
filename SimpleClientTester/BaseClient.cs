using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using SimpleServerLib.SimpleStream.SimpleSerializers;

namespace SimpleServerClient
{
    public class BaseClient
    {
        private SslStream _stream;
        private TcpClient _client;
        private BinarySerializer _serializer;

        public BaseClient(int serverPatienceMs)
        {
            _client = new TcpClient();
            _client.Connect("127.0.0.1", 443);
            _stream = new SslStream(_client.GetStream());
            _stream.AuthenticateAsClient("DESKTOP-8VSRA10");
            _serializer = new BinarySerializer();
        }

        public void Request<T>(object request)
        {
            _stream.Write(_serializer.Serialize(14));
            //SerializeToStream(_stream, 14);
            //SerializeToStream(_stream, 145);

            /*int serializeValue = 20;
            MemoryStream stream = SerializeToStream(serializeValue);
            int deserializedValue = (int) DeserializeFromStream(stream);


            /*Int32 preHeaderRequestLength = -1;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                byte[] memBytes = new byte[1000];
                BinaryWriter writer = new BinaryWriter(memoryStream);
                _serializer.Serialize(memoryStream, 17);
                memoryStream.Read(memBytes, 0, 1000);
                preHeaderRequestLength = (Int32)memoryStream.Length;
            }



            _serializer.Serialize(_stream, preHeaderRequestLength);
           // _serializer.Serialize(_stream, request);
            _stream.Flush();

            byte[] bytes = new byte[1000];
            while (true)
            {
                if (_stream.Read(bytes, 0, 1) > 0)
                    break;
                continue;
            }
            /*T response = (T)_serializer.Deserialize(_stream);
            Console.WriteLine("Response: " + response);
            return response;*/

        }

        /*public static void SerializeToStream(Stream stream, object obj)
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
        }

        public static T DeserializeFromStream<T>(Stream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            object obj = formatter.Deserialize(stream);
            return (T)obj;
        }*/
    }
}
