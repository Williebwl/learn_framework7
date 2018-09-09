using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIStudio.Framework.Utils;
using System.ComponentModel;

namespace BIStudio.Framework.Auth
{
    public struct SYSPassworkForgetDTO
    {
        public SYSPassworkForgetDTO(string email,string account,string code)
            : this()
        {
            Email = email;
            Account = account;
            VerificationCode = code;
        }

        public string Email { get; set; }

        public string Account { get; set; }

        public string VerificationCode { get; set; }
    }

    public enum STDPassworkForgetResult
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
        /// 电子邮箱不正确
        /// </summary>
        [Description("电子邮箱不正确")]
        EmailIncorrect,
    }
}
