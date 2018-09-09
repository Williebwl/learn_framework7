using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using BIStudio.Framework.MQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 聚合根
    /// </summary>
    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
        /// <summary>
        /// 分发领域消息
        /// </summary>
        /// <param name="domainEvent"></param>
        public void ApplyEvent<T>(T domainEvent) where T : IDomainEvent
        {
            domainEvent.AggregateRootID = this.ID;
            domainEvent.AggregateRootType = this.GetType().FullName;
            MessageService.Default.Dispatch(domainEvent);
        }

    }
}
