using BIStudio.Framework;
using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
    /// <typeparam name="TEntity"></typeparam>
    public abstract class RepositoryBase<TEntity> : TransientDependency, IRepository<TEntity>
        where TEntity : class ,IEntity, new()
    {
        #region 内部属性

        private readonly Lazy<DataEntityDefinition> _entityDefinition;

        public RepositoryBase()
        {
            this._entityDefinition = new Lazy<DataEntityDefinition>(() => DataEntityUtils.Entity(typeof(TEntity)));
            this.OnAdd += RepositoryBase_OnAdd;
            this.OnModify += RepositoryBase_OnModify;
            this.OnRemove += RepositoryBase_OnRemove;
            this.OnGet += RepositoryBase_OnGet;
        }


        #endregion

        #region 仓储事件

        public event RepositoryAddHandler<TEntity> OnAdd;
        public event RepositoryModifyHandler<TEntity> OnModify;
        public event RepositoryRemoveHandler<TEntity> OnRemove;
        public event RepositoryGetHandler<TEntity> OnGet;
                
        void RepositoryBase_OnAdd(ref TEntity entity)
        {
            var inputAudited = entity as IInputAudited;
            if (inputAudited != null)
            {
                inputAudited.InputTime = DateTime.Now;
                inputAudited.Inputer = CFContext.User.UserName;
                inputAudited.InputerID = CFContext.User.ID;
            }
            var deleteAudited = entity as ISoftDelete;
            if (deleteAudited != null)
            {
                deleteAudited.IsDelete = false;
            }
            var tenantAudited = entity as ITenantAudited;
            if (tenantAudited != null)
            {
                tenantAudited.SystemID = CFContext.User.SystemID;
            }
        }

        void RepositoryBase_OnModify(ref TEntity entity, ref ISpecification<TEntity> specification)
        {
            var updateAudited = entity as IUpdateAudited;
            if (updateAudited != null)
            {
                updateAudited.UpdateTime = DateTime.Now;
                updateAudited.Updater = CFContext.User.UserName;
                updateAudited.UpdaterID = CFContext.User.ID;
            }
            var tenantAudited = entity as ITenantAudited;
            if (tenantAudited != null)
            {
                tenantAudited.SystemID = CFContext.User.SystemID;
            }
        }

        void RepositoryBase_OnRemove(ref ISpecification<TEntity> specification)
        {
            if (typeof(ISoftDelete).IsAssignableFrom(_entityDefinition.Value.Type))
            {
                //执行逻辑删除
                var entity = new TEntity();
                if (entity is IDeleteAudited)
                {
                    var deleteAuditedEntity = (IDeleteAudited)entity;
                    deleteAuditedEntity.IsDelete = true;
                    deleteAuditedEntity.DeleteTime = DateTime.Now;
                    deleteAuditedEntity.Deleter = CFContext.User.UserName;
                    deleteAuditedEntity.DeleterID = CFContext.User.ID;
                }
                else
                {
                    var softDeleteEntity = (ISoftDelete)entity;
                    softDeleteEntity.IsDelete = true;
                }
                this.Modify(entity, specification);
                //取消物理删除
                specification = specification.And(new FalseSpec<TEntity>());
            }
            if (typeof(ITenantAudited).IsAssignableFrom(_entityDefinition.Value.Type))
            {
                specification = specification.And(new FieldSpec<TEntity, long>("SystemID", CFContext.User.SystemID));
            }
        }

        void RepositoryBase_OnGet(ref ISpecification<TEntity> specification)
        {
            if (typeof(ISoftDelete).IsAssignableFrom(_entityDefinition.Value.Type))
            {
                specification = specification.And(new FieldSpec<TEntity, bool>("IsDelete", false));
            }
            if (typeof(ITenantAudited).IsAssignableFrom(_entityDefinition.Value.Type))
            {
                specification = specification.And(new FieldSpec<TEntity, long>("SystemID", CFContext.User.SystemID));
            }
        }

        #endregion

        #region 仓储接口

        public virtual bool Add(TEntity entity)
        {
            if (OnAdd != null)
                OnAdd(ref entity);
            var fieldAndValues = GetFieldAndValue(entity, true);
            if (!fieldAndValues.ContainsKey(_entityDefinition.Value.TableAttribute.PrimaryKey))
            {
                entity.ID = CFID.NewID();
                fieldAndValues[_entityDefinition.Value.TableAttribute.PrimaryKey] = entity.ID;
            }
            var dbBuilder = DBBuilder.Insert(_entityDefinition.Value.TableAttribute.TableName, fieldAndValues);
            int result = this.UnitOfWork.Execute(dbBuilder);
            return result > 0;
        }

        public bool Remove(TEntity entity)
        {
            return Remove(new IDSpec<TEntity>(entity.ID ?? 0));
        }

        public bool Remove(long id)
        {
            return Remove(new IDSpec<TEntity>(id));
        }

        public bool Remove(Expression<Func<TEntity, bool>> predicate)
        {
            return Remove(new Spec<TEntity>(predicate));
        }
        public virtual bool Remove(ISpecification<TEntity> specification)
        {
            if (this.OnRemove != null)
                OnRemove(ref specification);
            DBBuilder condition;
            if (specification.Lambda != null)
            {
                var ids = this.Entities.Where(specification.Lambda).Select(d => d.ID).ToList();
                if (ids.Count > 0)
                    condition = DBBuilder.Define().Field(_entityDefinition.Value.TableAttribute.TableName, "ID").In(builder => builder.Value(ids.ToArray()));
                else
                    condition = new FalseSpec<TEntity>().Sql;
            }
            else
            {
                condition = specification.Sql;
            }
            var dbBuilder = condition.IsCommand ? condition : DBBuilder.Delete(_entityDefinition.Value.TableAttribute.TableName).Where(condition);
            var result = this.UnitOfWork.Execute(dbBuilder);
            return result > 0;
        }

        public bool Modify(TEntity entity)
        {
            return Modify(entity, new IDSpec<TEntity>(entity.ID ?? 0));
        }

        public bool Modify(TEntity entity, Expression<Func<TEntity, bool>> predicate)
        {
            return Modify(entity, new Spec<TEntity>(predicate));
        }
        public virtual bool Modify(TEntity entity, ISpecification<TEntity> specification)
        {
            if (this.OnModify != null)
                OnModify(ref entity, ref specification);
            DBBuilder condition;
            if (specification.Lambda != null)
            {
                var ids = this.Entities.Where(specification.Lambda).Select(d => d.ID).ToList();
                if (ids.Count > 0)
                    condition = DBBuilder.Define().Field(_entityDefinition.Value.TableAttribute.TableName, "ID").In(builder => builder.Value(ids.ToArray()));
                else
                    condition = new FalseSpec<TEntity>().Sql;
            }
            else
            {
                condition = specification.Sql;
            }
            var fieldAndValues = GetFieldAndValue(entity, true, false);
            var dbBuilder = condition.IsCommand ? condition : DBBuilder.Update(_entityDefinition.Value.TableAttribute.TableName, fieldAndValues).Where(condition);
            var result = this.UnitOfWork.Execute(dbBuilder);
            return result > 0;
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return Get(new Spec<TEntity>(predicate));
        }
        public TEntity Get(ISpecification<TEntity> specification)
        {
            return GetAll(specification).FirstOrDefault() ?? new TEntity();
        }

        public TEntity Get(long id)
        {
            return GetAll(new IDSpec<TEntity>(id)).FirstOrDefault() ?? new TEntity();
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null, SortExpression<TEntity> sortExpression = null)
        {
            return GetAll(new Spec<TEntity>(predicate), sortExpression);
        }
        public virtual IEnumerable<TEntity> GetAll(ISpecification<TEntity> specification = null, SortExpression<TEntity> sortExpression = null)
        {
            if (specification == null)
                specification = new TrueSpec<TEntity>();
            if (this.OnGet != null)
                OnGet(ref specification);
            if (specification.Lambda != null)
            {
                return (sortExpression == null ? this.Entities.Where(specification.Lambda) : sortExpression.Lambda(this.Entities.Where(specification.Lambda))).ToList();
            }
            else
            {
                var condition = specification.Sql;
                var dbBuilder = condition.IsSelect ? condition : DBBuilder.Select(_entityDefinition.Value.TableAttribute.TableName).Where(condition);
                return this.UnitOfWork.ToList<TEntity>(dbBuilder, sortExpression == null ? null : sortExpression.Sql());
            }
        }

        public PagedList<TEntity> GetPaged(Expression<Func<TEntity, bool>> predicate, int pageIndex = 1, int pageSize = 15, SortExpression<TEntity> sortExpression = null)
        {
            return GetPaged(new Spec<TEntity>(predicate), pageIndex, pageSize, sortExpression);
        }
        public virtual PagedList<TEntity> GetPaged(ISpecification<TEntity> specification, int pageIndex = 1, int pageSize = 15, SortExpression<TEntity> sortExpression = null)
        {
            if (specification == null)
                specification = new TrueSpec<TEntity>();
            if (sortExpression == null)
                sortExpression = new DefaultSortExpression<TEntity>();
            if (this.OnGet != null)
                OnGet(ref specification);
            if (specification.Lambda != null)
            {
                var count = this.Entities.Count(specification.Lambda);
                var result = sortExpression.Lambda(this.Entities.Where(specification.Lambda)).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                return new PagedList<TEntity>(pageIndex, pageSize, count, result);
            }
            else
            {
                var condition = specification.Sql;
                var dbBuilder = condition.IsSelect ? condition : DBBuilder.Select(_entityDefinition.Value.TableAttribute.TableName).Where(condition);
                return this.UnitOfWork.ToList<TEntity>(dbBuilder, pageIndex, pageSize, sortExpression.Sql());
            }
        }

        public GetPagedList<TEntity> GetPagedAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetPagedAsync(new Spec<TEntity>(predicate));
        }
        public GetPagedList<TEntity> GetPagedAsync(ISpecification<TEntity> specification)
        {
            return delegate(int pageIndex, int pageSize, out int count, string sortExpression)
            {
                var items = GetPaged(specification, pageIndex, pageSize, sortExpression == null ? null : new SortExpression<TEntity>(sortExpression));
                count = items.TotalItems;
                return items;
            };
        }

        protected IDictionary<string, object> GetFieldAndValue(TEntity entity, bool isContainPk = false, bool isContainNull = true)
        {
            IDataEntity info = entity as IDataEntity;
            if (info == null)
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                else
                    throw new TypeAccessException(entity.GetType() + " 不是合法的数据实体");
            }
            IDictionary<string, object> obj = new ExpandoObject();
            info.Property.ColumnAttributes
                .Where(item => item.Value.IsExtend == false)
                .Where(item => !(isContainPk && item.Key == info.Property.TableAttribute.PrimaryKey))
                .Where(item => isContainNull || info.Property[item.Key] != null || item.Value.IsDBNull)
                .ForEach(item => obj[item.Key] = info.Property[item.Key]);
            return obj;
        }
        
        #endregion

        #region 查询接口

        public abstract IQueryable<TEntity> Entities { get; }

        #endregion

    }
}
