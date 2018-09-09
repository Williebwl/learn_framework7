using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.MQ
{
    /// <summary>
    /// 消息通道
    /// </summary>
    public struct MessageChannel
    {
        public MessageChannel(MessageProtocol protocol, string address, MessageActor port)
            : this()
        {
            this.Protocol = protocol;
            this.Address = address;
            this.Port = (int)port;
        }
        public MessageChannel(MessageProtocol protocol, string address, int port)
            : this()
        {
            this.Protocol = protocol;
            this.Address = address;
            this.Port = port;
        }
        public override string ToString()
        {
            return this.Protocol + "://" + this.Address + ":" + this.Port;
        }
        public override bool Equals(object obj)
        {
            return string.Equals(this, obj);
        }
        /// <summary>
        /// 通讯协议
        /// </summary>
        public MessageProtocol Protocol { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// IP端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 定义生产端口
        /// </summary>
        public static MessageChannel ForProducer(string ip, int port = 6540, MessageProtocol protocol = MessageProtocol.tcp)
        {
            return new MessageChannel(protocol, ip, port + MessageActor.Producer);
        }
        /// <summary>
        /// 定义生产端口
        /// </summary>
        public static IEnumerable<MessageChannel> ForProducer(IEnumerable<string> ips)
        {
            return ips.Select(ip => ForProducer(ip));
        }
        /// <summary>
        /// 定义消费端口
        /// </summary>
        public static MessageChannel ForConsumer(string ip, int port = 6540, MessageProtocol protocol = MessageProtocol.tcp)
        {
            return new MessageChannel(protocol, ip, port + MessageActor.Consumer);
        }
        /// <summary>
        /// 定义消费端口
        /// </summary>
        public static IEnumerable<MessageChannel> ForConsumer(IEnumerable<string> ips)
        {
            return ips.Select(ip => ForConsumer(ip));
        }
    }

    #region 通讯端口

    /// <summary>
    /// 消息通讯端口
    /// </summary>
    public enum MessageActor
    {
        /// <summary>
        /// 生产者
        /// </summary>
        Producer,
        /// <summary>
        /// 消费者
        /// </summary>
        Consumer,
    }

    #endregion

    #region 通讯协议

    /// <summary>
    /// 消息通讯协议
    /// </summary>
    public enum MessageProtocol
    {
        /// <summary>
        /// 在多台主机之间通讯
        /// </summary>
        tcp,
        /// <summary>
        /// 在同一台主机的多个进程之间通讯（NetMQ不支持）
        /// </summary>
        ipc,
        /// <summary>
        /// 在同一个进程的多个线程之间通讯
        /// </summary>
        inproc,
        /// <summary>
        /// 使用MSMQ进行通讯
        /// </summary>
        pgm,
    }
    #endregion
}
