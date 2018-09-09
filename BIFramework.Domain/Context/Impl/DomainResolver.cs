using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 领域依赖解析器
    /// </summary>
    public abstract class DomainResolver : ContextResolver, IDomainResolver
    {
        private ConcurrentDictionary<Type, dynamic> trackingRepository = new ConcurrentDictionary<Type, dynamic>();
        /// <summary>
        /// 从当前上下文中获取仓储
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity
        {            
            return trackingRepository.GetOrAdd(typeof(TEntity), type => this.Resolve<IRepository<TEntity>>());
        }
        /// <summary>
        /// 将实体标记为新增状态
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Add<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            return this.Repository<TEntity>().Add(entity);
        }
        /// <summary>
        /// 将实体标记为变更状态
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Modify<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            return this.Repository<TEntity>().Modify(entity);
        }
        /// <summary>
        /// 将实体标记为删除状态
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Remove<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            return this.Repository<TEntity>().Remove(entity);
        }
        /// <summary>
        /// 从当前上下文中获取实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity Get<TEntity>(long id) where TEntity : class, IEntity
        {
            return this.Repository<TEntity>().Get(id);
        }

    }
}
