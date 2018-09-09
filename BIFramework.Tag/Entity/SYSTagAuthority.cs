
using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIStudio.Framework.Tag
{
    /// <summary>
    /// 标签权限
    /// </summary>
    [Table("SYSTagAuthority")]
    public class SYSTagAuthority : Entity, ITenantAudited
    {
        /// <summary>
        /// 单位ID
        /// </summary>
        public long? SystemID { get; set; }
        /// <summary>
        /// 被授权的目标(TagGroup,TagClass,Tag)
        /// </summary>
        public string ObjectType { get; set; }
        /// <summary>
        /// 被授权的目标编号
        /// </summary>
        public long? ObjectValue { get; set; }
        /// <summary>
        /// 被授权的目标
        /// </summary>
        public string ObjectText { get; set; }
        /// <summary>
        /// 权限所有者(User,Role,Dept)
        /// </summary>
        public string AuthorityType { get; set; }
        /// <summary>
        /// 权限所有者编号
        /// </summary>
        public long? AuthorityValue { get; set; }
        /// <summary>
        /// 权限所有者
        /// </summary>
        public string AuthorityText { get; set; }
        /// <summary>
        /// 允许的操作,EnumTagOperate
        /// </summary>
        public int? AcceptOperate { get; set; }
        /// <summary>
        /// 拒绝的操作,EnumTagOperate
        /// </summary>
        public int? RejectOperate { get; set; }
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
