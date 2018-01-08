using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace SimpleServerLib
{
    internal static class RequestMapper
    {
        private static Dictionary<Type, HandlerRegistryEntry> _handlerRegistry;
        private static BinaryFormatter _formatter;

        private class HandlerRegistryEntry
        {
            public Type actorType;
            public IRequestHandler2 actor;
            public MethodInfo method;

            public void Invoke(SslStream stream, object requestObj)
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
                    assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IRequestHandler2))))
                {
                    foreach (MethodInfo methodInfo in type.GetMethods().Where(method => method.Name == "Handle"))
                    {
                        ParameterInfo[] parameters = methodInfo.GetParameters();
                        if (parameters.Length != 1)
                            continue;
                        IRequestHandler2 handler2 = (IRequestHandler2) Activator.CreateInstance(type);
                        _handlerRegistry.Add(parameters[0].ParameterType,
                            new HandlerRegistryEntry
                            {
                                actorType = type,
                                actor = handler2,
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
                foreach (SslStream stream in workerManager.Connections)
                {
                    int sever = (int)_formatter.Deserialize(stream);
                    int server2 = (int)_formatter.Deserialize(stream);

                    /* byte[] lengthBytes = new byte[1000];
                     int bytesRead = stream.Deserialize(lengthBytes, 0, 1000);
                     if (bytesRead < 2) continue;
                     int messageLength = 0;
                     using (MemoryStream memoryStream = new MemoryStream())
                     {
                         BinaryWriter writer = new BinaryWriter(memoryStream);
                         writer.Write(lengthBytes);
                         messageLength = (int)_formatter.Deserialize(stream);
                     }
 
                     byte[] streamBytes = new byte[messageLength];
                     if (stream.Deserialize(streamBytes, 0, messageLength) != messageLength)
                         Console.WriteLine("fuck");
 
                     object requestObj;
                     using (MemoryStream memoryStream = new MemoryStream())
                     {
                         BinaryWriter writer = new BinaryWriter(memoryStream);
                         writer.Write(streamBytes);
                         requestObj = _formatter.Deserialize(stream);
                     }
 
                     Type t = requestObj.GetType();
                     if (_handlerRegistry.ContainsKey(t))
                     {
                         _handlerRegistry[t].Invoke(stream, requestObj);        
                     }*/
                }
            }
        }

    }
}
