using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIStudio.Framework.MQ
{
    /// <summary>
    /// 为Command消息通道注册响应
    /// </summary>
    public interface ICommandHandler<T> : IMessageHandler<T> where T : ICommand
    {
    }
}
