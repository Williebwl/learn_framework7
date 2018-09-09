using BIStudio.Framework.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Auth
{
    public struct SYSAuthorizeLoginDTO
    {
        public SYSAuthorizeLoginDTO(string code, string username, string password)
            : this()
        {
            this.code = code;
            this.username = username;
            this.password = password;
        }
        public string code { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
    public enum STDAuthorizeLoginResult
    {
        /// <summary>
        /// 操作成功
        /// </summary>
        [Description("操作成功")]
        Success = 0,
        /// <summary>
        /// 操作失败
        /// </summary>
        [Description("{0}")]
        Fail,
        /// <summary>
        /// 授权码无效
        /// </summary>
        [Description("授权码无效")]
        AuthorizeCodeInvalid,
        /// <summary>
        /// 账号或密码无效
        /// </summary>
        [Description("账号或密码无效")]
        AccountOrPasswordInvalid,
    }
}
