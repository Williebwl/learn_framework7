using System;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Data;


namespace BIStudio.Framework.Permission
{
    /// <summary>
    /// 权限资源
    /// </summary>
    [Table("SYSFilter")]
    public class SYSFilter : Entity
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
        /// 筛选器名称
        /// </summary>
        public string FilterName { get; set; }
        /// <summary>
        /// 筛选器代码
        /// </summary>
        public string FilterCode { get; set; }
        /// <summary>
        /// 实体类型
        /// </summary>
        public string EntityType { get; set; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// 操作符
        /// </summary>
        public string FilterOperation { get; set; }
        /// <summary>
        /// 操作值
        /// </summary>
        public string FilterValue { get; set; }
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
