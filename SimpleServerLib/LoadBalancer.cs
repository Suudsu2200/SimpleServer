﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Threading;

namespace SimpleServerLib
{
    public class LoadBalancer
    {
        private const int ThreadMax = 1;
        private readonly List<WorkerManager> _activeThreads;
        private readonly ParameterizedThreadStart _threadStart;

        public LoadBalancer(ParameterizedThreadStart threadStart)
        {
            _activeThreads = new List<WorkerManager>();
            _threadStart = threadStart;
        }

        public void DelegateNewConnection(SslStream connection)
        {
            if (_activeThreads.Count < ThreadMax)
            {
                WorkerManager newConnectionManager = new WorkerManager(_threadStart);
                newConnectionManager.Connections.Add(connection);
                _activeThreads.Add(newConnectionManager);
                newConnectionManager.Start();
            }
            else
                _activeThreads.OrderBy(thread => thread.Connections.Count).First().Connections.Add(connection);
        }
    }
}
