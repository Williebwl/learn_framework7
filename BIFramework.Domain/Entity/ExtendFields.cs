using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 扩展属性
    /// </summary>
    /// <example> 在DataEntityBase中使用扩展字段
    /// <code>
    /// public string Remarks { get; set; }
    /// 
    /// public ExtendFields ExtendFields;
    /// 
    /// public DataEntityBase()
    /// {
    ///     ExtendFields = new ExtendFields(this, "Remarks");
    /// }
    /// </code>
    /// </example>
    public class ExtendFields
    {
        #region 内部属性
        private object entity = null;
        private string xmlFieldName = null;
        private string xmlFieldValue
        {
            get
            {
                object value = this.entity.GetType().GetProperty(this.xmlFieldName).GetValue(this.entity, null);
                return value == DBNull.Value ? null : (string)value;
            }
            set
            {
                this.entity.GetType().GetProperty(this.xmlFieldName).SetValue(this.entity, value, null);
            }
        }

        /// <summary>
        /// 使用指定的xml字段装载扩展属性
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="xmlFieldName"></param>
        public ExtendFields(object entity, string xmlFieldName)
        {
            this.entity = entity;
            this.xmlFieldName = xmlFieldName;
        }
        #endregion

        #region 得到扩展属性的值
        /// <summary>
        /// 获得扩展属性的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetValue(string key)
        {
            if (string.IsNullOrEmpty(this.xmlFieldValue))
                return null;

            XDocument doc = XDocument.Parse(this.xmlFieldValue);
            if (doc == null) return null;

            XElement ele = doc.Root.Element(key);
            if (ele == null) return null;

            return ele.Value;
        }
        /// <summary>
        /// 设置扩展属性的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void SetValue(string key, string value)
        {
            XDocument doc = string.IsNullOrEmpty(this.xmlFieldValue) ? new XDocument(new XElement("fields")) : XDocument.Parse(this.xmlFieldValue);

            XElement ele = doc.Root.Element(key);
            if (ele == null)
            {
                ele = new XElement(key);
                doc.Root.Add(ele);
            }
            ele.Value = value;
            this.xmlFieldValue = doc.ToString();
        }
        #endregion

        /// <summary>
        /// 设置或获取扩展属性的值
        /// </summary>
        /// <param name="fieldName">扩展字段名称</param>
        /// <returns>扩展字段的值</returns>
        public string this[string fieldName]
        {
            get
            {
                return this.GetValue(fieldName);
            }
            set
            {
                this.SetValue(fieldName, value);
            }
        }

        public override string ToString()
        {
            return this.xmlFieldValue;
        }

    }
}
