using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIStudio.Framework.MQ
{
    /// <summary>
    /// 消息侦听器
    /// </summary>
    public sealed class MessageListener
    {
        /// <summary>
        /// 消息原始类型
        /// </summary>
        public Type Target { get; set; }
        /// <summary>
        /// 消息响应
        /// </summary>
        public Action<IMessage> Handler { get; set; } 
    }
}
