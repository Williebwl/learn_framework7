using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web;

namespace BIStudio.Framework.Utils
{
    /// <summary>
    /// 支持三种加密：TripleDES，MD5，BASE64
    /// </summary>
    /// <remarks>
    /// 密码一般采用MD5加密，业务数据一般采用BASE64加密
    /// [2012-03-11]
    /// </remarks>
    public static class ALEncrypt
    {
        /// <summary>
        /// 主密钥
        /// </summary>
        internal const string key = @"]#gfV'8P3]@x""R:Z";
        /// <summary>
        /// 实例密钥，系统加载时随机生成
        /// </summary>
        public static string InstanceKey = new Guid().ToString("N");

        #region TripleDES加密/解密

        #region 加密：TripleDES
        /// <summary>
        /// 使用TripleDES加密
        /// </summary>
        /// <param name="?">需要加密的字符串</param>
        /// <returns></returns>
        public static string TripleDESEnCode(string str)
        {
            return TripleDESEnCode(str, key);
        }
        /// <summary>
        /// 使用TripleDES加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <param name="key">16或24位密钥</param>
        /// <returns></returns>
        public static string TripleDESEnCode(string str, string key)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            byte[] data = UnicodeEncoding.Unicode.GetBytes(str);
            byte[] keys = ASCIIEncoding.ASCII.GetBytes(key);

            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider()
            {
                Key = keys,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cryp = des.CreateEncryptor();//加密

            return Convert.ToBase64String(cryp.TransformFinalBlock(data, 0, data.Length));
        }
        #endregion 解密

        #region 解密：TripleDES
        /// <summary>
        /// 使用TripleDES解密
        /// </summary>
        /// <param name="str">需要解密的字符串</param>
        /// <returns></returns>
        public static string TripleDESDeCode(string str)
        {
            return TripleDESDeCode(str, key);
        }
        /// <summary>
        /// 使用TripleDES解密
        /// </summary>
        /// <param name="str">需要解密的字符串</param>
        /// <param name="key">16或24位密钥</param>
        /// <returns></returns>
        public static string TripleDESDeCode(string str, string key)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(key) || str.Length % 4 != 0)
            {
                return string.Empty;
            }

            byte[] data = Convert.FromBase64String(str);
            byte[] keys = ASCIIEncoding.ASCII.GetBytes(key);

            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider()
            {
                Key = keys,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cryp = des.CreateDecryptor();//解密

            string returnValue = string.Empty;
            try
            {
                returnValue = UnicodeEncoding.Unicode.GetString(cryp.TransformFinalBlock(data, 0, data.Length));
            }
            catch
            {
            }
            return returnValue;
        }
        #endregion 

        #endregion

        #region MD5加密
        /// <summary>
        /// Md5加密，用于密码非可逆加密
        /// </summary>
        /// <param name="str">字符串参数</param>
        /// <returns>返回字符串值</returns>
        public static string Md5hash(string str)
        {
            string pwd = "";
            MD5 md5 = MD5.Create();

            byte[] s = md5.ComputeHash(Encoding.Unicode.GetBytes(str));

            for (int i = 0; i < s.Length; i++)
            {
                pwd = pwd + s[i].ToString("x");
            }
            return pwd;

        }

        public static string Md5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] temp = md5.ComputeHash(UnicodeEncoding.UTF8.GetBytes(str));
            return BitConverter.ToString(temp).Replace("-", "");
        }

        #endregion

        #region HMAC加密
        /// <summary>
        /// HMAC加密，用于密码非可逆加密
        /// </summary>
        /// <param name="str">字符串参数</param>
        /// <returns>返回字符串值</returns>
        public static byte[] HMAC(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            HMAC hmac = HMACSHA512.Create();
            hmac.Key = Encoding.Default.GetBytes(key);
            return hmac.ComputeHash(Encoding.Default.GetBytes(str));
        }
        #endregion

        #region BASE64加密/解密

        #region  base64加密
        /// <summary>
        /// base64加密字符串，一般性可逆加密,采用utf8编码方式加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string Base64EnCode(string str)
        {
            byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(str);
            try
            {
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return str;
            }

        }
        #endregion

        #region base64解密
        /// <summary>
        /// base64解密字符串
        /// </summary>
        /// <param name="str">需要解密的字符串</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64DeCode(string str)
        {
            byte[] bytes = Convert.FromBase64String(str);
            try
            {
                return Encoding.GetEncoding("utf-8").GetString(bytes);
            }
            catch
            {
                return str;
            }
        }
        #endregion 

        #endregion

        #region Crc12加密

        private static ushort[] crc12Table = null;
        /// <summary>
        /// Crc12密码表
        /// </summary>
        private static ushort[] Crc12Table
        {
            get
            {
                if (crc12Table == null)
                {
                    crc12Table = new ushort[256];
                    for (ushort i = 0; i < 256; i++)
                    {
                        ushort crc = (ushort)(i << 4);
                        for (ushort j = 0; j < 8; j++)
                            crc = (ushort)((crc << 1) ^ ((crc & 0x800) != 0 ? 0x80f : 0));
                        crc12Table[i] = (ushort)(crc & 0xfff);
                    }
                }
                return crc12Table;
            }
        }

        /// <summary>
        /// 计算Crc12校验码
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static ushort Crc12(byte[] buffer)
        {
            ushort crc = 0;
            foreach (byte data in buffer)
                crc = (ushort)(Crc12Table[((crc >> 4) ^ data) & 0xff] ^ (crc << 8));
            return (ushort)(crc & 0xfff);
        }

        #endregion

        #region URL编码/解码

        //#region URL编码
        ///// <summary>
        ///// URL编码，默认UTF-8编码
        ///// </summary>
        ///// <param name="str">需要编码的字符串</param>
        ///// <param name="encodingType">编码类型</param>
        ///// <returns></returns>
        //public static string UrlEnCode(string str, Encoding encodingType)
        //{
        //    if (encodingType == null)
        //        encodingType = Encoding.UTF8;

        //    return HttpUtility.UrlEncode(Base64EnCode(str), encodingType);
        //}

        ///// <summary>
        ///// URL编码，默认UTF-8编码
        ///// </summary>
        ///// <param name="str"></param>
        ///// <returns></returns>
        //public static string UrlEnCode(string str)
        //{
        //    return UrlEnCode(str, null);
        //}
        //#endregion

        #region URL解码
        /// <summary>
        /// URL解码，默认UTF-8解码
        /// </summary>
        /// <param name="str">需要解码的字符串</param>
        /// <param name="encodingType">解码类型</param>
        /// <returns></returns>
        public static string UrlDecode(string str, Encoding encodingType)
        {
            if (encodingType == null)
                encodingType = Encoding.UTF8;
            return Base64DeCode(HttpUtility.UrlDecode(str, encodingType).Replace(" ", "+"));
        }

        /// <summary>
        /// URL解码，默认UTF-8解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlDecode(string str)
        {
            return UrlDecode(str, null);
        }
        #endregion 

        #endregion


    }
}
