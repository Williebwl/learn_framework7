using System;

namespace BIStudio.Framework.Tag
{
    using Data;
    using Domain;

    /// <summary>
    /// 
    /// </summary>
    [Table("vw_SYSTagGroup")]
    public class SYSTagGroup : Entity, IInputAudited, ITenantAudited
    {
        /// <summary>
        /// 
        /// </summary>
        public long? SystemID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Sequence { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Views { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Inputer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? InputerID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? InputTime { get; set; }
    }

}
