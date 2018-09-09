using BIStudio.Framework.MQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 领域事件
    /// </summary>
    public abstract class DomainEvent : Event, IDomainEvent
    {
        /// <summary>
        /// 聚合根标识
        /// </summary>
        public long? AggregateRootID { get; set; }
        /// <summary>
        /// 聚合根类别
        /// </summary>
        public string AggregateRootType { get; set; }
    }
}
