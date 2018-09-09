using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 表示准备新增实体事件
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    public delegate void RepositoryAddHandler<TEntity>(ref TEntity entity) where TEntity : class, IEntity;
    /// <summary>
    /// 表示准备更新实体事件
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <param name="specification"></param>
    public delegate void RepositoryModifyHandler<TEntity>(ref TEntity entity, ref ISpecification<TEntity> specification) where TEntity : class ,IEntity;
    /// <summary>
    /// 表示准备删除实体事件
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="specification"></param>
    public delegate void RepositoryRemoveHandler<TEntity>(ref ISpecification<TEntity> specification) where TEntity : class ,IEntity;
    /// <summary>
    /// 表示准备获取实体事件
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="specification"></param>
    public delegate void RepositoryGetHandler<TEntity>(ref ISpecification<TEntity> specification) where TEntity : class ,IEntity;
}
