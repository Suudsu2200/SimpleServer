using System.Collections.Generic;
using System.Net.Security;
using System.Threading;

namespace SimpleServerLib
{
    public class WorkerManager
    {
        public List<SslStream> Connections { get; set; }
        private readonly Thread _thread;

        public WorkerManager(ParameterizedThreadStart threadStart)
        {
            _thread = new Thread(threadStart);
            Connections = new List<SslStream>();
        }

        public void Start()
        {
            _thread.Start(this);
        }
    }
}
