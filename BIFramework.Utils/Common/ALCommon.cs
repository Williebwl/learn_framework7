using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace BIStudio.Framework.Utils
{
    /// <summary>
    /// 基于工具类的工具类
    /// </summary>
    /// <remarks>
    /// [2012-03-11]
    /// </remarks>
    public static class ALCommon
    {
        #region 将对象转换为目标类型
        /// <summary>
        /// 将对象转换为目标类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        internal static T ConvertTo<T>(object src, T def)
        {
            Type type = typeof(T);
            T result;

            //如果是Nullable<T>类型,并且值为空
            if (type.Name == "Nullable`1" && type.IsValueType)
            {
                if (src == null || src.ToString() == string.Empty)
                    return default(T);
                else
                    type = type.GetGenericArguments()[0];
            }

            try
            {
                if (type == typeof(Guid))
                    result = (T)(object)new Guid(src.ToString());
                else if (type == typeof(Version))
                    result = (T)(object)new Version(src.ToString());
                else
                    result = (T)Convert.ChangeType(src, type);
            }
            catch
            {
                result = def;
            }
            return result;
        }

        /// <summary>
        /// 将对象转换为目标类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public  static object ConvertTo(object src, Type type)
        {
            object result;
            //如果是Nullable<T>类型,并且值为空
            if (type.Name == "Nullable`1" && type.IsValueType)
            {
                if (src == null || src.ToString() == string.Empty)
                    return null;
                type = type.GetGenericArguments()[0];
            }
            try
            {
                if (type == typeof(Guid))
                    result = new Guid(src.ToString());
                else if (type == typeof(Version))
                    result = new Version(src.ToString());
                else if (type == typeof(DateTime))
                    result =DateTime.Parse(src.ToString());
                else
                    result = Convert.ChangeType(src, type);
            }
            catch
            {
                result = Default(type);
            }
            return result;
        }
        /// <summary>
        /// 返回对应类型的默认值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static object Default(Type type)
        {
            if (!type.IsValueType||type == typeof(Nullable<>))
                return null;
            if (type == typeof(Guid))
                return new Guid();
            if (type == typeof(Version))
                return new Version();
            if (type == typeof(DateTime))
                return DateTime.MinValue;
            if (type == typeof(bool))
                return false;
            return 0;

        }
        #endregion

        #region 获取指定类型的非空默认值
        /// <summary>
        /// 获取指定类型的非空默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static T DefaultOf<T>()
        {
            Type type = typeof(T);

            if (type.FullName == "System.String")
                return (T)(string.Empty as Object);
            else if (type.IsValueType)
                return default(T);
            else
                return Activator.CreateInstance<T>();
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
        internal static List<T> ExpandList<T>(List<T> list, int rowCount)
        {
            if (list == null)
                list = new List<T>();

            for (int i = list.Count; i < rowCount; i++)
                list.Add(DefaultOf<T>());

            return list;
        }
        #endregion
    }
}
