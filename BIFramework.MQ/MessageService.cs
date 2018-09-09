using System.Collections.Concurrent;
using BIStudio.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIStudio.Framework.MQ
{
    /// <summary>
    /// 消息管理
    /// </summary>
    public static class MessageService
    {
        internal static IMessageDispatcher _default = null;
        /// <summary>
        /// 获得默认实例
        /// </summary>
        public static IMessageDispatcher Default
        {
            get
            {
                return _default = _default ?? GetInstance();
            }
        }

        /// <summary>
        /// 创建消息分派器
        /// </summary>
        /// <returns></returns>
        public static IMessageDispatcher GetInstance()
        {
            return new MessageDispatcher();
        }

    }
}
