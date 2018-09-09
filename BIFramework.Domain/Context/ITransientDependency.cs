using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 表示瞬态依赖
    /// </summary>
    public interface ITransientDependency
    {
        /// <summary>
        /// 将工作单元注入到实现了ITransientDependency接口的私有属性
        /// </summary>
        void OnInject();

        /// <summary>
        /// 设置当前类依赖的工作单元
        /// </summary>
        /// <param name="uow"></param>
        void DependOn(IBoundedContext uow);

        /// <summary>
        /// 工作单元
        /// </summary>
        IUnitOfWork UnitOfWork { get; }
    }
}
