using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework
{
    /// <summary>
    /// 无法获取到当前请求上下文
    /// </summary>
    internal sealed class EmptyContext : IContext
    {
        public EmptyContext()
        {
            this.User = new ContextUser();
            this.Server = new ContextServer();
            this.Page = new ContextPage();
        }
        public IContextUser User
        {
            get;
            private set;
        }
        public IContextServer Server
        {
            get;
            private set;
        }
        public IContextPage Page
        {
            get;
            private set;
        }
    }
}
