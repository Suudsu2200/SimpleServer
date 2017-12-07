using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServerUtil
{
    public class SimpleServer
    {
        private readonly TcpServer _server;
        private LoadBalancer _loadBalancer;

        public SimpleServer(IPAddress address, int port = 8083)
        {
            _server = new TcpServer(address, port);
            _loadBalancer = new LoadBalancer(RequestMapper.MapToIReturnHelper);
            _server.RegisterConnectionHandler(_loadBalancer.DelegateNewConnection);
        }

        public void Start()
        {
            _server.Poll();
        }
    }
}
