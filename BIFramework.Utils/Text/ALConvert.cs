using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Reflection;
namespace BIStudio.Framework.Utils
{
    /// <summary>
    /// 转换类
    /// </summary>
    /// <remarks>
    /// 用于各种类型之间的转换
    /// [2012-3-11]
    /// </remarks>
    public static partial class ALConvert
    {

        #region 转换：字符串<=>List集合
        
        /// <summary>
        /// 将字符串转变成List集合类型
        /// </summary>
        /// <typeparam name="T">类型，目前只支持int,string</typeparam>
        /// <param name="str">要转换的字符串</param>
        /// <returns>List集合</returns>
        public static List<T> ToList<T>(string str)
        {
            return ToList<T>(str, ',');
        }
        /// <summary>
        /// 将字符串转变成List集合类型
        /// </summary>
        /// <typeparam name="T">类型，目前只支持int,string</typeparam>
        /// <param name="str">要转换的字符串</param>
        /// <param name="split">分隔符</param>
        /// <returns>List集合</returns>
        public static List<T> ToList<T>(string str, char split)
        {
            if (!string.IsNullOrEmpty(str))
            {
                List<T> list = new List<T>();
                foreach (string item in str.Trim(split).Split(split))
                {
                    if (string.IsNullOrEmpty(item))
                        continue;

                    list.Add(ALCommon.ConvertTo<T>(item, ALCommon.DefaultOf<T>()));
                }
                return list;
            }
            else
            {
                return new List<T>();
            }
        }
        /// <summary>
        /// 将字符串转变成List集合类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="split">分隔符</param>
        /// <returns>List集合</returns>
        public static List<string> ToList(string str, char split)
        {
            return ToList<string>(str, split);
        }
        #endregion

        #region 转换：集合<=>字符串
        /// <summary>
        /// 将指定集合按照“分隔符”拼成字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst">List集合</param>
        /// <returns>字符串</returns>
        public static string ToString<T>(IEnumerable<T> lst)
        {
            return ListToString<T>(lst, ',');
        }
        /// <summary>
        /// 将指定集合按照“分隔符”拼成字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst">List集合</param>
        /// <param name="separator">分隔符</param>
        /// <returns>字符串</returns>
        public static string ListToString<T>(IEnumerable<T> lst, char separator)
        {
            if (lst == null || lst.Count() == 0)
                return string.Empty;

            List<T> list = new List<T>();
            string str = string.Empty;
            StringBuilder builder = new StringBuilder();
            foreach (T num in lst)
            {
                if (!list.Contains(num))
                {
                    list.Add(num);
                    builder.Append(separator + num.ToString());
                }
            }
            if (builder.ToString().Length > 0)
            {
                str = builder.ToString().Substring(1, builder.ToString().Length - 1);
            }
            return str;
        }
        #endregion

