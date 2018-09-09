using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 表示限界上下文
    /// </summary>
    public interface IBoundedContext : IDomainResolver, IDisposable
    {
        /// <summary>
        /// 设置或获取工作单元
        /// </summary>
        IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// 执行事务
        /// </summary>
        void Commit();

        /// <summary>
        /// 取消事务
        /// </summary>
        void Rollback();
    }
}
