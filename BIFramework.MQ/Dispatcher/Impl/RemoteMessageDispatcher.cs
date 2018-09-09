using BIStudio.Framework.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.MQ
{
    /// <summary>
    /// 订阅远程消息
    /// </summary>
    internal class RemoteMessageDispatcher : LocalMessageDispatcher, IMessageDispatcher
    {
        private Consumer consumer;
        private Producer producer;
        public RemoteMessageDispatcher(Consumer consumer, Producer producer)
        {
            this.consumer = consumer;
            this.producer = producer;
            consumer.Receive(rawMessage =>
            {
                List<MessageListener> messageListeners;
                globalListeners.TryGetValue(rawMessage.Topic, out messageListeners);
                if (messageListeners == null)
                    return;
                foreach (var messageListener in messageListeners)
                {
                    Task.Factory.StartNew(() =>
                    {
                        messageListener.Handler(rawMessage.Generalize(messageListener.Target));
                    });
                }
            });
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="message">消息</param>
        public override void Dispatch(IMessage message)
        {
            producer.Send(RawMessage.Parse(message));
        }
    }
}
