using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace BIStudio.Framework.Utils
{
    /// <summary>
    /// 安全令牌
    /// </summary>
    public class ALSecurityToken
    {
        /// <summary>
        /// 加密算法
        /// </summary>
        public enum EncryMode
        {
            /// <summary>
            /// 使用HMAC加密算法
            /// </summary>
            HMAC = 1,
            /// <summary>
            /// 使用MD5加密算法
            /// </summary>
            MD5 = 2,
        }
        /// <summary>
        /// 加密编码
        /// </summary>
        public enum EncryEncoding
        {
            /// <summary>
            /// 使用Default编码
            /// </summary>
            Default = 936,
            /// <summary>
            /// 使用UTF8编码
            /// </summary>
            UTF8 = 65001,
        }

        protected EncryMode encryMode = EncryMode.HMAC;
        protected EncryEncoding encryEncoding = EncryEncoding.UTF8;
        protected int timeoutMinutes = 3;
        protected string masterKey = null;

        public ALSecurityToken()
        {
        }
        /// <summary>
        /// 初始化新的安全令牌
        /// </summary>
        /// <param name="timeoutMinutes">超时时间</param>
        public ALSecurityToken(int timeoutMinutes)
        {
            this.timeoutMinutes = timeoutMinutes;
        }
        /// <summary>
        /// 初始化新的安全令牌
        /// </summary>
        /// <param name="encryMode">加密方式</param>
        /// <param name="encryEncoding">加密编码</param>
        public ALSecurityToken(EncryMode encryMode, EncryEncoding encryEncoding)
        {
            this.encryMode = encryMode;
            this.encryEncoding = encryEncoding;
        }
        /// <summary>
        /// 初始化新的安全令牌
        /// </summary>
        /// <param name="encryMode">加密方式</param>
        /// <param name="encryEncoding">加密编码</param>
        /// <param name="masterKey">公钥</param>
        public ALSecurityToken(EncryMode encryMode, EncryEncoding encryEncoding, string masterKey)
        {
            this.encryMode = encryMode;
            this.encryEncoding = encryEncoding;
            this.masterKey = masterKey;
        }
        /// <summary>
        /// 初始化新的安全令牌
        /// </summary>
        /// <param name="encryMode">加密方式</param>
        /// <param name="encryEncoding">加密编码</param>
        /// <param name="timeoutMinutes">超时时间</param>
        public ALSecurityToken(EncryMode encryMode, EncryEncoding encryEncoding, int timeoutMinutes)
        {
            this.encryMode = encryMode;
            this.encryEncoding = encryEncoding;
            this.timeoutMinutes = timeoutMinutes;
        }
        /// <summary>
        /// 初始化新的安全令牌
        /// </summary>
        /// <param name="encryMode">加密方式</param>
        /// <param name="encryEncoding">加密编码</param>
        /// <param name="masterKey">公钥</param>
        /// <param name="timeoutMinutes">超时时间</param>
        public ALSecurityToken(EncryMode encryMode, EncryEncoding encryEncoding, string masterKey, int timeoutMinutes)
        {
            this.encryMode = encryMode;
            this.encryEncoding = encryEncoding;
            this.masterKey = masterKey;
            this.timeoutMinutes = timeoutMinutes;
        }

        #region 高级加密选项
        
        /// <summary>
        /// 初始化新的安全令牌
        /// </summary>
        /// <param name="encryMode">加密方式</param>
        /// <param name="encryEncoding">加密编码</param>
        [Obsolete("请使用ALSecurityToken.EncryEncoding枚举传递编码方式")]
        public ALSecurityToken(EncryMode encryMode, Encoding encryEncoding)
        {
            this.encryMode = encryMode;
            this.encryEncoding = (EncryEncoding)encryEncoding.CodePage;
        }
        /// <summary>
        /// 初始化新的安全令牌
        /// </summary>
        /// <param name="encryMode">加密方式</param>
        /// <param name="encryEncoding">加密编码</param>
        /// <param name="timeoutMinutes">超时时间</param>
        [Obsolete("请使用ALSecurityToken.EncryEncoding枚举传递编码方式")]
        public ALSecurityToken(EncryMode encryMode, Encoding encryEncoding, int timeoutMinutes)
        {
            this.encryMode = encryMode;
            this.encryEncoding = (EncryEncoding)encryEncoding.CodePage;
            this.timeoutMinutes = timeoutMinutes;
        }
        #endregion

        /// <summary>
        /// 为指定的数据创建安全令牌
        /// </summary>
        /// <param name="key">动态密钥</param>
        /// <returns></returns>
        public string Create(string key)
        {
            return Create(key, DateTime.Now);
        }

        /// <summary>
        /// 为指定的数据创建安全令牌
        /// </summary>
        /// <param name="key">动态密钥</param>
        /// <param name="time">令牌创建时间</param>
        /// <returns></returns>
        public string Create(string key, DateTime time)
        {
            string masterKey = this.masterKey ?? ALConfig.DllConfigs["BIUtils"]["SecurityConfig"]["MasterKey"];
            string inputStr = key + "|" + masterKey + "|" + time.ToString("yyyy-MM-dd HH:mm");
            Encoding encoding = Encoding.GetEncoding((int)this.encryEncoding);
            switch (this.encryMode)
            {
                case EncryMode.HMAC:
                    HMAC hmac = HMACSHA512.Create();                    
                    hmac.Key = encoding.GetBytes(ALEncrypt.key);
                    byte[] hmacData = hmac.ComputeHash(encoding.GetBytes(inputStr));
                    return BitConverter.ToString(hmacData).Replace("-", "");
                case EncryMode.MD5:
                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] md5Data = md5.ComputeHash(encoding.GetBytes(inputStr));
                    return BitConverter.ToString(md5Data).Replace("-", "");
                default:
                    throw new ArgumentOutOfRangeException("encryMode");
            }
        }

        /// <summary>
        /// 验证指定的安全令牌是否可用
        /// </summary>
        /// <param name="key">动态密钥</param>
        /// <param name="token">安全令牌</param>
        /// <returns></returns>
        public bool Verify(string key, string token)
        {
            DateTime now = DateTime.Now;
            for (int i = -this.timeoutMinutes; i <= this.timeoutMinutes; i++)
            {
                if (Create(key, now.AddMinutes(i)) == token)
                    return true;
            }
            return false;
        }

    }

}
