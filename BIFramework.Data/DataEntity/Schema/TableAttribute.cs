using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Data
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute :  Attribute, ICloneable
    {
        public TableAttribute()
        {
            this.TableName = "";
            this.PrimaryKey = "ID";
            this.PrimaryKeyType = PKIDType.Custom;
        }
        public TableAttribute(string tableName)
        {
            this.TableName = tableName;
            this.PrimaryKey = "ID";
            this.PrimaryKeyType = PKIDType.Custom;
        }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 主键类型
        /// </summary>
        public PKIDType PrimaryKeyType { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        /// 关联的主表
        /// </summary>
        public string AssociateTable { get; set; }

        /// <summary>
        /// 外键
        /// </summary>
        public string ForeignKey { get; set; }

        /// <summary>
        /// 是否强制添加一条新数据
        /// </summary>
        public bool IsForcein { get; set; }

        /// <summary>
        /// 是否自动保存日志
        /// </summary>
        public bool AutoLog { get; set; }

        public TableAttribute Clone()
        {
            return new TableAttribute
            {
                TableName = this.TableName,
                PrimaryKeyType = this.PrimaryKeyType,
                PrimaryKey = this.PrimaryKey,
                AssociateTable = this.AssociateTable,
                ForeignKey = this.ForeignKey,
                IsForcein = this.IsForcein,
                AutoLog = this.AutoLog,
            };
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }

    [Serializable]
    public enum Join
    {
        Inner = 0,
        Left = 1,
        Right = 2,
        SubQueryIn = 3,
        SubQueryNotIn = 4
    }

    /// <summary>
    /// MaxID 使用整型自动增加方式  GUID 全球唯一码主键  Custom自定义字符串 Identity 自增长
    /// </summary>
    public enum PKIDType
    {
        MaxID = 0,
        Identity = 1,
        Custom = 2,
        GUID = 3
    }  
}
