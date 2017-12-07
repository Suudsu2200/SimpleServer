using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace SimpleServerUtil
{
    public class WorkerManager
    {
        public List<NetworkStream> Connections { get; set; }
        private readonly Thread _thread;

        public WorkerManager(ParameterizedThreadStart threadStart)
        {
            _thread = new Thread(threadStart);
            Connections = new List<NetworkStream>();
        }

        public void Start()
        {
            _thread.Start(this);
        }
    }
}
