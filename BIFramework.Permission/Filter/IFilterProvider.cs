using BIStudio.Framework.Permission;
using BIStudio.Framework.Domain;

namespace BIStudio.Framework.Permission
{
    /// <summary>
    /// 申明过滤器
    /// </summary>
    public interface IFilterProvider
    {
        /// <summary>
        /// 使用预定配置初始化过滤器
        /// </summary>
        /// <param name="config"></param>
        void Init(SYSFilter config);

        /// <summary>
        /// 将过滤器编译成规约
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        ISpecification<TEntity> Compile<TEntity>() where TEntity : class;
    }
}
