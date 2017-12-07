using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using SimpleServerUtil;

namespace SimpleServerTester
{
    public class GameServer
    {
        public static void GameServerMain(object workerConnectionManager)
        {
            WorkerManager connectionManager =
                (WorkerManager) workerConnectionManager;
            BinaryFormatter formatter = new BinaryFormatter();
            Dictionary<Type, Tuple<IRequestHandler, MethodInfo>> handlerRegistry = GenerateHandlerRegistry();

            while (true)
            {
                foreach (NetworkStream connectionStream in connectionManager.Connections)
                { 
                    if (!connectionStream.DataAvailable)
                        continue;

                    object request = formatter.Deserialize(connectionStream);
                    Type t = request.GetType();

                    Tuple<IRequestHandler, MethodInfo> registeredHandler = handlerRegistry[t];
                    if (registeredHandler != null)
                    {
                        IRequestHandler handler = registeredHandler.Item1;
                        MethodInfo methodInfo = registeredHandler.Item2;
                        formatter.Serialize(connectionStream, methodInfo.Invoke(handler, new object[] { request }));
                        connectionStream.Flush();
                    }
                }
            }
        }

        private static Dictionary<Type, Tuple<IRequestHandler, MethodInfo>> GenerateHandlerRegistry()
        {
            Dictionary<Type, Tuple<IRequestHandler, MethodInfo>> handlerRegistry = new Dictionary<Type, Tuple<IRequestHandler, MethodInfo>>();
            foreach (Assembly assembly in new Assembly[] { Assembly.GetEntryAssembly() })
            {
                foreach (
                    Type type in
                    assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IRequestHandler))))
                {
                    IRequestHandler handler = (IRequestHandler)Activator.CreateInstance(type);
                    foreach (MethodInfo methodInfo in type.GetMethods().Where(method => method.Name == "Handle"))
                    {
                        ParameterInfo[] parameters = methodInfo.GetParameters();
                        if (parameters.Length != 1)
                            continue;
                        handlerRegistry.Add(parameters[0].ParameterType, new Tuple<IRequestHandler, MethodInfo>(handler, methodInfo));
                    }
                }
            }
            return handlerRegistry;
        }
    }
}
