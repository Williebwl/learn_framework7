using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Data
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute, ICloneable
    {
        /// <summary>
        /// 是否标识列(数据访问层暂时只考虑主键)
        /// </summary>
        public bool IsIdentity { get; set; }
        
        /// <summary>
        /// 是否忽略该字段的数据处理
        /// </summary>
        public bool IsExtend { get; set; }

        /// <summary>
        /// 是否从父级实体继承
        /// </summary>
        public bool IsInherit { get; set; }
        
        /// <summary>
        /// 是否设置为DBNull，只为了修改时,把相应字段置空
        /// </summary>
        public bool IsDBNull { get; set; }

        /// <summary>
        /// 是模糊查询还是精确查询
        /// </summary>
        public bool IsExact { get; set; }

        /// <summary>
        /// 字段的长度
        /// </summary>
        public int Leng { get; set; }

        /// <summary>
        /// 查询时指出查询日期类型时是否在同一年还是同一个季度……
        /// </summary>
        public DatePart DateTimePart { get; set; }

        /// <summary>
        /// 可以设置其它不方便设置的查询条件的表达式
        /// </summary>
        public string WhereCondition { get; set; }


        public ColumnAttribute Clone()
        {
            return new ColumnAttribute
            {
                IsIdentity = this.IsIdentity,
                IsExtend = this.IsExtend,
                IsInherit = this.IsInherit,
                IsDBNull = this.IsDBNull,
                IsExact = this.IsExact,
                Leng = this.Leng,
                DateTimePart = this.DateTimePart,
                WhereCondition = this.WhereCondition,
            };
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }

    /// <summary>
    /// 查询时指出查询日期类型时是否在同一年还是同一个季度……
    /// </summary>
    public enum DatePart
    {
        year, quarter, month, week, day, hour, minute, second
    }
}
