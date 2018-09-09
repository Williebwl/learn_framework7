
using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Auth
{
    /// <summary>
    /// 平台通行证
    /// </summary>
    [Table("SYSPassport")]
    public class SYSPassport : Entity
    {
        /// <summary>
        /// 自动解锁时间(分钟)
        /// </summary>
        private const int AutoUnlockMinutes = 60 * 2;
        /// <summary>
        /// 连续登录异常次数
        /// </summary>
        private const int MaxLoginError = 4;

        #region 数据属性
        /// <summary>
        /// 登陆名
        /// </summary>
        [Column(IsExact = true)]
        public string LoginName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public byte[] Password { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        /// <summary>
        /// 连续登录异常
        /// </summary>
        public int? LastLoginError { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? IsValid { get; set; }
        /// <summary>
        /// 是否密码锁定
        /// </summary>
        public bool? IsLocked { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Inputer { get; set; }
        /// <summary>
        /// 创建人的ID
        /// </summary>
        public long? InputerID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? InputTime { get; set; }
        /// <summary>
        /// 找回密码的验证码
        /// </summary>
        public string VerificationCode { get; set; }

        #endregion


        /// <summary>
        /// 计算密码
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public byte[] ComputePassword(string loginName, string password)
        {
            return ALEncrypt.HMAC(loginName + "|" + password);
        }

        /// <summary>
        /// 检查登录锁定
        /// </summary>
        /// <returns></returns>
        public bool CheckLock()
        {
            return LastLoginTime != null && (LastLoginTime.Value.AddMinutes(AutoUnlockMinutes) > DateTime.Now && IsLocked == true);
        }

        public void UnLock()
        {
            IsLocked = false;
            LastLoginError = 0;
        }

        public bool CheckPassword(string password)
        {
            return BitConverter.ToString(Password) == BitConverter.ToString(ComputePassword(LoginName, password));
        }

        public void PasswordError()
        {
            LastLoginError++;
            if (LastLoginError >= MaxLoginError)
                IsLocked = true;
        }
    }

}
