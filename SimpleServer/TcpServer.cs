using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SimpleServerUtil
{
    public class TcpServer
    { 
        private TcpListener _server;
        public TcpServer(IPAddress localAddress, int listenerPort)
        {
            _server = new TcpListener(localAddress, listenerPort);           
        }

        public delegate void ConnectionHandler(NetworkStream connection);
        internal event ConnectionHandler OnNewConnection;

        public void RegisterConnectionHandler(ConnectionHandler handler)
        {
            OnNewConnection += handler;
        }

        public void Poll()
        {
            _server.Start();
            while (true)
            {
                TcpClient newClient = _server.AcceptTcpClient();
                NetworkStream connection = newClient.GetStream();
                OnNewConnection(connection);
            }
        }
    }
}
