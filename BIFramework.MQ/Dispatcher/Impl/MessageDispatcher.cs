using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using BIStudio.Framework;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using BIStudio.Framework.Utils;

namespace BIStudio.Framework.MQ
{
    /// <summary>
    /// 订阅消息
    /// </summary>
    internal class MessageDispatcher : IMessageDispatcher
    {
        protected static readonly Type genericHandlerType = typeof(IMessageHandler<>);
        protected readonly ConcurrentDictionary<string, List<MessageListener>> globalListeners = new ConcurrentDictionary<string, List<MessageListener>>();

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="typeName">消息类型</param>
        /// <param name="listener">消息响应</param>
        protected void Subscribe(string typeName, MessageListener listener)
        {
            globalListeners.AddOrUpdate(typeName, new List<MessageListener> { listener }, (key, value) =>
            {
                value.Add(listener);
                return value;
            });
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="handler">消息响应</param>
        public virtual void Subscribe(Action<IMessage> handler)
        {
            var handlerType = handler.GetType();
            var messageType = handlerType.GetGenericArguments()[0];
            var message = (IMessage)Activator.CreateInstance(messageType);

            this.Subscribe(message.TypeName, new MessageListener
            {
                Target = messageType,
                Handler = handler,
            });
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="message">消息</param>
        public virtual void Dispatch(IMessage message)
        {
            List<MessageListener> messageListeners;
            globalListeners.TryGetValue(message.TypeName, out messageListeners);
            if (messageListeners == null)
                return;
            foreach (var messageListener in messageListeners)
            {
                Task.Factory.StartNew(() =>
                {
                    var msg = (IMessage)ALSerialize.JsonDeserialize(ALSerialize.JsonSerialize(message), messageListener.Target);
                    msg.ID = message.ID;
                    msg.Timestamp = message.Timestamp;
                    msg.TypeName = message.TypeName;
                    messageListener.Handler(msg);
                });
            }
        }

    }
}
