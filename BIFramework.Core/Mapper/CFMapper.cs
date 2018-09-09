using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using EmitMapper;
using EmitMapper.MappingConfiguration;

namespace BIStudio.Framework
{
    /// <summary>
    /// 类型转换
    /// </summary>
    public abstract class CFMapper
    {
        private static Dictionary<CFMapperConfig, Delegate> dictMap = new Dictionary<CFMapperConfig, Delegate>();
        private static object lockHelper = new object();
        /// <summary>
        /// 注册对象类型转换器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="fn"></param>
        public static void CreateMap<T, TResult>(MapTo<T, TResult> fn)
            where TResult : new()
        {
            var config = new MapperConfig<T, TResult>();
            lock (lockHelper)
            {
                if (!dictMap.ContainsKey(config))
                    dictMap.Add(config, fn);
                else
                    dictMap[config] = fn;
            }
        }
        
        /// <summary>
        /// 将对象转换为制定的类型
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static TTarget Map<TSource, TTarget>(TSource entity) where TTarget : new()
        {
            var config = new MapperConfig<TSource, TTarget>();
            if (dictMap.ContainsKey(config))
                return ((Func<TSource, TTarget>)dictMap[config])(entity);
            foreach (var kv in dictMap.Where(d => d.Key.SourceType == config.SourceType && config.TargetType.IsAssignableFrom(d.Key.TargetType)))
                return ((Func<TSource, TTarget>)kv.Value)(entity);
            return DynamicMap<TSource, TTarget>(entity);
        }

        /// <summary>
        ///  用source 更新target
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="entity"></param>
        /// <param name="target"></param>
        /// <param name="ignoreMembers"></param>
        /// <returns></returns>
        public static TTarget Map<TSource, TTarget>(TSource entity, TTarget target, params string[] ignoreMembers) where TTarget : new()
        {
            var config = new MapperConfig<TSource, TTarget>();
            if (dictMap.ContainsKey(config))
                return ((Func<TSource, TTarget>)dictMap[config])(entity);
            foreach (var kv in dictMap.Where(d => d.Key.SourceType == config.SourceType && config.TargetType.IsAssignableFrom(d.Key.TargetType)))
                return ((Func<TSource, TTarget>)kv.Value)(entity);
            return DynamicMap(entity, target);
        }

        #region 内部实现

        /// <summary>
        /// 获得默认的映射器
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="entity"></param>
        /// <param name="target"></param>
        /// <param name="ignoreMembers"></param>
        /// <returns></returns>
        protected static TTarget DynamicMap<TSource, TTarget>(TSource entity, TTarget target, params string[] ignoreMembers) where TTarget : new()
        {
            if (entity is DataRow)
                return DataRowMapper(entity as DataRow, target, ignoreMembers);
            if (typeof(TSource) == typeof(DbDataReader))
                return DataReaderMapper(entity as DbDataReader, target, ignoreMembers);
            return ObjectMapper(entity, target, ignoreMembers);
        }

        /// <summary>
        /// 获得默认的映射器
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected static TTarget DynamicMap<TSource, TTarget>(TSource entity) where TTarget : new()
        {
            if (entity is DataRow)
                return DataRowMapper<TTarget>(entity as DataRow);
            else if (typeof(TSource) == typeof(DbDataReader))
                return DataReaderMapper<TTarget>(entity as DbDataReader);
            else
                return ObjectMapper<TSource, TTarget>(entity);
        }

        /// <summary>
        /// 将DataRow映射到对象
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <returns></returns>
        protected static TTarget DataRowMapper<TTarget>(DataRow dr, TTarget target, params string[] ignoreMembers) where TTarget : new()
        {
            var ignores = ignoreMembers == null ? new List<string>() : ignoreMembers.ToList();
            var infos = typeof(TTarget).GetProperties().Where(item => !ignores.Contains(item.Name)).ToList();
            foreach (DataColumn dc in dr.Table.Columns)
            {
                var info = infos.FirstOrDefault(d => d.Name == dc.ColumnName);
                if (info != null)
                {
                    if (info.PropertyType.FullName != "System.String" && !info.PropertyType.IsValueType)
                        continue;
                    info.SetValue(target, dr[dc.ColumnName] == DBNull.Value ? null : dr[dc.ColumnName], null);
                }
            }
            return target;

        }

