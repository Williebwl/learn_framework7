using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

namespace BIStudio.Framework.Utils
{
    /// <summary>
    /// 配置类
    /// </summary>
    /// <remarks>
    /// [2012-03-11]
    /// </remarks>
    public static class ALConfig
    {
        private static AppSet appSettings = new AppSet();
        /// <summary>
        /// 获取AppSettings下的节点值
        /// </summary>
        public static AppSet AppSettings
        {
            get { return appSettings; }
        }
        
        private static ConnSet connectionStrings = new ConnSet();
        /// <summary>
        /// 获取ConnectionStrings下的节点值
        /// </summary>
        public static ConnSet ConnectionStrings
        {
            get { return connectionStrings; }
        }

        private static DllSet dllConfigs = new DllSet();
        /// <summary>
        /// 获取DLL配置文件
        /// </summary>
        public static DllSet DllConfigs
        {
            get { return dllConfigs; }
        }

        /// <summary>
        /// 获取DllConfig的相对路径
        /// </summary>
        /// <param name="dllConfigName"></param>
        /// <returns></returns>
        private static string GetRelativePath(string dllConfigName)
        {
            return "~/Config/" + dllConfigName + ".config";
        }

        /// <summary>
        /// 获取DllConfig的绝对路径
        /// </summary>
        /// <param name="dllConfigName"></param>
        /// <returns></returns>
        internal static string GetAbsolutePath(string dllConfigName)
        {
            if (HttpContext.Current != null)
                return CFContext.Server.MapPath(GetRelativePath(dllConfigName));
            else
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    GetRelativePath(dllConfigName).TrimStart("~/".ToCharArray()).Replace("/", "\\\\"));
        }
        
    }
}
