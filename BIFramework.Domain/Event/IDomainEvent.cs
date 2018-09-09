using BIStudio.Framework.MQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 表示领域事件
    /// </summary>
    public interface IDomainEvent : IEvent
    {
        /// <summary>
        /// 聚合根标识
        /// </summary>
        long? AggregateRootID { get; set; }
        /// <summary>
        /// 聚合根类别
        /// </summary>
        string AggregateRootType { get; set; }
    }
}
