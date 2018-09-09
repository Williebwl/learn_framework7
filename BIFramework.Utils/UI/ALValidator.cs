using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace BIStudio.Framework.Utils
{
       /// <summary>
       /// Intro:验证 网址，IP，邮箱，电话，手机，数字，英文，日期，身份证，邮编
       /// </summary>
       /// <remarks>
       /// 针对某个值进行有效性验证
       /// [2012-03-11]
       /// </remarks>
    public static class ALValidator
    {

        #region 验证邮箱
        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="email">Email地址</param>
        /// <returns></returns>
        public static bool IsEmail(string email)
        {
            return Regex.IsMatch(email, @"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", RegexOptions.IgnoreCase);
        }

        public static bool HasEmail(string email)
        {
            return Regex.IsMatch(email, @"[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})", RegexOptions.IgnoreCase);
        }
        #endregion

        #region 验证网址
        /// <summary>
        /// 验证网址是否有效
        /// </summary>
        /// <param name="url">网址</param>
        /// <returns></returns>
        public static bool IsUrl(string url)
        {
            return Regex.IsMatch(url, @"^(((file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://)|(www\.))+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(/[a-zA-Z0-9\&amp;%_\./-~-]*)?$", RegexOptions.IgnoreCase);
        }

        public static bool HasUrl(string url)
        {
            return Regex.IsMatch(url, @"(((file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://)|(www\.))+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(/[a-zA-Z0-9\&amp;%_\./-~-]*)?", RegexOptions.IgnoreCase);
        }
        #endregion

        #region 验证日期
        /// <summary>
        /// 验证日期格式是否有效
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDateTime(string str)
        {
            try
            {
                DateTime time = Convert.ToDateTime(str);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 验证手机号
        /// <summary>
        /// 验证手机号，目前只支持中国手机号码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsMobile(string mobile)
        {
            return Regex.IsMatch(mobile, @"^(13[0-9]|15[0-9]|18[0-9]|14[0-9])\d{8}$", RegexOptions.IgnoreCase);
        }

        public static bool HasMobile(string mobile)
        {
            return Regex.IsMatch(mobile, @"^(13[0-9]|15[0-9]|18[0-9]|14[0-9])\d{8}$", RegexOptions.IgnoreCase);
        }
        #endregion

        #region 验证IP地址
        /// <summary>
        /// 验证IP是否有效
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$", RegexOptions.IgnoreCase);
        }

        public static bool HasIP(string ip)
        {
            return Regex.IsMatch(ip, @"(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])", RegexOptions.IgnoreCase);
        }
        #endregion

        #region 验证身份证号码
        /// <summary>
        /// 验证身份证是否有效，目前只支持中国身份证号码
        /// </summary>
        /// <param name="Id">身份证号码</param>
        /// <returns></returns>
        public static bool IsIDCard(string Id)
        {
            if (Id.Length == 18)
            {
                bool check = IsIDCard18(Id);
                return check;
            }
            else if (Id.Length == 15)
            {
                bool check = IsIDCard15(Id);
                return check;
            }
            else
            {
                return false;
            }
        }

        public static bool IsIDCard18(string Id)
        {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准
        }

        public static bool IsIDCard15(string Id)
        {
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            return true;//符合15位身份证标准
        }
        #endregion

        #region 验证是否Int型
        /// <summary>
        /// 是不是Int型的
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInt(string str)
        {
            Regex regex = new Regex(@"^(-){0,1}\d+$");
            if (regex.Match(str).Success)
            {
                if ((long.Parse(str) > 0x7fffffffL) || (long.Parse(str) < -2147483648L))
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        #endregion

        #region 验证字符串长度
        /// <summary>
        /// 看字符串的长度是不是在限定数之间（一个中文为两个字符）
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="begin">大于等于</param>
        /// <param name="end">小于等于</param>
        /// <returns></returns>
        public static bool IsLengthStr(string source, int begin, int end)
        {
            int length = Regex.Replace(source, @"[^\x00-\xff]", "OK").Length;
            if (length < begin || length > end)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 验证电话号码
        /// <summary>
        /// 目前只支持中国电话，格式010-85849685
        /// </summary>
        /// <param name="str">电话号码</param>
        /// <returns></returns>
        public static bool IsTel(string source)
        {
            return Regex.IsMatch(source, @"^\d{3,4}-?\d{6,8}$", RegexOptions.IgnoreCase);
        }
        #endregion

        #region 验证邮政编码
        /// <summary>
        /// 目前只支持中国邮政编码，即6个数字
        /// </summary>
        /// <param name="str">邮编号码</param>
        /// <returns></returns>
        public static bool IsPostCode(string str)
        {
            return Regex.IsMatch(str, @"^\d{6}$", RegexOptions.IgnoreCase);
        }
        #endregion

        #region 验证中文
        /// <summary>
        /// 验证字符是否为中文汉字
        /// </summary>
        /// <param name="str">汉字字符串</param>
        /// <returns></returns>
        public static bool IsChineseChar(string str)
        {
            return Regex.IsMatch(str, @"^[\u4e00-\u9fa5]+$", RegexOptions.IgnoreCase);
        }

        public static bool hasChineseChar(string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]+", RegexOptions.IgnoreCase);
        }
        #endregion

        #region 验证“字母+数字+下划线”的组合
        /// <summary>
        /// 验证是不是正常字符 字母，数字，下划线的组合
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNormalChar(string str)
        {
            return Regex.IsMatch(str, @"[\w\d_]+", RegexOptions.IgnoreCase);
        }
        #endregion

        /// <summary>
        /// 验证给定的URL是否为图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsImage(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            return new string[] { ".jpeg", ".jpg", ".png", ".tif", ".tiff", ".bmp", ".gif" }.FirstOrDefault(d => url.ToLower().EndsWith(d)) != null;
        }

    }
}
