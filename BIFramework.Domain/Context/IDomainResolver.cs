namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 表示领域依赖解析器
    /// </summary>
    public interface IDomainResolver : IContextResolver
    {
        /// <summary>
        /// 从当前上下文中获取仓储
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity;
        /// <summary>
        /// 将实体标记为新增状态
        /// </summary>
        /// <param name="entity"></param>
        bool Add<TEntity>(TEntity entity) where TEntity : class, IEntity;
        /// <summary>
        /// 将实体标记为变更状态
        /// </summary>
        /// <param name="entity"></param>
        bool Modify<TEntity>(TEntity entity) where TEntity : class, IEntity;
        /// <summary>
        /// 将实体标记为删除状态
        /// </summary>
        /// <param name="entity"></param>
        bool Remove<TEntity>(TEntity entity) where TEntity : class, IEntity;
        /// <summary>
        /// 从当前上下文中获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Get<TEntity>(long id) where TEntity : class, IEntity;
    }
}