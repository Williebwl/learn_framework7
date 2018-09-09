
using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIStudio.Framework.Tag
{
    /// <summary>
    /// 标签贴入对象
    /// </summary>
    [Table("SYSTagTarget")]
    public class SYSTagTarget : Entity, ITenantAudited
    {
        /// <summary>
        /// 单位ID
        /// </summary>
        public long? SystemID { get; set; }
        /// <summary>
        /// 贴入对象名称
        /// </summary>
        public string TargetName { get; set; }
        /// <summary>
        /// 对象标识，通常是数据库表名
        /// </summary>
        public string TargetCode { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? Sequence { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

}
