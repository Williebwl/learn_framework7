using BIStudio.Framework;
using BIStudio.Framework.MQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Configuration
{
    /// <summary>
    /// 消息总线配置
    /// </summary>
    public static class MQConfig
    {
        private static Broker broker;
        /// <summary>
        /// 启动消息代理端
        /// </summary>
        /// <param name="config"></param>
        /// <param name="clientIP">允许接入的客户端地址</param>
        /// <returns></returns>
        public static CFConfig RegisterMessageBroker(this CFConfig config, string clientIP = "*")
        {
            broker = Broker.Start(MessageChannel.ForProducer(clientIP), MessageChannel.ForConsumer(clientIP));
            return config;
        }

        /// <summary>
        /// 启动消息客户端
        /// </summary>
        /// <param name="config"></param>
        /// <param name="brokerIP">远程消息代理</param>
        /// <returns></returns>
        public static CFConfig RegisterMessageDispatcher(this CFConfig config, string brokerIP = null)
        {
            MessageService._default = !string.IsNullOrEmpty(brokerIP) ? 
                new RemoteMessageDispatcher(Consumer.Connect(MessageChannel.ForProducer(brokerIP)), Producer.Connect(MessageChannel.ForConsumer(brokerIP))) : 
                new LocalMessageDispatcher();
            return config;
        }
        
    }
}
