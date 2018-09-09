using BIStudio.Framework.Domain;
using BIStudio.Framework.MQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.BestPractice.Impl
{
    /// <summary>
    /// 请求创建用户
    /// </summary>
    public class CreateUserCommand : Command
    {
        public CreateUserCommand()
        {

        }
        public CreateUserCommand(string userName)
        {
            this.UserName = userName;
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public long? UserID { get; set; }
    }
}
