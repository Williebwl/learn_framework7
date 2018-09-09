using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIStudio.Framework.Tag
{
    public struct SYSTagConferAuthorityDTO
    {
        /// <summary>
        /// 授权的目标类型（用户，角色，部门）
        /// </summary>
        public EnumSYSTagAuthorityType TargetType { get; set; }
        /// <summary>
        /// 授权的目标
        /// </summary>
        public List<SYSTagAuthorityDTO> TargetContent { get; set; }
        /// <summary>
        /// 授权类型
        /// </summary>
        public EnumSYSTagOperate AuthorityOperate { get; set; }
        /// <summary>
        /// 授权范围
        /// </summary>
        public EnumSYSTagRange AuthorityRange { get; set; }
        /// <summary>
        /// 授予权限的标签
        /// </summary>
        public SYSTagNodeDTO TagNode { get; set; }
        /// <summary>
        /// 授予权限的标签类型（留空则授予所有类型）
        /// </summary>
        public EnumSYSTagDisplayLevel? TagDisplayLevel { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
