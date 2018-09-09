using BIStudio.Framework.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.MQ
{
    /// <summary>
    /// 订阅本地消息
    /// </summary>
    internal class LocalMessageDispatcher : MessageDispatcher, IMessageDispatcher
    {
        public LocalMessageDispatcher()
        {
            CFConfig.Default.ScanTypes(handlerType =>
            {
                if (handlerType.IsInterface)
                    return;
                foreach (Type interfaceType in handlerType.ImplementedInterfaces)
                {
                    if (interfaceType.IsGenericType && genericHandlerType == interfaceType.GetGenericTypeDefinition())
                    {
                        var handler = Activator.CreateInstance(handlerType);
                        var messageType = interfaceType.GetGenericArguments()[0];
                        var message = (IMessage)Activator.CreateInstance(messageType);
                        var method = handlerType.GetMethod("Handle", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.DeclaredOnly, Type.DefaultBinder, new Type[] { messageType }, null);

                        this.Subscribe(message.TypeName, new MessageListener
                        {
                            Target = messageType,
                            Handler = arg => method.Invoke(handler, new object[] { arg }),
                        });
                    }
                }
            });
        }
    }
}
