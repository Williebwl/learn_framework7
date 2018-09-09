
using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIStudio.Framework.Tag
{
    /// <summary>
    /// 标签贴入日志
    /// </summary>
    [Table("SYSTagApply")]
    public class SYSTagApply : Entity, ITenantAudited
    {
        /// <summary>
        /// 标签编号
        /// </summary>
        public long? TagID { get; set; }
        /// <summary>
        /// 贴入对象
        /// </summary>
        public long? TargetID { get; set; }
        /// <summary>
        /// 贴入对象的主键值
        /// </summary>
        public long? TargetObjectID { get; set; }
        /// <summary>
        /// 单位ID
        /// </summary>
        public long? SystemID { get; set; }
    }

}
