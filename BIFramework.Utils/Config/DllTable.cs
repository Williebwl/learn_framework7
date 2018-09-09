using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;
using System.IO;
using System.Collections;
using System.Xml.Linq;


namespace BIStudio.Framework.Utils
{
    /// <summary>
    /// 管理配置文件条目
    /// </summary>
    public class DllTable : IEnumerable<DllItem>
    {
        private System.Configuration.Configuration config;

        internal DllTable(string configName)
        {
            //得到配置文件路径
            ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
            configFile.ExeConfigFilename = ALConfig.GetAbsolutePath(configName);
            //得到配置文件
            config = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);
        }

        /// <summary>
        /// 读取或设置配置文件条目
        /// </summary>
        /// <param name="configItemName"></param>
        /// <returns></returns>
        public DllItem this[string configItemName]
        {
            get
            {
                //得到配置节点
                return new DllItem(config,configItemName);
            }
            set
            {
                //得到配置节点
                value.Save();
            }
        }


        public bool IsExists(string configItemName)
        {
            ConfigurationSection section = config.Sections[configItemName];
            return section != null;
        }


        public IEnumerator<DllItem> GetEnumerator()
        {
            foreach (string sectionKey in config.Sections.Keys)
                yield return this[sectionKey];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

    }
}
