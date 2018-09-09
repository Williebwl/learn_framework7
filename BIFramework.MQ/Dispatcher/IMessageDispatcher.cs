using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIStudio.Framework.MQ
{
    /// <summary>
    /// 消息分派
    /// </summary>
    public interface IMessageDispatcher
    {
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="handler">消息响应</param>
        void Subscribe(Action<IMessage> handler);

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="message">消息</param>
        void Dispatch(IMessage message);
    }
}
