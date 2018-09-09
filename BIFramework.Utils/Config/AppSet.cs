using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BIStudio.Framework.Utils
{
    /// <summary>
    /// 管理AppSettings节点
    /// </summary>
    public class AppSet
    {
        internal AppSet()
        {
        }

        /// <summary>
        /// 获取AppSettings下的节点值
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public string this[string keyName]
        {
            get
            {
                return ConfigurationManager.AppSettings.Get(keyName);
            }
        }

    }
}
