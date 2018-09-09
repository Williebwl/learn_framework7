using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework
{
    /// <summary>
    /// 表示当前登录用户的令牌
    /// </summary>
    internal struct ContextUserToken : IContextUserToken
    {
        public ContextUserToken(
            string authenticationType = default(string), 
            bool isAuthenticated = default(bool), 
            string name = default(string),
            string[] operations = default(string[]))
            : this()
        {
            this.AuthenticationType = authenticationType;
            this.IsAuthenticated = isAuthenticated;
            this.Name = name;
            this.Operations = operations;
        }
        public string AuthenticationType
        {
            get;
            private set;
        }

        public bool IsAuthenticated
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }
        public string[] Operations
        {
            get;
            private set;
        }
        public bool HasPermission(string operation)
        {
            return this.Operations != null && this.Operations.Contains(operation);
        }
    }
}
