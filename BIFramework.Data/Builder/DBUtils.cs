using BIStudio.Framework;
using BIStudio.Framework.Data;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Data
{
    /// <summary>
    /// 数据库工具类
    /// </summary>
    public static class DBUtils
    {
        private static ConcurrentDictionary<string, Type> registeredDBAdapter = new ConcurrentDictionary<string, Type>();
        /// <summary>
        /// 获得指定数据库类型的数据适配器
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public static IDBAdapter GetDBAdapter(string providerName)
        {
            Type dbAdapter;
            if (registeredDBAdapter.TryGetValue(providerName, out dbAdapter))
                return (IDBAdapter)Activator.CreateInstance(dbAdapter);
            throw CFException.Create(OperateResult.NotFound, "尚未注册" + providerName + "类型的数据适配器。");
        }
        /// <summary>
        /// 注册指定数据库类型的数据适配器
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="dbAdapter"></param>
        public static void RegisterDBAdapter(string providerName, Type dbAdapter)
        {
            registeredDBAdapter.AddOrUpdate(providerName, dbAdapter, (key, value) => value = dbAdapter);
        }

        private static ConcurrentDictionary<string, Type> registeredDBAnalyzer = new ConcurrentDictionary<string, Type>();
        /// <summary>
        /// 获得指定数据库类型的数据分析器
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public static IDBAnalyzer GetDBAnalyzer(string providerName)
        {
            Type dbAnalyzer;
            if (registeredDBAnalyzer.TryGetValue(providerName, out dbAnalyzer))
                return (IDBAnalyzer)Activator.CreateInstance(dbAnalyzer);
            throw CFException.Create(OperateResult.NotFound, "尚未注册" + providerName + "类型的数据分析器。");
        }
        /// <summary>
        /// 注册指定数据库类型的数据分析器
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="dbAnalyzer"></param>
        public static void RegisterDBAnalyzer(string providerName, Type dbAnalyzer)
        {
            registeredDBAnalyzer.AddOrUpdate(providerName, dbAnalyzer, (key, value) => value = dbAnalyzer);
        }

        /// <summary>
        /// 判断指定的数据表是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool HasDataTable(string tableName)
        {
            IDBQuery query = CFAspect.Resolve<IDBQuery>();
            DBBuilder sql = DBBuilder.Select().Append(dbAdapter => "count(*) from " + dbAdapter.FormatFunction("gettables") + " where table_name=" + dbAdapter.FormatValue(tableName));
            return Convert.ToInt32(query.ToScalar(sql)) > 0;
        }

        /// <summary>
        /// 判断指定的数据字段是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static bool HasDataField(string tableName, string fieldName)
        {
            IDBQuery query = CFAspect.Resolve<IDBQuery>();
            DBBuilder sql = DBBuilder.Select().Append(dbAdapter => "count(*) from " + dbAdapter.FormatFunction("getcolumns", dbAdapter.FormatValue(tableName)) + " where column_name=" + dbAdapter.FormatValue(fieldName));
            return Convert.ToInt32(query.ToScalar(sql)) > 0;
        }
    }
}
