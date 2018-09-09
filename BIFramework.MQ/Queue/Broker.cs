using NetMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BIStudio.Framework.MQ
{
    /// <summary>
    /// 消息代理
    /// </summary>
    public class Broker
    {
        private readonly IEnumerable<MessageChannel> producerPort, consumerPort;
        /// <summary>
        /// 定义消息代理
        /// </summary>
        /// <param name="producerPort">允许接入的生产者地址，允许所有ip地址填*</param>
        /// <param name="consumerPort">允许接入的消费者地址，允许所有ip地址填*</param>
        public Broker(MessageChannel producerPort, MessageChannel consumerPort)
        {
            this.producerPort = new MessageChannel[] { producerPort };
            this.consumerPort = new MessageChannel[] { consumerPort };
        }
        /// <summary>
        /// 定义消息代理
        /// </summary>
        /// <param name="producerPort">允许接入的生产者地址，允许所有ip地址填*</param>
        /// <param name="consumerPort">允许接入的消费者地址，允许所有ip地址填*</param>
        public Broker(IEnumerable<MessageChannel> producerPort, IEnumerable<MessageChannel> consumerPort)
        {
            this.producerPort = producerPort;
            this.consumerPort = consumerPort;
        }
        /// <summary>
        /// 启动消息代理
        /// </summary>
        private async void Start()
        {
            using (var context = NetMQContext.Create())
            using (var producer = context.CreateXSubscriberSocket())
            using (var consumer = context.CreateXPublisherSocket())
            {
                foreach (var port in consumerPort)
                {
                    consumer.Bind(port.ToString());
                    Console.WriteLine("[Broker]Waiting for consumer at " + port + ".");
                }
                foreach (var port in producerPort)
                {
                    producer.Bind(port.ToString());
                    Console.WriteLine("[Broker]Waiting for producer at " + port + ".");
                }

                var proxy = new Proxy(consumer, producer);
                await Task.Run(() => proxy.Start());
            }
        }
        
        /// <summary>
        /// 创建消息代理
        /// </summary>
        /// <returns></returns>
        public static Broker Start(MessageChannel producerPort, MessageChannel consumerPort)
        {
            Broker broker = new Broker(producerPort, consumerPort);
            broker.Start();
            return broker;
        }
    }
}
