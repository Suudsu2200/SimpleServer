using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using SimpleServerLib.SimpleStream;
using SimpleServerLib.SimpleStream.SimpleSerializers;

namespace SimpleServerLib
{
    public class SimpleServer<T>
    {
        private readonly TcpListener _listener;
        private List<SimpleStreamIO> _activeConnections;
        private readonly X509Certificate _serverCertificate;
        private readonly ISimpleSerializer _serializer;

        public SimpleServer(IPAddress localAddress, int listenerPort, ISimpleSerializer serializer, X509Certificate serverCertificate = null)
        {
            _listener = new TcpListener(localAddress, listenerPort);
            _activeConnections = new List<SimpleStreamIO>();
            _serverCertificate = serverCertificate;
            _serializer = serializer;
        }

        public void Listen()
        {
            _listener.Start();
            while (true)
            {
                TcpClient newClient = _listener.AcceptTcpClient();
                Stream newStream;
                if (_serverCertificate != null)
                {
                    SslStream sslStream = new SslStream(newClient.GetStream());
                    sslStream.AuthenticateAsServer(_serverCertificate);
                    newStream = sslStream;
                }
                else
                {
                    newStream = newClient.GetStream();
                }

                SimpleStreamIO streamIO = new SimpleStreamIO(newStream, _serializer);
                _activeConnections.Add(streamIO);
                AcceptRequests(streamIO);
            }
        }

        public async void AcceptRequests(SimpleStreamIO streamIO)
        {
            while (true)
            {
                T obj = await streamIO.Read<T>();
                if (obj != null)
                {
                    BroadcastRequest?.Invoke(obj);
                }
            }
        }

        public delegate void RequestRecipient(T request);
        public event RequestRecipient BroadcastRequest;
    }
}
