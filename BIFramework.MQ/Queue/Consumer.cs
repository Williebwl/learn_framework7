using BIStudio.Framework.Utils;
using NetMQ;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.MQ
{
    /// <summary>
    /// 定义消息消费
    /// </summary>
    public class Consumer
    {
        //private static ConcurrentDictionary<MessageChannel, Consumer> connectedInstances = new ConcurrentDictionary<MessageChannel, Consumer>();
        private ConcurrentQueue<Action<RawMessage>> subscribers = new ConcurrentQueue<Action<RawMessage>>();
        private readonly MessageChannel brokerAddressWithConsumerPort;
        /// <summary>
        /// 定义消息消费
        /// </summary>
        /// <param name="brokerAddressForConsumer">需要接入的代理者地址</param>
        public Consumer(MessageChannel brokerAddressForConsumer)
        {
            this.brokerAddressWithConsumerPort = brokerAddressForConsumer;
        }
        /// <summary>
        /// 启动消息消费
        /// </summary>
        private async void Connect()
        {
            using (var context = NetMQContext.Create())
            using (var consumer = context.CreateSubscriberSocket())
            {
                //consumer.Options.ReceiveHighWatermark = 1000;
                consumer.Connect(brokerAddressWithConsumerPort.ToString());
                consumer.Subscribe("");
                //connectedInstances.AddOrUpdate(brokerAddressWithConsumerPort, this, (key, value) => this);
                Console.WriteLine("[Consumer]Connected to broker at " + brokerAddressWithConsumerPort + ".");

                await Task.Run(() =>
                {
                    while (true)
                    {
                        var message = consumer.Receive();
                        foreach (var subscriber in subscribers)
                            Task.Run(() => subscriber(message));
                    }
                });
            }
        }
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="handler"></param>
        public void Receive(Action<RawMessage> handler)
        {
            this.subscribers.Enqueue(handler);
        }

        /// <summary>
        /// 连接到消息代理
        /// </summary>
        /// <param name="brokerAddressForConsumer"></param>
        /// <returns></returns>
        public static Consumer Connect(MessageChannel brokerAddressForConsumer)
        {
            Consumer consumer = new Consumer(brokerAddressForConsumer);
            consumer.Connect();
            return consumer;
        }

        ///// <summary>
        ///// 订阅消息
        ///// </summary>
        ///// <param name="action">要执行的操作</param>
        ///// <param name="brokerIPs">来源端口，订阅到所有可用来源请设置为null</param>
        //public static void Receive(Action<RawMessage> action, string[] brokerIPs = null)
        //{
        //    var brokers = (brokerIPs == null ? null : brokerIPs.Select(ip => MessageChannel.ForConsumer(ip)));
        //    foreach (var kv in connectedInstances.Where(kv => brokerIPs == null || brokers.Contains(kv.Key)))
        //        kv.Value.subscribers.Enqueue(action);
        //}
    }
}
