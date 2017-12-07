using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SimpleServerUtil;

namespace SimpleClientTester
{
    class Program
    {
        static void Main(string[] args)
        {
            BaseClient client = new BaseClient(5);
            while (true)
            { 
                Console.WriteLine("Request: " + client.Request<int>(75));
                Thread.Sleep(1000);
            }
        }
    }
}
