using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using System.Collections;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace BIStudio.Framework.Utils
{
    /// <summary>
    /// 序列化类
    /// </summary>
    /// <remarks>
    /// 主要包括XML序列化，文件序列化，JSON序列化
    /// [2012-03-11]
    /// </remarks>
    public static class ALSerialize
    {

        #region 序列化：对象<=>xml字符串
        /// <summary>
        /// 对象序列化成xml字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string XMLSerialize<T>(T value)
        {
            if (value == null) return default(string);
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(T));
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                //ns.Add("", "");
                XmlTextWriter tw = new XmlTextWriter(stream, Encoding.UTF8);
                xmlSer.Serialize(tw, value, ns);
                tw.Flush();

                stream.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(stream);
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// xml字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T XMLDeserialize<T>(string value)
        {
            if (value == null) return default(T);
            try
            {
                XDocument xmlDoc = XDocument.Parse(value);
                XmlSerializer xmlSer = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlDoc.Root.Name.LocalName));
                return (T)xmlSer.Deserialize(xmlDoc.CreateReader());
            }
            catch
            {
                return default(T);
            }
        } 
        #endregion

        #region 序列化：对象<=>文件
        /// <summary>
        /// 对象序列化文件
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="t">对象</param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static void FileSerialize<T>(T t, string fileName)
        {
            MemoryStream ms = new MemoryStream();
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Encoding = new UTF8Encoding(false);
            settings.Indent = true;
            using (System.Xml.XmlWriter xw = System.Xml.XmlWriter.Create(ms, settings))
            {
                System.Xml.Serialization.XmlSerializer xz =
                    new System.Xml.Serialization.XmlSerializer(t.GetType());
                xz.Serialize(xw, t);
            }

            using (TextWriter tw = new StreamWriter(fileName))
            {
                tw.Write(Encoding.UTF8.GetString(ms.ToArray()));
                tw.Close();
            }
        }

        /// <summary>
        /// 文件反序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T FileDeserialize<T>(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                System.Xml.Serialization.XmlSerializer serializer =
                    new System.Xml.Serialization.XmlSerializer(typeof(T));
                T t = (T)serializer.Deserialize(fs);
                fs.Close();

                return t;
            }
        } 
        #endregion

        #region 序列化：对象<=>Json
        /// <summary>
        /// 对象序列化成Json
        /// </summary>
        public static string JsonSerialize<T>(T t)
        {
            List<Type> types = new List<Type>();
            Type currentType = typeof(T);
            Type exactType = t.GetType();
            if (currentType != exactType)
                types.Add(exactType);

            //DataContractJsonSerializer ser = new DataContractJsonSerializer(currentType, types);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(exactType);
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);

            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();

            //替换Json的Date字符串
            string p = @"\\/Date\((\d+)\+\d+\)\\/";

            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonToString);
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);

            return jsonString;
        }

        /// <summary>
        /// JSON反序列化成对象
        /// </summary>
        public static T JsonDeserialize<T>(string json)
        {
            return (T)JsonDeserialize(json, typeof(T));
        }
        
        /// <summary>
        /// JSON反序列化成对象
        /// </summary>
        public static object JsonDeserialize(string json, Type type)
        {
            //将"yyyy-MM-dd HH:mm:ss"格式的字符串转为"\/Date(1294499956278+0800)\/"格式
            string p = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";

            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateToJson);
            Regex reg = new Regex(p);
            json = reg.Replace(json, matchEvaluator);

            DataContractJsonSerializer ser = new DataContractJsonSerializer(type);
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ser.ReadObject(ms);
        }

        #region 私有方法
        /// <summary>
        /// 将Json时间转为时间字符串
        /// </summary>
        private static string ConvertJsonToString(Match m)
        {
            string result = string.Empty;

            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();

            result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }

        /// <summary>
        /// 将时间字符串转为Json时间
        /// </summary>
        private static string ConvertDateToJson(Match m)
        {
            string result = string.Empty;

            DateTime dt = DateTime.Parse(m.Groups[0].Value);
            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");

            result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
            return result;
        }
        #endregion
        #endregion

        #region 序列化:DataTable<=>Json
        /// <summary>
        /// 把DataTable转化成 JSON 字符串 
        /// </summary>
        public static string DataTabelSerialize(DataTable dt)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            ArrayList arrayList = new ArrayList();
            foreach (DataRow row in dt.Rows)
            {
                Dictionary<string, object> dicts = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    dicts.Add(col.ColumnName, row[col.ColumnName]);
                }
                arrayList.Add(dicts);
            }
            return jss.Serialize(arrayList);
        }

        /// <summary>
        /// 把JSON 字符串转化成DataTable
        /// </summary>
        public static DataTable DataTableDeserialize(string json)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            ArrayList arrayList = jss.Deserialize<ArrayList>(json);
            DataTable dt = new DataTable();
            if (arrayList.Count > 0)
            {
                foreach (Dictionary<string, object> drow in arrayList)
                {
                    if (dt.Columns.Count == 0)
                    {
                        foreach (string key in drow.Keys)
                            dt.Columns.Add(key, drow[key].GetType());
                    }

                    DataRow row = dt.NewRow();
                    foreach (string key in drow.Keys)
                        row[key] = drow[key]; 

                    dt.Rows.Add(row);
                }
            }
            return dt;
        }
        #endregion
    }
}
