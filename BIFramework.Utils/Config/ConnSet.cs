using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BIStudio.Framework.Utils
{
    /// <summary>
    /// 管理连接字符串
    /// </summary>
    public class ConnSet
    {
        internal ConnSet()
        {
        }
        
        /// <summary>
        /// 读取连接字符串
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public string this[string connName]
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[connName].ConnectionString;
            }
        }

    }
}
