using BIStudio.Framework;
using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    public static class RepositoryUtils
    {
        /// <summary>
        /// 将DataRow转换为数据实体
        /// </summary>
        /// <typeparam name="T">数据实体类型</typeparam>
        /// <param name="dr">要转换的数据行</param>
        /// <param name="ev">需要应用的事务</param>
        /// <returns></returns>
        public static T As<T>(this DataRow dr, IUnitOfWork ev) where T : new()
        {
            if (typeof(IDataEntity).IsAssignableFrom(typeof(T)))
            {
                Entity info = Activator.CreateInstance(typeof(T), ev) as Entity;
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
        /// 将DataTable转换为数据实体列表
        /// </summary>
        /// <typeparam name="T">数据实体类型</typeparam>
        /// <param name="dt">要转换的数据表</param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt, IUnitOfWork ev) where T : new()
        {
            List<T> list = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(dr.As<T>(ev));
            }
            return list;
        }

        /// <summary>
        /// 将DataReader转换为数据实体
        /// </summary>
        /// <typeparam name="T">数据实体类型</typeparam>
        /// <param name="dr">要转换的数据流</param>
        /// <param name="ev">需要应用的事务</param>
        /// <returns></returns>
        public static T As<T>(this DbDataReader dr, IUnitOfWork ev) where T : new()
        {
            if (typeof(IDataEntity).IsAssignableFrom(typeof(T)))
            {
                Entity info = Activator.CreateInstance(typeof(T), ev) as Entity;
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
        /// 将DataReader转换为数据实体列表
        /// </summary>
        /// <typeparam name="T">数据实体类型</typeparam>
        /// <param name="dr">要转换的数据流</param>
        /// <param name="ev">需要应用的事务</param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DbDataReader dr, IUnitOfWork ev) where T : new()
        {
            List<T> list = new List<T>();
            using (dr)
            {
                while (dr.Read())
                {
                    list.Add(dr.As<T>());
                }
                dr.Dispose();
            }
            return list;
        }
        
    }
}
