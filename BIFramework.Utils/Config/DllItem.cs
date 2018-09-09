using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Configuration;
using System.Collections;

namespace BIStudio.Framework.Utils
{
    /// <summary>
    /// 管理配置文件条目下的配置项
    /// </summary>
    public class DllItem : IEnumerable<XElement>
    {
        private System.Configuration.Configuration config;
        private string configItemName;
        private XElement root;

        internal DllItem(System.Configuration.Configuration config, string configItemName)
        {
            this.config = config;
            this.configItemName = configItemName;
            this.Refresh();
        }
        /// <summary>
        /// 读取或设置配置文件条目下的配置项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                XElement xNode = this.FirstOrDefault(d => d.Name == "add" && d.Attribute("key").Value == key);
                if (xNode == null)
                    return null;
                else
                    return xNode.Attribute("value").Value;
            }
            set
            {
                XElement xNode = this.FirstOrDefault(d => d.Name == "add" && d.Attribute("key").Value == key);
                if (xNode == null)
                    root.Add(new XElement("add", new XAttribute("key", key), new XAttribute("value", value)));
                else
                    xNode.Attribute("value").Value = value;
            }
        }
        /// <summary>
        /// 保存配置信息
        /// </summary>
        public void Save()
        {
            //得到配置节点
            ConfigurationSection section = config.GetSection(configItemName);
            XDocument xdoc = XDocument.Parse(section.SectionInformation.GetRawXml());
            //写入配置节点
            xdoc.Elements().First().ReplaceAll(this as IEnumerable<XElement>);

            section.SectionInformation.SetRawXml(xdoc.ToString());
            config.Save();
        }
        /// <summary>
        /// 重新读取配置信息
        /// </summary>
        public void Refresh()
        {
            ConfigurationSection section = config.GetSection(configItemName);
            XDocument xdoc = XDocument.Parse(section.SectionInformation.GetRawXml());
            this.root = xdoc.Elements().First();
        }
        /// <summary>
        /// 返回全部配置项的键值对
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> ToDictionary()
        {
            return root.Elements("add").ToDictionary(d => d.Attribute("key").Value, d => d.Attribute("value").Value);
        }

        public bool IsExists(string configItemName)
        {
            return root.Elements("add").FirstOrDefault(d => d.Attribute("key").Value == configItemName) != null;
        }

        #region IEnumerable<XElement> 成员

        public IEnumerator<XElement> GetEnumerator()
        {
            foreach (XElement xNode in root.Elements())
                yield return xNode;
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
