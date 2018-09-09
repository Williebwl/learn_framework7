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
    /// 更改通行证密码
    /// </summary>
    public struct SYSPassportChangePasswordDTO
    {
        public SYSPassportChangePasswordDTO(string loginName, string oldPassword, string newPassword, string reNewPassword)
            : this()
        {
            this.LoginName = loginName;
            this.OldPassword = oldPassword;
            this.NewPassword = newPassword;
            this.ReNewPassword = reNewPassword;
        }
        public SYSPassportChangePasswordDTO(string loginName, string newPassword)
            : this()
        {
            this.LoginName = loginName;
            this.OldPassword = ALEncrypt.InstanceKey;
            this.NewPassword = newPassword;
            this.ReNewPassword = newPassword;
        }
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 原密码
        /// </summary>
        public string OldPassword { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; }
        /// <summary>
        /// 重复新密码
        /// </summary>
        public string ReNewPassword { get; set; }
    }

    public enum SYSPassportChangePasswordResult
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
        /// 登录名不存在
        /// </summary>
        [Description("登录名不存在")]
        LoginNameDoesNotExist,
        /// <summary>
        /// 原密码不正确
        /// </summary>
        [Description("原密码不正确")]
        PasswordIncorrect,
        /// <summary>
        /// 新密码必需输入6~16个字符
        /// </summary>
        [Description("新密码必需输入6~16个字符")]
        PasswordTooWeak,
        /// <summary>
        /// 两次输入的密码不一致
        /// </summary>
        [Description("两次输入的密码不一致")]
        RePasswordIncorrect,
    }
}
