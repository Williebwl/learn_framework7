using NetMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.MQ
{
    /// <summary>
    /// 使用NetMQ发送消息
    /// </summary>
    public static class RawMessageExtensions
    {
        public static void Send(this IOutgoingSocket socket, RawMessage message)
        {
            NetMQMessage mqMessage = new NetMQMessage();
            mqMessage.Append(message.Topic);
            mqMessage.Append(message.Header);
            mqMessage.Append(message.Body, Encoding.UTF8);
            socket.SendMultipartMessage(mqMessage);

        }
        public static RawMessage Receive(this IReceivingSocket socket)
        {
            NetMQMessage mqMessage = socket.ReceiveMultipartMessage();
            return new RawMessage
            {
                Topic = mqMessage[0].ConvertToString(),
                Header = mqMessage[1].ToByteArray(),
                Body = mqMessage[2].ConvertToString(Encoding.UTF8),
            };
        }

    }
}