        /// <summary>
        /// 将DataRow映射到对象
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <returns></returns>
        protected static TTarget DataRowMapper<TTarget>(DataRow dr) where TTarget : new()
        {

            var target = new TTarget();
            PropertyInfo[] infos = typeof(TTarget).GetProperties();

            foreach (DataColumn dc in dr.Table.Columns)
            {
                PropertyInfo info = infos.FirstOrDefault(d => d.Name == dc.ColumnName);
                if (info != null)
                {
                    if (info.PropertyType.FullName != "System.String" && !info.PropertyType.IsValueType)
                        continue;
                    info.SetValue(target, dr[dc.ColumnName] == DBNull.Value ? null : dr[dc.ColumnName], null);
                }
            }
            return target;

        }

        /// <summary>
        /// 将DataRow映射到对象
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <returns></returns>
        protected static TTarget DataReaderMapper<TTarget>(DbDataReader dr, TTarget target, params string[] ignoreMembers) where TTarget : new()
        {
            var ignores = ignoreMembers == null ? new List<string>() : ignoreMembers.ToList();
            var infos = typeof(TTarget).GetProperties().Where(item => !ignores.Contains(item.Name)).ToList();
            for (int i = 0; i < dr.FieldCount; i++)
            {
                string fieldName = dr.GetName(i);
                object fieldValue = dr.GetValue(i);
                var info = infos.FirstOrDefault(d => d.Name == fieldName);
                if (info == null) continue;
                if (info.PropertyType.FullName != "System.String" && !info.PropertyType.IsValueType)
                    continue;
                info.SetValue(target, fieldValue == DBNull.Value ? null : fieldValue, null);
            }
            return target;
        }

        /// <summary>
        /// 将DataRow映射到对象
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <returns></returns>
        protected static TTarget DataReaderMapper<TTarget>(DbDataReader dr) where TTarget : new()
        {
            var newT = Activator.CreateInstance<TTarget>();
            PropertyInfo[] infos = typeof(TTarget).GetProperties();

            for (int i = 0; i < dr.FieldCount; i++)
            {
                string fieldName = dr.GetName(i);
                object fieldValue = dr.GetValue(i);
                PropertyInfo info = infos.FirstOrDefault(d => d.Name == fieldName);
                if (info != null)
                {
                    if (info.PropertyType.FullName != "System.String" && !info.PropertyType.IsValueType)
                        continue;

                    info.SetValue(newT, fieldValue == DBNull.Value ? null : fieldValue, null);
                }
            }
            return newT;
        }

        /// <summary>
        /// 将对象映射到对象
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <returns></returns>
        protected static TTarget ObjectMapper<TSource, TTarget>(TSource source) where TTarget : new()
        {
            var mapper = ObjectMapperManager.DefaultInstance.GetMapper<TSource, TTarget>(new DefaultMapConfig());
            return mapper.Map(source);
        }

        /// <summary>
        ///  用source 更新target
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="ignoreMembers"> 忽略的属性</param>
        /// <returns></returns>
        protected static TTarget ObjectMapper<TSource, TTarget>(TSource source, TTarget target, params string[] ignoreMembers)
        {
            var configurator = ignoreMembers == null ? new DefaultMapConfig() : new DefaultMapConfig().IgnoreMembers<TSource, TTarget>(ignoreMembers);
            var mapper = ObjectMapperManager.DefaultInstance.GetMapper<TSource, TTarget>(configurator);
            return mapper.Map(source, target);
        }

        #endregion
    }

    /// <summary>
    /// 将指定的数据类型转换为其他数据类型
    /// </summary>
    /// <typeparam name="T">原始数据类型</typeparam>
    /// <typeparam name="TResult">目标数据类型</typeparam>
    /// <param name="source">原始数据</param>
    /// <param name="target">目标数据</param>
    /// <returns></returns>
    public delegate TResult MapTo<T, TResult>(T source, TResult target);
}
