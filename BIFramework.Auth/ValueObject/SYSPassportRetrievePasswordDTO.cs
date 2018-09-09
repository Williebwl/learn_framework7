using BIStudio.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Auth
{
    /// <summary>
    /// 找回密码
    /// </summary>
    public struct SYSPassportRetrievePasswordDTO
    {
        public SYSPassportRetrievePasswordDTO(string loginName, string password,string confirmedPassword)
            :this()
        {
            LoginName = loginName;
            Password = password;
            ConfirmedPassword = confirmedPassword;
        }
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string ConfirmedPassword { get; set; }
    }
    public enum SYSPassportRetrievePasswordResult
    {
        /// <summary>
        /// 操作成功
        /// </summary>
        [ALEnumDescription("操作成功")]
        Success = 0,
        /// <summary>
        /// 操作失败
        /// </summary>
        [ALEnumDescription("{0}")]
        Fail,
        /// <summary>
        /// 登录名不存在
        /// </summary>
        [ALEnumDescription("登录名不存在")]
        LoginNameDoesNotExist,
        /// <summary>
        /// 电子邮箱不正确
        /// </summary>
        [ALEnumDescription("电子邮箱不正确")]
        EmailIncorrect,
    }
}
