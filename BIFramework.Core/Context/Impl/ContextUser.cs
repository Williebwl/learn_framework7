using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework
{
    /// <summary>
    /// 表示当前登录用户
    /// </summary>
    public struct ContextUser : IContextUser
    {
        public ContextUser(
            long systemID = default(long),
            long userID = default(long),
            string userIP = default(string),
            string loginName = default(string),
            string userName = default(string),
            string[] groups = default(string[]),
            string[] operations = default(string[]))
            : this()
        {
            this.SystemID = systemID;
            this.ID = userID;
            this.IP = userIP;
            this.LoginName = loginName;
            this.UserName = userName;
            this.Identity = new ContextUserToken("Basic", userID > 0, loginName, operations);
            this.Groups = groups;
        }

        public long SystemID
        {
            get;
            private set;
        }
        public long ID
        {
            get;
            private set;
        }
        public string IP
        {
            get;
            private set;
        }
        public string LoginName
        {
            get;
            private set;
        }
        public string UserName
        {
            get;
            private set;
        }
        public string[] Groups
        {
            get;
            private set;
        }
        public IContextUserToken Identity
        {
            get;
            private set;
        }
        public bool IsInRole(string role)
        {
            return this.Groups != null && this.Groups.Contains(role);
        }
        IIdentity IPrincipal.Identity
        {
            get { return this.Identity; }
        }
    }
}
