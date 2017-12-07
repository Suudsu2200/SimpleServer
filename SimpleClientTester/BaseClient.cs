using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SimpleServerUtil
{
    public class BaseClient
    {
        private NetworkStream _stream;
        private TcpClient _client;
        private BinaryFormatter _serializer;
        private int _serverPatienceMs;

        public BaseClient(int serverPatienceMs)
        {
            _client = new TcpClient();
            _client.Connect("127.0.0.1", 8081);
            _stream = _client.GetStream();
            _serializer = new BinaryFormatter();
            _serverPatienceMs = serverPatienceMs;
        }

        public T Request<T>(object request)
        {
            _serializer.Serialize(_stream, request);
            _stream.Flush();
            while (!_stream.DataAvailable)
                continue;
            T response = (T)_serializer.Deserialize(_stream);
            Console.WriteLine("Response: " + response);
            return response;
        }
    }
}
