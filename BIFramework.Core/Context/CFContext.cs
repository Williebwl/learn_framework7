using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BIStudio.Framework
{
    /// <summary>
    /// 请求上下文
    /// </summary>
    public static class CFContext
    {
        /// <summary>
        /// 当前默认实例
        /// </summary>
        private static IContext Current
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Items != null)
                    return (IContext)HttpContext.Current.Items["CFContext"] ?? new EmptyContext();
                else
                    return new EmptyContext();
            }
            set
            {
                if (HttpContext.Current != null && HttpContext.Current.Items != null)
                    HttpContext.Current.Items["CFContext"] = value;
            }
        }
        /// <summary>
        /// 设置当前线程的请求上下文
        /// </summary>
        /// <param name="current"></param>
        public static void SetCurrent(IContext current)
        {
            Current = current;
        }
        /// <summary>
        /// 用户信息
        /// </summary>
        public static IContextUser User
        {
            get
            {
                return Current.User;
            }
        }
        /// <summary>
        /// 服务器信息
        /// </summary>
        public static IContextServer Server
        {
            get
            {
                return Current.Server;
            }
        }
        /// <summary>
        /// 页面信息
        /// </summary>
        public static IContextPage Page
        {
            get
            {
                return Current.Page;
            }
        }

    }
}
