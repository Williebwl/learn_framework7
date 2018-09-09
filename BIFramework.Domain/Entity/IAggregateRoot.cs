using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 表示聚合根
    /// </summary>
    public interface IAggregateRoot : IEntity
    {
        /// <summary>
        /// 分发领域消息
        /// </summary>
        /// <param name="domainEvent"></param>
        void ApplyEvent<T>(T domainEvent) where T : IDomainEvent;
    }
}
