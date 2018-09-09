using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace BIStudio.Framework.Utils
{
    /// <summary>
    /// 管理配置文件
    /// </summary>
    public class DllSet
    {
        internal DllSet()
        {
        }

        /// <summary>
        /// 读取或设置配置文件
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public DllTable this[string configName]
        {
            get
            {
                return new DllTable(configName);
            }
        }

        /// <summary>
        /// 读取当前DLL的配置文件
        /// </summary>
        public DllTable Current
        {
            get
            {
                return new DllTable(Assembly.GetCallingAssembly().GetName().Name);
            }
        }

        /// <summary>
        /// 检查DllConfig是否存在
        /// </summary>
        /// <param name="dllConfigName"></param>
        /// <returns></returns>
        public bool IsExists(string dllConfigName)
        {
            string path = ALConfig.GetAbsolutePath(dllConfigName);
            return File.Exists(path);
        }
    }
}
