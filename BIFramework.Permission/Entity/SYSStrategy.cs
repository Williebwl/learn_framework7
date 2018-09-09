using System;
using System.Collections.Generic;

using BIStudio.Framework.Domain;
using BIStudio.Framework.Utils;
using BIStudio.Framework;
using BIStudio.Framework.Data;


namespace BIStudio.Framework.Permission
{
    /// <summary>
    /// 策略
    /// </summary>
    [Table("SYSStrategy")]
    public class SYSStrategy : Entity
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
        /// 策略代码 必须的
        /// </summary>
        [Column(IsExact = true)]
        public string StrategyCode { get; set; }
        /// <summary>
        /// 策略名称
        /// </summary>
        public string StrategyName { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? Sequence { get; set; }
        /// <summary>
        /// 时间段
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 时间段
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// IP段 '000.000.000.000'
        /// </summary>
        public string StartIP { get; set; }
        /// <summary>
        /// IP段 '255.255.255.255'
        /// </summary>
        public string EndIP { get; set; }
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
