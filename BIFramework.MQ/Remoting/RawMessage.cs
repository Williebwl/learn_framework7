using BIStudio.Framework.Utils;
using NetMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.MQ
{
    /// <summary>
    /// 通讯消息
    /// </summary>
    public struct RawMessage
    {
        /// <summary>
        /// 消息主题
        /// </summary>
        public string Topic { get; set; }
        /// <summary>
        /// 消息信头
        /// </summary>
        public byte[] Header { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Body { get; set; }

        public static RawMessage Parse(IMessage message)
        {
            return new RawMessage
            {
                Topic = message.TypeName,
                Header = BitConverter.GetBytes(message.ID).Concat(BitConverter.GetBytes(message.Timestamp.ToBinary())).ToArray(),
                Body = ALSerialize.JsonSerialize(message),
            };
        }

        public IMessage Generalize(Type messageType)
        {
            IMessage message = (IMessage)ALSerialize.JsonDeserialize(this.Body, messageType);
            message.ID = BitConverter.ToInt64(this.Header, 0);
            message.TypeName = this.Topic;
            message.Timestamp = DateTime.FromBinary(BitConverter.ToInt64(this.Header, 8));
            return message;
        }
    }
}
