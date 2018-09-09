using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace BIStudio.Framework
{
    /// <summary>
    /// 全局应用设置
    /// </summary>
    public sealed class CFConfig
    {
        private static Assembly[] referencedAssemblies = null;
        private CFConfig()
        {
            Func<Assembly, bool> assemblyFilter = assembly =>
            {
                var fullName = assembly.FullName.ToLower();

                if (!fullName.EndsWith("publickeytoken=null") ||
                    fullName.StartsWith("mscorlib") ||
                    fullName.StartsWith("dapper") ||
                    fullName.StartsWith("emitmapper") ||
                    fullName.StartsWith("stackexchange") ||
                    fullName.StartsWith("system") ||
                    fullName.StartsWith("microsoft") ||
                    fullName.StartsWith("am.charts") ||
                    fullName.StartsWith("componentart") ||
                    fullName.StartsWith("syncfusion") ||
                    fullName.StartsWith("lumisoft") ||
                    fullName.StartsWith("newtonsoft") ||
                    fullName.StartsWith("mysql") ||
                    fullName.StartsWith("krystalware") ||
                    fullName.StartsWith("htmlagilitypack") ||
                    fullName.StartsWith("ionic") ||
                    fullName.StartsWith("icsharpcode") ||
                    fullName.StartsWith("ewebeditor") ||
                    fullName.StartsWith("app_code") ||
                    fullName.StartsWith("app_global"))
                    return false;
                return true;
            };

            var loadedAssemblies = AppDomain.CurrentDomain
                .GetAssemblies()
                .AsParallel()
                .Select(item =>
                {
                    try
                    {
                        return item.Location.ToLower();
                    }
                    catch (NotSupportedException ex)
                    {
                        return "";
                    }
                })
                .ToArray();


            //b/s
            if (!string.IsNullOrEmpty(AppDomain.CurrentDomain.SetupInformation.DynamicBase))
            {

                Directory.GetFiles(AppDomain.CurrentDomain.SetupInformation.DynamicBase, "*.dll", SearchOption.AllDirectories)
                    .AsParallel()
                    .Where(path => !loadedAssemblies.Contains(path.ToLower()))
                    .ForAll(item => Assembly.LoadFrom(item));
            }

            //c/s
            if (!string.IsNullOrEmpty(AppDomain.CurrentDomain.SetupInformation.ApplicationBase))
            {
                Directory.GetFiles(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "*.dll", SearchOption.AllDirectories)
               .AsParallel()
               .Where(path => !loadedAssemblies.Contains(path.ToLower()))
               .ForAll(item => Assembly.LoadFrom(item));

               
            }

            referencedAssemblies = AppDomain.CurrentDomain
                     .GetAssemblies()
                     .AsParallel()
                     .Where(assemblyFilter)
                     .ToArray();
        }

        /// <summary>
        /// 实例化应用程序配置器
        /// </summary>
        public static readonly CFConfig Default = new CFConfig();

        /// <summary>
        /// 扫描程序集
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public CFConfig ScanAssemblies(Action<Assembly> action)
        {
            foreach (Assembly assembly in referencedAssemblies)
            {
                try
                {
                    action(assembly);
                }
                catch
                {
                    continue;
                }
            }
            return this;
        }
        /// <summary>
        /// 扫描程序集中定义的类型
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public CFConfig ScanTypes(Action<TypeInfo> action)
        {
            foreach (Assembly assembly in referencedAssemblies)
            {
                try
                {
                    foreach (TypeInfo type in assembly.DefinedTypes)
                    {
                        try
                        {
                            action(type);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }
            return this;
        }

        /// <summary>
        /// 扫描程序集中定义的类型
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<TypeInfo> ParallelGetTypes(Func<TypeInfo, bool> predicate)
        {
            return referencedAssemblies.AsParallel().SelectMany(d => d.DefinedTypes).Where(predicate);
        }

        /// <summary>
        /// 扫描程序集中定义的类型
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public CFConfig ParallelScanTypes(Action<TypeInfo> action)
        {
            referencedAssemblies.AsParallel().SelectMany(d => d.DefinedTypes).ForAll(action);
            return this;
        }

        /// <summary>
        /// 使用并行扫描程序集中定义的属性
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public CFConfig ParallelScanAttributes<T>(Action<TypeInfo, T> action) where T : Attribute
        {
            var q = from d in referencedAssemblies
                    from b in d.DefinedTypes
                    let atr = b.GetCustomAttributes<T>(false)
                    where atr.Any()
                    from a in atr
                    select new { b, a };

            var qt = q.AsParallel().ToArray();

            foreach (var d in qt) { try { action(d.b, d.a); } catch { } }
            return this;
        }

        /// <summary>
        /// 扫描程序集中定义的属性
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public CFConfig ScanAttributes<T>(Action<TypeInfo, T> action) where T : Attribute
        {
            ScanTypes(type =>
            {
                foreach (T attribute in type.GetCustomAttributes(typeof(T), false))
                {
                    try
                    {
                        action(type, attribute);
                    }
                    catch
                    {
                        continue;
                    }
                }
            });
            return this;
        }
        /// <summary>
        /// 扫描类中定义的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="classType"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public CFConfig ScanField<T>(Type classType, Action<FieldInfo> action) where T : class
        {
            foreach (var field in classType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(field => (field.IsPrivate || field.IsFamily) && !field.IsStatic && !field.IsInitOnly && typeof(T).IsAssignableFrom(field.FieldType)))
            {
                action(field);
            }
            return this;
        }

        /// <summary>
        /// 默认数据库连接名称
        /// </summary>
        public const string DefaultConnectionName = "CnStr";
        /// <summary>
        /// 获得数据库连接字符串
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public static ConnectionStringSettings GetConnection(string connectionName = null)
        {
            if (!string.IsNullOrEmpty(connectionName) && ConfigurationManager.ConnectionStrings[connectionName] != null)
                return ConfigurationManager.ConnectionStrings[connectionName];
            else
                return ConfigurationManager.ConnectionStrings[DefaultConnectionName];
        }
        ///// <summary>
        ///// 数据库连接类型
        ///// </summary>
        //public class ProviderName
        //{
        //    public const string SqlServer = "System.Data.SqlClient";
        //    public const string MySql = "MySql.Data.MySqlClient";
        //    public const string Oracle = "Oracle.DataAccess.Client";
        //    public const string SQLite = "System.Data.SQLite";
        //}
    }
}
