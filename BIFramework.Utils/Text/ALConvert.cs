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
    /// ת����
    /// </summary>
    /// <remarks>
    /// ���ڸ�������֮���ת��
    /// [2012-3-11]
    /// </remarks>
    public static partial class ALConvert
    {

        #region ת�����ַ���<=>List����
        
        /// <summary>
        /// ���ַ���ת���List��������
        /// </summary>
        /// <typeparam name="T">���ͣ�Ŀǰֻ֧��int,string</typeparam>
        /// <param name="str">Ҫת�����ַ���</param>
        /// <returns>List����</returns>
        public static List<T> ToList<T>(string str)
        {
            return ToList<T>(str, ',');
        }
        /// <summary>
        /// ���ַ���ת���List��������
        /// </summary>
        /// <typeparam name="T">���ͣ�Ŀǰֻ֧��int,string</typeparam>
        /// <param name="str">Ҫת�����ַ���</param>
        /// <param name="split">�ָ���</param>
        /// <returns>List����</returns>
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
        /// ���ַ���ת���List��������
        /// </summary>
        /// <param name="str">Ҫת�����ַ���</param>
        /// <param name="split">�ָ���</param>
        /// <returns>List����</returns>
        public static List<string> ToList(string str, char split)
        {
            return ToList<string>(str, split);
        }
        #endregion

        #region ת��������<=>�ַ���
        /// <summary>
        /// ��ָ�����ϰ��ա��ָ�����ƴ���ַ���
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst">List����</param>
        /// <returns>�ַ���</returns>
        public static string ToString<T>(IEnumerable<T> lst)
        {
            return ListToString<T>(lst, ',');
        }
        /// <summary>
        /// ��ָ�����ϰ��ա��ָ�����ƴ���ַ���
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst">List����</param>
        /// <param name="separator">�ָ���</param>
        /// <returns>�ַ���</returns>
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

        #region ת��������<=>�ֵ�
        /// <summary>
        /// ����ֵ�Լ���ƴ���ֵ�
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
        /// ����ֵ�Լ���ƴ���ֵ�
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
        /// <param name="deleterepeat">�Զ�ɾ���ظ���key</param>
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

        #region ת�������<=>�����(��д) / ����<=>����

        /// <summary>
        /// �����ת���ɴ�д�����
        /// </summary>
        /// <param name="money">���/����</param>
        public static string MoneyToRMB(double money)
        {
            if (money < 0)
                throw new ArgumentOutOfRangeException("����money����Ϊ��ֵ��");

            string s = money.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            s = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");

            return Regex.Replace(s, ".", delegate(Match m) { return "��Ԫ����Ҽ��������½��ƾ��տտտտտտշֽ�ʰ��Ǫ�f�|�׾������"[m.Value[0] - '-'].ToString(); });
        }

        /// <summary>
        /// ������ת��ΪСд���ĺ���
        /// </summary>
        /// <param name="num">����</param>
        /// <returns></returns>
        public static string NumberToChinese(double num, bool isLowerChinese)
        {
            string s = num.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");

            string r = "";
            if (isLowerChinese)
            {
                r = Regex.Replace(d, ".", delegate(Match m) { return "�� ����һ�����������߰˾ſտտտտտտշֽ�ʮ��ǧ�����׾������"[m.Value[0] - '-'].ToString(); });

                //�������ʮ�������ݣ��硰12����Ĭ�ϵõ��Ľ��Ϊ��һʮ����������õ�����ʮ������Ҫ���´���
                if (num >= 10 && num <= 19)
                {
                    r = r.Substring(1);
                }
            }
            else
                r = Regex.Replace(s, ".", delegate(Match m) { return "�� ����Ҽ��������½��ƾ��տտտտտտշֽ�ʰ��Ǫ�f�|�׾������"[m.Value[0] - '-'].ToString(); });
            
            return r;
        }

        /// <summary>
        /// ������ת��Ϊ��д���ĺ���
        /// </summary>
        /// <param name="num">����</param>
        /// <returns></returns>
        public static string NumberToChinese(double num)
        {
            return NumberToChinese(num, false);
        }
        #endregion

        #region ת����XML<=>DataSet
        /// <summary>
        /// ��xmlת����DataSet
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

                //��ȡ�ַ����е���Ϣ
                stream = new StringReader(xmlData);

                //��ȡstream�е�����
                reader = new XmlTextReader(stream);

                //DataSet��ȡXmlrdr�е�����
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

        #region ת����XML<=>DataTable
        /// <summary>
        /// ��xmlת����DataTable
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

        #region ת����Object<=>Int��Float��Decimal��Double��Datetime

        #region//��valueת��Ϊ�ɿ��������Ϊ���ַ�������null������ת�����󷵻�Null
        /// <summary>
        /// ��valueת��Ϊint?���Ϊ���ַ�������null������ת�����󷵻�Null
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
        /// ��valueת��Ϊlong?���Ϊ���ַ�������null������ת�����󷵻�Null
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
        /// ��valueת��Ϊfloat?���Ϊ���ַ�������null������ת�����󷵻�Null
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
        /// ��valueת��Ϊdecimal?���Ϊ���ַ�������null������ת�����󷵻�Null
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
        /// ��valueת��Ϊdouble?���Ϊ���ַ�������null������ת�����󷵻�Null
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
        /// ��valueת��ΪDateTime?���Ϊ���ַ�������null������ת�����󷵻�Null
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
        /// ��valueת��ΪGuid?���Ϊ���ַ�������null������ת�����󷵻�Null
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
        /// ��valueת��Ϊbool?���Ϊ���ַ�������null������ת�����󷵻�Null
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

        #region//��valueת��Ϊֵ�������ת�������򷵻�0

        /// <summary>
        /// ��valueת��Ϊint���Ϊnull��ת�������򷵻�0
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
        /// ��valueת��Ϊlong���Ϊnull��ת�������򷵻�0
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
        /// ��valueת��Ϊfloat���Ϊnull��ת�������򷵻�0
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
        /// ��valueת��Ϊdecimal���Ϊnull��ת�������򷵻�0
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
        /// ��valueת��Ϊdouble���Ϊnull��ת�������򷵻�0
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
        /// ��valueת��Ϊbool���Ϊnull��ת�������򷵻�False
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

        #region ת����List<T> => DataTable
        /// <summary>
        /// ��List<T> ת��Ϊ DataTable
        /// </summary>
        /// <typeparam name="T">������</typeparam>
        /// <param name="entitys">ʵ�弯</param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(List<T> entitys)
        {
            //���ʵ�弯�ϲ���Ϊ��
            if (entitys == null || entitys.Count < 1)
            {
                throw new Exception("��ת���ļ���Ϊ��");
            }
            //ȡ����һ��ʵ�������Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();
            //����DataTable��structure
            //���������У�Ӧ�����ɵ�DataTable�ṹCache�������˴���
            DataTable dt = new DataTable();
            for (int i = 0; i < entityProperties.Length; i++)
            {
                //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
                dt.Columns.Add(entityProperties[i].Name);
            }
            //������entity��ӵ�DataTable��
            foreach (object entity in entitys)
            {
                //������еĵ�ʵ�嶼Ϊͬһ����
                if (entity.GetType() != entityType)
                {
                    throw new Exception("Ҫת���ļ���Ԫ�����Ͳ�һ��");
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

        #region ����DateTime�õ���ʼʱ��ͽ���ʱ��

        /// <summary>
        /// ��DateTimeת��Ϊһ����ʼʱ�䣬��ʽΪ yyyy-MM-dd 00:00:00
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime ConvertToStartTime(this DateTime time)
        {
            return DateTime.Parse(time.ToString("yyyy-MM-dd") + " 00:00:00");
        }

        /// <summary>
        /// ��DateTimeת��Ϊһ������ʱ�䣬��ʽΪ yyyy-MM-dd 23:59:59
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