        #region 转换：集合<=>字典
        /// <summary>
        /// 将键值对集合拼成字典
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(IEnumerable<TKey> key, IEnumerable<TValue> value)
        {
            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            if (key != null && value != null)
            {
                for (int i = 0; i < Math.Min(key.Count(), value.Count()); i++)
                {
                    dict.Add(key.ElementAt(i), value.ElementAt(i));
                }
            }
            return dict;
        }
        /// <summary>
        /// 将键值对集合拼成字典
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="strKey"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(string strKey, string strValue)
        {
            IEnumerable<TKey> key = strKey.Trim(',').Split(',').Select(d => ALCommon.ConvertTo<TKey>(d, ALCommon.DefaultOf<TKey>()));
            IEnumerable<TValue> value = strValue.Trim(',').Split(',').Select(d => ALCommon.ConvertTo<TValue>(d, ALCommon.DefaultOf<TValue>()));
            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            if (key != null && value != null)
            {
                for (int i = 0; i < Math.Min(key.Count(), value.Count()); i++)
                {
                    dict.Add(key.ElementAt(i), value.ElementAt(i));
                }
            }
            return dict;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="strKey"></param>
        /// <param name="strValue"></param>
        /// <param name="deleterepeat">自动删除重复的key</param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(string strKey, string strValue, bool deleterepeat)
        {
            if (deleterepeat)
            {
                IEnumerable<TKey> key = strKey.Trim(',').Split(',').Select(d => ALCommon.ConvertTo<TKey>(d, ALCommon.DefaultOf<TKey>()));
                IEnumerable<TValue> value = strValue.Trim(',').Split(',').Select(d => ALCommon.ConvertTo<TValue>(d, ALCommon.DefaultOf<TValue>()));
                Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
                if (key != null && value != null)
                {
                    for (int i = 0; i < Math.Min(key.Count(), value.Count()); i++)
                    {
                        if (!dict.ContainsKey(key.ElementAt(i)))
                        {
                            dict.Add(key.ElementAt(i), value.ElementAt(i));
                        }
                    }
                }
                return dict;
            }
            else
            {
                return ToDictionary<TKey, TValue>(strKey, strValue);
            }
            
        }
        #endregion

        #region 转换：金额<=>人民币(大写) / 数字<=>汉字

        /// <summary>
        /// 将金额转换成大写人民币
        /// </summary>
        /// <param name="money">金额/数字</param>
        public static string MoneyToRMB(double money)
        {
            if (money < 0)
                throw new ArgumentOutOfRangeException("参数money不能为负值！");

            string s = money.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            s = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");

            return Regex.Replace(s, ".", delegate(Match m) { return "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟萬億兆京垓秭穰"[m.Value[0] - '-'].ToString(); });
        }

        /// <summary>
        /// 将数字转换为小写中文汉字
        /// </summary>
        /// <param name="num">数字</param>
        /// <returns></returns>
        public static string NumberToChinese(double num, bool isLowerChinese)
        {
            string s = num.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");

            string r = "";
            if (isLowerChinese)
            {
                r = Regex.Replace(d, ".", delegate(Match m) { return "负 空零一二三四五六七八九空空空空空空空分角十百千万亿兆京垓秭穰"[m.Value[0] - '-'].ToString(); });

                //针对类似十几的数据，如“12”，默认得到的结果为“一十二”，你想得到的是十二，需要如下处理
                if (num >= 10 && num <= 19)
                {
                    r = r.Substring(1);
                }
            }
            else
                r = Regex.Replace(s, ".", delegate(Match m) { return "负 空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟萬億兆京垓秭穰"[m.Value[0] - '-'].ToString(); });
            
            return r;
        }

        /// <summary>
        /// 将数字转换为大写中文汉字
        /// </summary>
        /// <param name="num">数字</param>
        /// <returns></returns>
        public static string NumberToChinese(double num)
        {
            return NumberToChinese(num, false);
        }
        #endregion

        #region 转换：XML<=>DataSet
        /// <summary>
        /// 将xml转换成DataSet
        /// </summary>
        /// <param name="xmlData">XML</param>
        public static DataSet XMLToDataSet(string xmlData)
        {
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                if (string.IsNullOrEmpty(xmlData)) return new DataSet();

                DataSet ds = new DataSet();

                //读取字符串中的信息
                stream = new StringReader(xmlData);

                //获取stream中的数据
                reader = new XmlTextReader(stream);

                //DataSet获取Xmlrdr中的数据
                ds.ReadXml(reader);
                
                return ds;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (stream != null)
                    stream.Close();
            }
        }
        #endregion

        #region 转换：XML<=>DataTable
        /// <summary>
        /// 将xml转换成DataTable
        /// </summary>
        /// <param name="xmlData">XML</param>
        public static DataTable XMLToDataTable(string xmlData)
        {
            DataSet ds = XMLToDataSet(xmlData);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }
        #endregion

        #region 转换：Object<=>Int、Float、Decimal、Double、Datetime

        #region//将value转换为可空类型如果为空字符串返回null，其它转换错误返回Null
        /// <summary>
        /// 将value转换为int?如果为空字符串返回null，其它转换错误返回Null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int? ToInt(object value)
        {
            int temp = 0;
            if (value == null || string.IsNullOrEmpty(value.ToString()) || !int.TryParse(value.ToString(), out temp))
                return null;
            return temp;
        }

        /// <summary>
        /// 将value转换为long?如果为空字符串返回null，其它转换错误返回Null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long? ToLong(object value)
        {
            long temp = 0;
            if (value == null || string.IsNullOrEmpty(value.ToString()) || !long.TryParse(value.ToString(), out temp))
                return null;
            return temp;
        }

        /// <summary>
        /// 将value转换为float?如果为空字符串返回null，其它转换错误返回Null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float? ToFloat(object value)
        {
            float temp = 0;
            if (value == null || string.IsNullOrEmpty(value.ToString()) || !float.TryParse(value.ToString(), out temp))
                return null;
            return temp;
        }

