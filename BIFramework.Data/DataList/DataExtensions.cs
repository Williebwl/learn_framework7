using BIStudio.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BIStudio.Framework.Data;

namespace BIStudio.Framework.Data
{
    /// <summary>
    /// 将DbDataReader或DataTable转换为实体
    /// </summary>
    public static class DataExtensions
    {
        #region DataTable

        /// <summary>
        /// 将DataRow转换为数据传输对象
        /// </summary>
        /// <typeparam name="T">数据传输对象类型</typeparam>
        /// <param name="dr">要转换的数据行</param>
        /// <returns></returns>
        public static T As<T>(this DataRow dr) where T : new()
        {
            if (typeof(IDataEntity).IsAssignableFrom(typeof(T)))
            {
                IDataEntity info = new T() as IDataEntity;
                foreach (var field in info.Property.ColumnAttributes)
                {
                    if (dr.Table.Columns.Contains(field.Key))
                    {
                        info.Property[field.Key] = dr[field.Key];
                        info.Property.ColumnAttributes[field.Key].IsDBNull = false;
                    }
                }
                return (T)(info as object);
            }
            else
                return dr.Map<DataRow, T>();
        }
        /// <summary>
        /// 将DataTable转换为数据传输对象列表
        /// </summary>
        /// <typeparam name="T">数据传输对象类型</typeparam>
        /// <param name="dt">要转换的数据表</param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt) where T : new()
        {
            List<T> list = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(dr.As<T>());
            }
            return list;
        }

        #endregion

        #region DbDataReader
        
        /// <summary>
        /// 将DataReader转换为数据传输对象
        /// </summary>
        /// <typeparam name="T">数据传输对象类型</typeparam>
        /// <param name="dr">要转换的数据流</param>
        /// <returns></returns>
        public static T As<T>(this DbDataReader dr) where T : new()
        {
            if (typeof(IDataEntity).IsAssignableFrom(typeof(T)))
            {
                IDataEntity info = new T() as IDataEntity;
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    string fieldName = dr.GetName(i);
                    object fieldValue = dr.GetValue(i);

                    if (info.Property.ColumnAttributes.ContainsKey(fieldName))
                        info.Property[fieldName] = fieldValue;
                }
                return (T)(info as object);
            }
            else
                return dr.Map<DbDataReader, T>();
        }
        /// <summary>
        /// 将DataReader转换为数据传输对象列表
        /// </summary>
        /// <typeparam name="T">数据传输对象类型</typeparam>
        /// <param name="dr">要转换的数据流</param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DbDataReader dr) where T : new()
        {
            List<T> list = new List<T>();
            using (dr)
            {
                while (dr.Read())
                {
                    list.Add(dr.As<T>());
                }
                dr.Close();
            }
            return list;
        }

        #endregion

        #region PagedList
        
        /// <summary>
        /// 获得分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="currentPage"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int? currentPage, int? itemsPerPage)
        {
            currentPage = currentPage ?? 1;
            itemsPerPage = itemsPerPage ?? 15;
            return new PagedList<T>(currentPage.Value, itemsPerPage.Value, source.Count(), source.Skip((currentPage.Value - 1) * itemsPerPage.Value).Take(itemsPerPage.Value).ToList());
        }
        /// <summary>
        /// 获得分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, PagedQuery query)
        {
            return ToPagedList<T>(source, query != null ? query.PageIndex : null, query != null ? query.PageSize : null);
        }

        #endregion
    }
}
