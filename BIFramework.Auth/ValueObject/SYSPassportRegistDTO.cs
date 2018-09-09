using BIStudio.Framework.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Auth
{
    /// <summary>
    /// 通行证注册
    /// </summary>
    public struct SYSPassportRegistDTO
    {
        public SYSPassportRegistDTO(string loginName, string password)
            :this()
        {
            this.LoginName = loginName;
            this.Password = password;
            this.RePassword = password;
            this.Email = loginName + "@bicms.net";
        }
        public SYSPassportRegistDTO(string loginName, string password, string rePassword, string email)
            : this()
        {
            this.LoginName = loginName;
            this.Password = password;
            this.RePassword = rePassword;
            this.Email = email;
        }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
        public string Email { get; set; }
        public string Remarks { get; set; }
    }

    public enum SYSPassportRegistResult
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
        /// 账号已存在
        /// </summary>
        [Description("账号已存在")]
        LoginNameAlreadyExists,
        /// <summary>
        /// 账号格式不正确
        /// </summary>
        [Description("账号格式不正确")]
        LoginNameInvalid,
        /// <summary>
        /// 密码必需输入6~16个字符
        /// </summary>
        [Description("密码必需输入6~16个字符")]
        PasswordTooWeak,
        /// <summary>
        /// 两次输入的密码不一致
        /// </summary>
        [Description("两次输入的密码不一致")]
        RePasswordIncorrect,
        /// <summary>
        /// 电子邮箱已被其他用户使用
        /// </summary>
        [Description("电子邮箱已被其他用户使用")]
        EmailAlreadyExists,
        /// <summary>
        /// 电子邮箱格式不正确
        /// </summary>
        [Description("电子邮箱格式不正确")]
        EmailInvalid
    }
}