        /// <summary>
        /// 将value转换为decimal?如果为空字符串返回null，其它转换错误返回Null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal? ToDecimal(object value)
        {
            decimal temp = 0;
            if (value == null || string.IsNullOrEmpty(value.ToString()) || !decimal.TryParse(value.ToString(), out temp))
                return null;
            return temp;
        }

        /// <summary>
        /// 将value转换为double?如果为空字符串返回null，其它转换错误返回Null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double? ToDouble(object value)
        {
            double temp = 0;
            if (value == null || string.IsNullOrEmpty(value.ToString()) || !double.TryParse(value.ToString(), out temp))
                return null;
            return temp;
        }

        /// <summary>
        /// 将value转换为DateTime?如果为空字符串返回null，其它转换错误返回Null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(object value)
        {
            DateTime temp;
            if (value == null || string.IsNullOrEmpty(value.ToString()) || !DateTime.TryParse(value.ToString(), out temp))
                return null;
            return temp;
        }

        /// <summary>
        /// 将value转换为Guid?如果为空字符串返回null，其它转换错误返回Null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Guid? ToGuid(object value)
        {
            Guid? temp = null;
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return null;
            try
            {
                temp = new Guid(value.ToString());
            }
            catch
            {
            }
            return temp;
        }

        /// <summary>
        /// 将value转换为bool?如果为空字符串返回null，其它转换错误返回Null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool? ToBool(object value)
        {
            bool temp;
            if (value == null || string.IsNullOrEmpty(value.ToString()) || !bool.TryParse(value.ToString(), out temp))
                return null;
            return temp;
        }
        #endregion

        #region//将value转换为值类型如果转换错误则返回0

        /// <summary>
        /// 将value转换为int如果为null或转换错误则返回0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt0(object value)
        {
            int temp = 0;
            if (value != null)
                int.TryParse(value.ToString(), out temp);
            return temp;
        }
        /// <summary>
        /// 将value转换为long如果为null或转换错误则返回0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLong0(object value)
        {
            long temp = 0;
            if (value != null)
                long.TryParse(value.ToString(), out temp);
            return temp;
        }

        /// <summary>
        /// 将value转换为float如果为null或转换错误则返回0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToFloat0(object value)
        {
            float temp = 0;
            if (value != null)
                float.TryParse(value.ToString(), out temp);
            return temp;
        }

        /// <summary>
        /// 将value转换为decimal如果为null或转换错误则返回0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimal0(object value)
        {
            decimal temp = 0;
            if (value != null)
                decimal.TryParse(value.ToString(), out temp);
            return temp;
        }

        /// <summary>
        /// 将value转换为double如果为null或转换错误则返回0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble0(object value)
        {
            double temp = 0;
            if (value != null)
                double.TryParse(value.ToString(), out temp);
            return temp;
        }

        /// <summary>
        /// 将value转换为bool如果为null或转换错误则返回False
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBool0(object value)
        {
            if (value == null) return false;
            string str = value.ToString().ToLower();
            return str == "true" || str == "1";
        }
        #endregion 

        #endregion

        #region 转换：List<T> => DataTable
        /// <summary>
        /// 将List<T> 转化为 DataTable
        /// </summary>
        /// <typeparam name="T">对象泛型</typeparam>
        /// <param name="entitys">实体集</param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(List<T> entitys)
        {
            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                throw new Exception("需转换的集合为空");
            }
            //取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();
            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            DataTable dt = new DataTable();
            for (int i = 0; i < entityProperties.Length; i++)
            {
                //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
                dt.Columns.Add(entityProperties[i].Name);
            }
            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }
        #endregion

        #region 根据DateTime得到开始时间和结束时间

        /// <summary>
        /// 将DateTime转化为一个开始时间，格式为 yyyy-MM-dd 00:00:00
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime ConvertToStartTime(this DateTime time)
        {
            return DateTime.Parse(time.ToString("yyyy-MM-dd") + " 00:00:00");
        }

        /// <summary>
        /// 将DateTime转化为一个结束时间，格式为 yyyy-MM-dd 23:59:59
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime ConvertToEndTime(this DateTime time)
        {
            return DateTime.Parse(time.ToString("yyyy-MM-dd") + " 23:59:59");
        }
        
        #endregion

        

        
    }
}
