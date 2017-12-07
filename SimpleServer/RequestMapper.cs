using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServerUtil
{
    internal static class RequestMapper
    {
        private static Dictionary<Type, HandlerRegistryEntry> _handlerRegistry;
        private static BinaryFormatter _formatter;

        private class HandlerRegistryEntry
        {
            public Type actorType;
            public IRequestHandler actor;
            public MethodInfo method;

            public void Invoke(NetworkStream stream, object requestObj)
            {
                _formatter.Serialize(stream, method.Invoke(actor, new object[] { requestObj }));
                stream.Flush();
            }
        }


        static RequestMapper()
        {
            _handlerRegistry = new Dictionary<Type, HandlerRegistryEntry>();
            _formatter = new BinaryFormatter();
            foreach (Assembly assembly in new Assembly[] {Assembly.GetEntryAssembly()})
            {
                foreach (
                    Type type in
                    assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IRequestHandler))))
                {
                    foreach (MethodInfo methodInfo in type.GetMethods().Where(method => method.Name == "Handle"))
                    {
                        ParameterInfo[] parameters = methodInfo.GetParameters();
                        if (parameters.Length != 1)
                            continue;
                        IRequestHandler handler = (IRequestHandler) Activator.CreateInstance(type);
                        _handlerRegistry.Add(parameters[0].ParameterType,
                            new HandlerRegistryEntry
                            {
                                actorType = type,
                                actor = handler,
                                method = methodInfo
                            });
                    }
                }
            }
           
        }

        internal static void MapToIReturnHelper(object workerManager)
        {
            MapToIReturn((WorkerManager)workerManager);
        }

        internal static void MapToIReturn(WorkerManager workerManager)
        {
           
            while (true)
            {
                foreach (NetworkStream stream in workerManager.Connections)
                {
                    if (!stream.DataAvailable)
                        continue;

                    object request = _formatter.Deserialize(stream);
                    Type t = request.GetType();

                    if (_handlerRegistry.ContainsKey(t))
                    {
                        _handlerRegistry[t].Invoke(stream, request);        
                    }
                }
            }
        }

    }
}
