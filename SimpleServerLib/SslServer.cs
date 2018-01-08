using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace SimpleServerLib
{
    /*public class SslServer<T> where T : StreamSerializer
    { 
        private TcpListener _server;
        private List<T> _activeConnectionSerializers;

        public SslServer(IPAddress localAddress, int listenerPort)
        {
            _server = new TcpListener(localAddress, listenerPort);
            _server.Start();
            PollConnections();
            Console.WriteLine("Listening for clients...");
        }

        public delegate void SS_RequestHandler(object request);
        internal event SS_RequestHandler OnNewRequest;
        public void RegisterRequestHandler(SS_RequestHandler handler)
        {
            OnNewRequest += handler;
        }

        public void PollConnections()
        {
            _server.BeginAcceptTcpClient(
                (IAsyncResult connectionResult) =>
                {
                    PollConnections();
                    Console.WriteLine("Connection Established");
                    TcpClient client = _server.EndAcceptTcpClient(connectionResult);
                    SslStream sslStream = new SslStream(client.GetStream());
                    
                    X509Certificate2Collection certCollection = new X509Certificate2Collection();
                    certCollection.Import("C:\\users\\Dan\\desktop\\fictioncert.pfx", "certpass1", X509KeyStorageFlags.PersistKeySet);
                    sslStream.AuthenticateAsServer(certCollection[0]);
                }, 
                _server);
        }

        public void PollRequests()
        {

        }
        
    }*/
}
