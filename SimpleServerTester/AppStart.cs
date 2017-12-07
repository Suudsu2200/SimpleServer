using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SimpleServerUtil;
using SimpleServer = SimpleServerUtil.SimpleServer;

namespace SimpleServerTester
{
    public class AppStart
    {
        public static void Main()
        {
            SimpleServer server = new SimpleServer(IPAddress.Parse("127.0.0.1"), 8081);
            server.Start();
        }
    }
}
