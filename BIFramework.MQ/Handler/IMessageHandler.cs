using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIStudio.Framework.MQ
{
    /// <summary>
    /// 为指定的消息通道注册响应
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMessageHandler<T> where T : IMessage
    {
        /// <summary>
        /// 消息响应
        /// </summary>
        /// <param name="evt"></param>
        void Handle(T evt);
    }
}
