using System;

namespace BIStudio.Framework.Tag
{
    using Data;
    using Domain;

    /// <summary>
    /// 标签选项
    /// </summary>
    [Table("SYSTag")]
    public class SYSTag : Entity, IInputAudited, ITenantAudited
    {
        /// <summary>
        /// 
        /// </summary>
        public long? SystemID { get; set; }

        /// <summary>
        /// 标签组编号
        /// </summary>
        public long? AppID { get; set; }

        /// <summary>
        /// 标签类型名称
        /// </summary>
        public string TagClass { get; set; }

        /// <summary>
        /// 标签类型ID
        /// </summary>
        public long? TagClassID { get; set; }

        /// <summary>
        /// 标签名称
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// code标示符
        /// </summary>
        public string TagCode { get; set; }

        /// <summary>
        /// 标签值范围,例如(0~10000)
        /// </summary>
        public string TagValue { get; set; }

        /// <summary>
        /// 标签的全路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 当前所在层
        /// </summary>
        public int? Layer { get; set; }

        /// <summary>
        /// 多级分类标签时上级标签id，默认为0
        /// </summary>
        public long? ParentID { get; set; }

        /// <summary>
        /// 是否叶子，1表示是叶子，不是为0，只有没有子节点时才是叶子
        /// </summary>
        public int? IsLeaf { get; set; }

        /// <summary>
        /// 标签项等级
        /// </summary>
        public int? IsBuiltIn { get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
        public int? Sequence { get; set; }

        /// <summary>
        /// 说明
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

        /// <summary>
        /// 
        /// </summary>
        public string ComputedSequence { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Views { get; set; }
    }

}