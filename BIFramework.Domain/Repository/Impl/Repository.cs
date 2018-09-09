using BIStudio.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 仓储
    /// </summary>
    public class Repository<TEntity> : TransientDependency, IRepository<TEntity>
        where TEntity : class ,IEntity, new()
    {
        private readonly IRepository<TEntity> Instance = CFAspect.Resolve<IRepositoryProvider>().Instance<TEntity>();

        public Repository()
        {
            //等待仓储默认事件执行完毕之后，再执行自定义事件
            Instance.OnAdd += Instance_OnAdd;
            Instance.OnModify += Instance_OnModify;
            Instance.OnRemove += Instance_OnRemove;
            Instance.OnGet += Instance_OnGet;
        }

        #region 仓储事件

        /// <summary>
        /// 当准备新增实体时触发
        /// </summary>
        public event RepositoryAddHandler<TEntity> OnAdd;
        /// <summary>
        /// 当准备更新实体时触发
        /// </summary>
        public event RepositoryModifyHandler<TEntity> OnModify;
        /// <summary>
        /// 当准备删除实体时触发
        /// </summary>
        public event RepositoryRemoveHandler<TEntity> OnRemove;
        /// <summary>
        /// 当准备获取实体时触发
        /// </summary>
        public event RepositoryGetHandler<TEntity> OnGet;

        void Instance_OnAdd(ref TEntity entity)
        {
            if (this.OnAdd != null)
                this.OnAdd(ref entity);
        }
        void Instance_OnModify(ref TEntity entity, ref ISpecification<TEntity> specification)
        {
            if (this.OnModify != null)
                this.OnModify(ref entity, ref specification);
        }
        void Instance_OnRemove(ref ISpecification<TEntity> specification)
        {
            if (this.OnRemove != null)
                this.OnRemove(ref specification);
        }
        void Instance_OnGet(ref ISpecification<TEntity> specification)
        {
            if (this.OnGet != null)
                this.OnGet(ref specification);
        }

        #endregion

        #region 仓储接口

        public virtual bool Add(TEntity entity)
        {
            return Instance.Add(entity);
        }

        public bool Remove(TEntity entity)
        {
            return Instance.Remove(entity);
        }

        public bool Remove(long id)
        {
            return Instance.Remove(id);
        }

        public bool Remove(Expression<Func<TEntity, bool>> predicate)
        {
            return Instance.Remove(predicate);
        }
        public virtual bool Remove(ISpecification<TEntity> specification)
        {
            return Instance.Remove(specification);
        }

        public bool Modify(TEntity entity)
        {
            return Instance.Modify(entity);
        }

        public bool Modify(TEntity entity, Expression<Func<TEntity, bool>> predicate)
        {
            return Instance.Modify(entity, predicate);
        }
        public virtual bool Modify(TEntity entity, ISpecification<TEntity> specification)
        {
            return Instance.Modify(entity, specification);
        }

        public TEntity Get(long id)
        {
            return Instance.Get(id);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return Instance.Get(predicate);
        }
        public TEntity Get(ISpecification<TEntity> specification)
        {
            return Instance.Get(specification);
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, SortExpression<TEntity> sortExpression = null)
        {
            return Instance.GetAll(predicate, sortExpression);
        }
        public virtual IEnumerable<TEntity> GetAll(ISpecification<TEntity> specification = null, SortExpression<TEntity> sortExpression = null)
        {
            return Instance.GetAll(specification, sortExpression);
        }

        public PagedList<TEntity> GetPaged(Expression<Func<TEntity, bool>> predicate, int pageIndex = 1, int pageSize = 15, SortExpression<TEntity> sortExpression = null)
        {
            return Instance.GetPaged(predicate, pageIndex, pageSize, sortExpression);
        }
        public virtual PagedList<TEntity> GetPaged(ISpecification<TEntity> specification, int pageIndex = 1, int pageSize = 15, SortExpression<TEntity> sortExpression = null)
        {
            return Instance.GetPaged(specification, pageIndex, pageSize, sortExpression);
        }

        public GetPagedList<TEntity> GetPagedAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Instance.GetPagedAsync(predicate);
        }
        public virtual GetPagedList<TEntity> GetPagedAsync(ISpecification<TEntity> specification)
        {
            return Instance.GetPagedAsync(specification);
        }

        #endregion

        #region 查询接口

        public IQueryable<TEntity> Entities
        {
            get
            {
                return this.Instance.Entities;
            }
        }

        #endregion

        public override void DependOn(IBoundedContext uow)
        {
            base.DependOn(uow);
            Instance.DependOn(uow);
        }

    }
}
