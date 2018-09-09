using System;

using BIStudio.Framework.Domain;
using BIStudio.Framework.Data;


namespace BIStudio.Framework.Permission
{
    /// <summary>
    /// 权限操作
    /// </summary>
    [Table("SYSOperation")]
    public class SYSOperation : Entity
    {
        /// <summary>
        ///  服务提供商ID
        /// </summary>
        public long? SystemID { get; set; }
        /// <summary>
        ///  服务提供商ID
        /// </summary>
        public long? AppID { get; set; }
        /// <summary>
        /// 操作名称
        /// </summary>
        public string OperationName { get; set; }
        /// <summary>
        /// 操作代码
        /// </summary>
        [Column(IsExact = true)]
        public string OperationCode { get; set; }
        /// <summary>
        /// 所有者标志
        /// </summary>
        public string OperationFlag { get; set; }
        /// <summary>
        /// 所有者标志代码
        /// </summary>
        public long? OperationFlagID { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? Sequence { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
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
