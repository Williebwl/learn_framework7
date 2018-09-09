using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework
{
    /// <summary>
    /// 请求上下文
    /// </summary>
    public interface IContext
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        IContextUser User { get; }
        /// <summary>
        /// 服务器信息
        /// </summary>
        IContextServer Server { get; }
        /// <summary>
        /// 表示当前请求的页面
        /// </summary>
        IContextPage Page { get; }
    }
}
