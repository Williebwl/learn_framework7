using System;

namespace BIStudio.Framework.Tag
{
    using Data;
    using Domain;

    /// <summary>
    /// 标签
    /// </summary>
    [Table("SYSTagClass")]
    public class SYSTagClass : Entity, IInputAudited, ITenantAudited
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
        /// 类型名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// code标示符
        /// </summary>
        public string ClassCode { get; set; }

        /// <summary>
        /// 标签类型
        /// </summary>
        public int? DisplayLevel { get; set; }

        /// <summary>
        /// 标签等级
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
        public string ClassValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Views { get; set; }
    }

}