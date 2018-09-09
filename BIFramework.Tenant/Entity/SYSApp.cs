using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIStudio.Framework.Tenant
{
    using BIStudio.Framework.Data;
    using BIStudio.Framework.Domain;

    /// <summary>
    /// 标签组
    /// </summary>
    [Table("SYSApp")]
    public class SYSApp : Entity, IInputAudited, ITenantAudited
    {
        /// <summary>
        /// 系统标示
        /// </summary>
        public long? SystemID { get; set; }

        /// <summary>
        /// 标签组名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 标签组前缀
        /// </summary>
        public string AppCode { get; set; }

        /// <summary>
        /// 类目编号
        /// </summary>
        public long? AppTypeID { get; set; }

        /// <summary>
        /// 类目名称
        /// </summary>
        public string AppType { get; set; }

        /// <summary>
        /// 是否内置（1-是；0-否）
        /// </summary>
        public int? IsBuiltIn { get; set; }

        /// <summary>
        /// 是否有效（1-有效，0-停用）
        /// </summary>
        public int? IsValid { get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
        public int? Sequence { get; set; }

        /// <summary>
        /// 点击量
        /// </summary>
        public int? Views { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Inputer { get; set; }

        /// <summary>
        /// 创建人的ID
        /// </summary>
        public long? InputerID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? InputTime { get; set; }
    }

}
