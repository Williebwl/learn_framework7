using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace BIStudio.Framework.Utils
{
    /// <summary>
    /// 其他杂七杂八的工具类方法
    /// </summary>
    /// <remarks>
    /// 提取工作中比较通用的工具类方法，且无法进行合理的归类
    /// [2012-03-11]
    /// </remarks>
    public static class ALUtils
    {
        #region 填充Table行号
        /// <summary>
        /// 填充Table行号
        /// </summary>
        public static DataTable FillTableNumber(DataTable dt, string s)
        {
            dt.Columns.Add("RowID");
            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                dt.Rows[i - 1]["RowID"] = i.ToString() + s;
            }
            return dt;
        }
        #endregion

        #region 输出限长字符串
        /// <summary>
        /// 输出限长字符串
        /// </summary>
        /// <param name="sText">字符串</param>
        /// <param name="iLength">限制的长度</param>
        /// <returns>限长后的字符串</returns>
        public static string LimitStringLength(string sText, int iLength)
        {
            if (string.IsNullOrEmpty(sText))
                return sText;

            sText = sText.TrimEnd();
            if (iLength < 1) return sText;
            byte[] b = System.Text.Encoding.Default.GetBytes(sText);
            double n = 0.0;
            int m = 0;
            bool l0 = false, l1 = false, l2 = false;
            for (int i = 0; i < b.Length; i++)
            {
                l0 = ((int)b[i] > 128);
                if (l0) i++;
                n += (l0 ? 1.0 : 0.5);
                if (n > iLength)
                {
                    string strOut = (l2 ? sText.Substring(0, m - 1) : sText.Substring(0, m - 2));
                    if (System.Text.Encoding.GetEncoding("GB2312").GetByteCount(strOut) + 2 > iLength * 2)
                        strOut = strOut.Substring(0, strOut.Length - 1);
                    return strOut + "..";
                }
                m++;
                l2 = l1;
                l1 = l0;
            }
            return sText;
        }
        #endregion

        #region 取全球唯一码
        /// <summary>
        /// 取全球唯一码
        /// </summary>
        /// <returns></returns>
        public static string GetGUID()
        {
            return System.Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 取较短的全球唯一码
        /// </summary>
        /// <returns></returns>
        public static string GetGUIDShort()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }
        #endregion

        #region 获得随机字符串
        /// <summary>
        /// 获得随机字符串
        /// </summary>
        /// <returns></returns>
        public static string GetRandom()
        {
            Random ra = new Random();
            System.Threading.Thread.Sleep(10);//延迟1毫秒,防止出现相同的随机数

            return Convert.ToInt32(((ra.NextDouble() * 999999999) + 1)).ToString();
        }
        #endregion

        #region 根据服务器控件生成html代码并返回
        /// <summary>
        /// 根据服务器控件生成html代码并返回
        /// </summary>
        /// <param name="ctl">服务端控件</param>
        public static string GetHtmlContentByControl(Control ctl)
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms);
            HtmlTextWriter tw = new HtmlTextWriter(sw);
            //取页面代码
            Page page = new Page();
            HtmlForm form = new HtmlForm();
            ctl.EnableViewState = false;
            page.EnableEventValidation = false;
            page.DesignerInitialize();
            page.Controls.Add(form);
            form.Controls.Add(ctl);
            page.RenderControl(tw);
            sw.Flush();
            string html = sw.Encoding.GetString(ms.ToArray());
            tw.Close();
            sw.Close();
            ms.Close();
            //取控件代码
            XDocument xdoc = XDocument.Parse(html.Replace("&nbsp;", " ").Replace("&", "&amp;"));
            html = "";
            int i = 0;
            xdoc.Elements("form").Elements().ToList().ForEach(h =>
            {
                if (i > 0)
                    html += h.ToString();
                i++;
            });
            return html;
        }
        #endregion

        #region 获得中文字符串的首字母
        /// <summary>
        /// 获得中文字符串的首字母
        /// </summary>
        /// <param name="str">中文字符串</param>
        /// <returns></returns>
        public static string GetInitialSpells(string str)
        {
            return ALSpell.GetSpell(str);
        }

        /// <summary>
        /// 用来获得一个字的拼音首字母
        /// </summary>
        /// <param name="cnChar">一个字</param>
        /// <returns></returns>
        public static string GetInitialSpell(string cnChar)
        {
            if (string.IsNullOrEmpty(cnChar))
                return cnChar;
            return ALSpell.GetSpell(cnChar[0]);
        }

        /// <summary>
        /// 获得姓名的缩写，例如zhangs
        /// </summary>
        /// <param name="cnChar"></param>
        /// <returns></returns>
        public static string GetNameSpells(string cnChar)
        {
            if (string.IsNullOrEmpty(cnChar))
                return cnChar;
            string output = "";
            for (int i = 0; i < cnChar.Length; i++)
                output += (i == 0 ? ALSpell.GetSpells(cnChar[i]).ToLower() : ALSpell.GetSpell(cnChar[i]).ToLower());
            return output;
        }
        #endregion

        #region 将集合扩展到指定长度
        /// <summary>
        /// 将集合扩展到指定长度
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<T> ExpandList<T>(List<T> list, int rowCount)
        {
            if (list == null)
                list = new List<T>();
            else if (list.Count > rowCount)
                return list.Take(rowCount).ToList();

            for (int i = list.Count; i < rowCount; i++)
                list.Add(ALCommon.DefaultOf<T>());

            return list;
        }
        #endregion

        #region 设置页面控件的显示和隐藏，isEdit 为 True 时，为编辑状态，文本框显示，Label隐藏，反之则反。
        /// <summary>
        /// 设置页面控件的显示和隐藏，isEdit 为 True 时，为编辑状态，文本框显示，Label隐藏，反之则反。
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="isEdit"></param>
        public static void SetVisible(ControlCollection controls, bool isEdit)
        {
            foreach (Control control in controls)
            {
                if (control is TextBox || control is CheckBox || control is CheckBoxList
                    || control is RadioButton || control is RadioButtonList
                    || control is DropDownList || control is LinkButton
                    || control is BaseValidator || control is ValidationSummary)
                    control.Visible = isEdit;
                else if (control is Label)
                    control.Visible = !isEdit;
                else if (control is HtmlContainerControl)
                    SetVisible(control.Controls, isEdit);
                else if (control is INamingContainer)
                    SetVisible(control.Controls, isEdit);
                else if (control is Panel)
                    SetVisible(control.Controls, isEdit);
            }
        }
        #endregion
    }
}
