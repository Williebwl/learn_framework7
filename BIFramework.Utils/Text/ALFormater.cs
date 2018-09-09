using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace BIStudio.Framework.Utils
{
    /// <summary>
    /// 格式化类
    /// </summary>
    /// <remarks>
    /// [2012-03-11]
    /// </remarks>
    public static class ALFormater
    {
        #region 格式化：日期/时间
        private static DateTime minDate = Convert.ToDateTime("1900-1-1");

        /// <summary>
        /// 格式化短日期
        /// </summary>
        /// <param name="t">日期</param>
        /// <returns>返回字符串日期 格式:yyyy-MM-dd</returns>
        public static string FormatDate(DateTime? t)
        {
            if (t.HasValue && t.Value != minDate)
                return t.Value.ToString("yyyy-MM-dd");
            else
                return "";
        }

        /// <summary>
        /// 格式化短日期
        /// </summary>
        /// <param name="t">日期对象</param>
        /// <returns>返回字符串日期 格式:yyyy-MM-dd</returns>
        public static string FormatDate(object obj)
        {
            DateTime? t = ALConvert.ToDateTime(obj);
            if (t.HasValue && t.Value != minDate)
                return t.Value.ToString("yyyy-MM-dd");
            else
                return "";
        }

        /// <summary>
        /// 格式化长日期
        /// </summary>
        /// <param name="t">日期</param>
        /// <returns>返回字符串日期 格式:yyyy-MM-dd HH:mm:ss</returns>
        public static string FormatDateTime(DateTime? t)
        {
            if (t.HasValue && t.Value != minDate)
                return t.Value.ToString("yyyy-MM-dd HH:mm:ss");
            else
                return "";
        }

        /// <summary>
        /// 格式化长日期
        /// </summary>
        /// <param name="t">日期对象</param>
        /// <returns>返回字符串日期 格式:yyyy-MM-dd HH:mm:ss</returns>
        public static string FormatDateTime(object obj)
        {
            DateTime? t = ALConvert.ToDateTime(obj);
            if (t.HasValue && t.Value != minDate)
                return t.Value.ToString("yyyy-MM-dd HH:mm:ss");
            else
                return "";
        }
        #endregion

        #region 格式化：字符串
        /// <summary>
        /// 格式化掉显示字符串前边的数字以及空格 例："1 合理咯"==>"合理咯"
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FormatStringNumber(string s)
        {
            Regex reg = new Regex(@"^(\d+\s+)\S+");
            Regex regRep = new Regex(@"^(\d+\s+)");
            if (reg.IsMatch(s))
            {
                return regRep.Replace(s, "");
            }
            return s;
            
        }
        /// <summary>
        /// 去除指定字符串起始空格
        /// </summary>
        /// <param name="str"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string TrimStart(this string str, string suffix)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(suffix) || suffix.Length > str.Length)
                return str;
            else if (suffix.Length == str.Length)
                return string.Empty;
            else if (str.StartsWith(suffix))
                return str.Substring(suffix.Length);
            else
                return str;
        }
        /// <summary>
        /// 去除指定字符串末尾空格
        /// </summary>
        /// <param name="str"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string TrimEnd(this string str, string suffix)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(suffix) || suffix.Length > str.Length)
                return str;
            else if (suffix.Length == str.Length)
                return string.Empty;
            else if (str.EndsWith(suffix))
                return str.Substring(0, str.Length - suffix.Length);
            else
                return str;
        }
        #endregion

        #region 格式化：金额/货币
        /// <summary>
        /// 格式:#,###,##0.00
        /// </summary>
        public static string FormatDecimal(decimal? t)
        {
            if (t.HasValue)
                return string.Format("{0}", t.Value.ToString("#,###,##0.00"));
            else
                return t.ToString();
        }

        /// <summary>
        /// 格式:#,###,##0.00
        /// </summary>
        public static string FormatDecimal(object obj)
        {
            decimal? t = ALConvert.ToDecimal(obj);
            return FormatDecimal(t);
        }

        /// <summary>
        /// 格式:#,###,##0.00+%
        /// </summary>
        public static string FormatPercent(decimal? t)
        {
            if (t.HasValue)
                return t.Value.ToString("#,###,##0.00") + "%";
            else
                return t.ToString();
        }

        /// <summary>
        /// 格式:#,###,##0.00+%
        /// </summary>
        public static string FormatPercent(object obj)
        {
            decimal? t = ALConvert.ToDecimal(obj);
            return FormatPercent(t);
        }

        /// <summary>
        /// 报表格式:(#,###,##0.00*100)+%
        /// </summary>
        public static string FormatPercents(decimal? t)
        {
            if (t.HasValue)
                return string.Format("{0:p}", t);
            else
                return t.ToString();
        }

        /// <summary>
        /// 报表格式:(#,###,##0.00*100)+%
        /// </summary>
        public static string FormatPercents(object obj)
        {
            decimal? t = ALConvert.ToDecimal(obj);
            return FormatPercents(t);
        }
        #endregion

        #region 格式化：多行文本
        /// <summary>
        /// 多行文本字符串格式化为HTML，如备注信息
        /// </summary>
        /// <param name="multiText">多行文本字符串</param>
        /// <returns>HTML</returns>
        public static string FormatMultiText(string multiText)
        {
            if (!string.IsNullOrEmpty(multiText))
                return HttpUtility.HtmlEncode(multiText).Replace("\r\n", "\r").Replace("\n", "\r").Replace("\r", "<br/>");
            else
                return multiText;
        }
        #endregion

        #region 替换：HTML
        /// <summary>
        /// HTML替换规则
        /// </summary>
        public enum ReplaceOptions
        {
            /// <summary>
            /// 全部HTML元素
            /// </summary>
            All = 1,
            /// <summary>
            /// 外部元素，包括script,style,iframe等
            /// </summary>
            ExternalEmbeds = 2,
            /// <summary>
            /// HTML元素样式，包括class,style等
            /// </summary>
            Styles = 4,
            /// <summary>
            /// 多媒体内容，包括所有标签
            /// </summary>
            Media = 8,
            /// <summary>
            /// 特殊字符，包括&amp;nbsp;,&amp;gt;等
            /// </summary>
            SpecialCharacter = 256,
            /// <summary>
            /// 输出为多行文本
            /// </summary>
            OutputAsMultiline = 512,
        }

        /// <summary>
        /// 除去HTML中特殊脚本
        /// </summary>
        /// <param name="s">HTML文本</param>
        /// <returns></returns>
        public static string ReplaceHtml(string s)
        {
            return ReplaceHtml(s, ReplaceOptions.All);
        }
        /// <summary>
        /// 除去HTML中特殊脚本
        /// </summary>
        /// <param name="s">HTML文本</param>
        /// <param name="replaceOptions">替换规则</param>
        /// <returns></returns>
        public static string ReplaceHtml(string s, ReplaceOptions replaceOptions)
        {
            Dictionary<string, string> regs = new Dictionary<string, string>();

            if ((replaceOptions | ReplaceOptions.All) == replaceOptions || (replaceOptions | ReplaceOptions.ExternalEmbeds) == replaceOptions)
            {
                regs.Add(@"<script[\s\S]+?</script *>", "");
                regs.Add(@"<style[\s\S]+?</style *>", "");
                regs.Add(@"<iframe[\s\S]+?</iframe *>", "");
                regs.Add(@"<frameset[\s\S]+?</frameset *>", "");
                regs.Add(@" href *= *[\s\S]*?script *:", "");
                regs.Add(@"<!--[\s\S]+?-->", "");
            }

            if ((replaceOptions | ReplaceOptions.All) == replaceOptions || (replaceOptions | ReplaceOptions.Styles) == replaceOptions)
            {
                regs.Add(@" style=""[^""]*""", "");
                regs.Add(@" style='[^']*'", "");
                regs.Add(@" class=""[^""]*""", "");
                regs.Add(@" class='[^']*'", "");
            }

            if ((replaceOptions | ReplaceOptions.All) == replaceOptions || (replaceOptions | ReplaceOptions.Media) == replaceOptions)
            {
                regs.Add(Environment.NewLine, "");
                regs.Add("  "," ");
                if ((replaceOptions | ReplaceOptions.OutputAsMultiline) == replaceOptions)
                {
                    regs.Add(@"\<br[^\>]*\>", Environment.NewLine);
                    regs.Add(@"\</p[^\>]*\>", Environment.NewLine);
                }
                regs.Add(@"<[^>]*>", "");
            }

            foreach (var reg in regs)
            {
                s = Regex.Replace(s, reg.Key, reg.Value, RegexOptions.IgnoreCase);
            }


            if ((replaceOptions | ReplaceOptions.All) == replaceOptions || (replaceOptions | ReplaceOptions.SpecialCharacter) == replaceOptions)
            {
                s = HttpUtility.HtmlDecode(s);
            }
            return s;
        }

        /// <summary>
        /// 替换表格为空的时候用[&nbsp]代替
        /// </summary>
        public static string ReplaceSpace(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "&nbsp;";
            else
                return s;
        }

        /// <summary>
        /// 替换HMLT之空行及换行
        /// </summary>
        public static string ReplaceSpaceLine(string s)
        {
            return s.Replace("&nbsp;", " ").Replace("<br/>", "\n");
        }

        /// <summary>
        /// 替换C#单引号、换行等标签
        /// </summary>
        public static string ReplaceTag(string s)
        {
            s = s.Replace("'", "\"");
            s = s.Replace("\r", @"\r");
            s = s.Replace("\n", @"\n");
            s = s.Replace("\t", @"\t");
            s = s.Replace("\v", @"\v");
            s = s.Replace("\a", @"\a");
            s = s.Replace("\b", @"\b");
            s = s.Replace("\t", @"\t");
            s = s.Replace("\f", @"\f");
            return s;
        }
        #endregion

    }
}
