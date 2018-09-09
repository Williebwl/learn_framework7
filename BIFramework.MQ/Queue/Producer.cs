using NetMQ;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BIStudio.Framework.Utils;

namespace BIStudio.Framework.MQ
{
    /// <summary>
    /// 定义消息生产
    /// </summary>
    public class Producer
    {
        //private static ConcurrentDictionary<MessageChannel, Producer> connectedInstances = new ConcurrentDictionary<MessageChannel, Producer>();
        //原有的自旋队列
        private ConcurrentQueue<RawMessage> messages = new ConcurrentQueue<RawMessage>();
        //new 阻塞队列可以省去同步代码
        private BlockingCollection<RawMessage> blockQueue = new BlockingCollection<RawMessage>();
        //如果把每个线程比作一辆汽车的话，AutoResetEvent和ManualResetEvent就是公路上的收费站。
        //设置收费站车闸默认关闭，需要开闸才能通行汽车；
        private EventWaitHandle waiter = new AutoResetEvent(false);
        private readonly MessageChannel brokerAddressForProducer;
        /// <summary>
        /// 定义消息生产
        /// </summary>
        /// <param name="brokerAddressForProducer">需要接入的代理者地址</param>
        public Producer(MessageChannel brokerAddressForProducer)
        {
            this.brokerAddressForProducer = brokerAddressForProducer;
        }
        /// <summary>
        /// 启动消息生产
        /// </summary>
        private async void Connect()
        {
            using (var context = NetMQContext.Create())
            using (var producer = context.CreatePublisherSocket())
            {
                //producer.Options.SendHighWatermark = 1000;
                producer.Connect(brokerAddressForProducer.ToString());
                //connectedInstances.AddOrUpdate(brokerAddressWithProducerPort, this, (key, value) => this);
                Console.WriteLine("[Producer]Connected to broker at " + brokerAddressForProducer + ".");

                await Task.Run(() =>
                {
                    //foreach (var message in blockQueue.GetConsumingEnumerable())
                    //{
                    //    producer.Send(message);
                    //}
                    while (true)
                    {
                        if (!messages.IsEmpty)
                        {
                            RawMessage message;
                            if (messages.TryDequeue(out message))
                                producer.Send(message);
                        }
                        else
                        {
                            waiter.WaitOne();
                        }
                    }
                });
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        public void Send(RawMessage message)
        {
            this.messages.Enqueue(message);
            //this.blockQueue.TryAdd(message);
            this.waiter.Set();
        }

        /// <summary>
        /// 连接到消息代理
        /// </summary>
        /// <param name="brokerAddressForProducer"></param>
        /// <returns></returns>
        public static Producer Connect(MessageChannel brokerAddressForProducer)
        {
            Producer producer = new Producer(brokerAddressForProducer);
            producer.Connect();
            return producer;
        }

        ///// <summary>
        ///// 发布消息
        ///// </summary>
        ///// <param name="message">要发送的消息</param>
        ///// <param name="brokerIPs">目标端口，发送到所有可用目标请设置为null</param>
        //public static void Send(RawMessage message, string[] brokerIPs = null)
        //{
        //    var brokers = (brokerIPs == null ? null : brokerIPs.Select(ip => MessageChannel.ForProducer(ip)));
        //    foreach (var kv in connectedInstances.Where(kv => brokers == null || brokers.Contains(kv.Key)))
        //    {
        //        kv.Value.messages.Enqueue(message);
        //        kv.Value.waiter.Set();
        //    }
        //}
    }
}
