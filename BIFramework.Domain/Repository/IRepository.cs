using BIStudio.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BIStudio.Framework.Domain
{
    public interface IRepository<TEntity> : ITransientDependency
        where TEntity : class, IEntity
    {
        /// <summary>
        /// 当准备新增实体时触发
        /// </summary>
        event RepositoryAddHandler<TEntity> OnAdd;
        /// <summary>
        /// 当准备更新实体时触发
        /// </summary>
        event RepositoryModifyHandler<TEntity> OnModify;
        /// <summary>
        /// 当准备删除实体时触发
        /// </summary>
        event RepositoryRemoveHandler<TEntity> OnRemove;
        /// <summary>
        /// 当准备获取实体时触发
        /// </summary>
        event RepositoryGetHandler<TEntity> OnGet;

        /// <summary>
        ///     新增实体
        /// </summary>
        /// <param name="entity"></param>
        bool Add(TEntity entity);

        /// <summary>
        ///     删除实体
        /// </summary>
        /// <param name="entity"></param>
        bool Remove(TEntity entity);

        /// <summary>
        ///     删除实体
        /// </summary>
        bool Remove(long id);

        /// <summary>
        ///     删除实体
        /// </summary>
        /// <param name="predicate"></param>
        bool Remove(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        ///     删除实体
        /// </summary>
        /// <param name="specification"></param>
        bool Remove(ISpecification<TEntity> specification);

        /// <summary>
        ///     更新实体
        /// </summary>
        /// <param name="entity"></param>
        bool Modify(TEntity entity);

        /// <summary>
        ///     批量更新更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="predicate"></param>
        bool Modify(TEntity entity, Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        ///     批量更新更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="specification"></param>
        bool Modify(TEntity entity, ISpecification<TEntity> specification);

        /// <summary>
        ///     获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Get(long id);


        /// <summary>
        ///    根据条件获取单个实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        ///    根据条件获取单个实体
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        TEntity Get(ISpecification<TEntity> specification);

        /// <summary>
        ///     根据条件获取实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="sortExpression"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, SortExpression<TEntity> sortExpression = null);
        /// <summary>
        ///     根据条件获取实体
        /// </summary>
        /// <param name="specification"></param>
        /// <param name="sortExpression"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll(ISpecification<TEntity> specification = null, SortExpression<TEntity> sortExpression = null);

        /// <summary>
        ///     获取分页实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortExpression"></param>
        /// <returns></returns>
        PagedList<TEntity> GetPaged(Expression<Func<TEntity, bool>> predicate, int pageIndex = 1, int pageSize = 15, SortExpression<TEntity> sortExpression = null);
        /// <summary>
        ///     获取分页实体
        /// </summary>
        /// <param name="specification"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortExpression"></param>
        /// <returns></returns>
        PagedList<TEntity> GetPaged(ISpecification<TEntity> specification, int pageIndex = 1, int pageSize = 15, SortExpression<TEntity> sortExpression = null);

        /// <summary>
        ///     获取分页实体的委托
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        GetPagedList<TEntity> GetPagedAsync(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        ///     获取分页实体的委托
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        GetPagedList<TEntity> GetPagedAsync(ISpecification<TEntity> specification);

        /// <summary>
        /// 查询仓储实体（不使用审计功能）
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> Entities { get; }
    }
}