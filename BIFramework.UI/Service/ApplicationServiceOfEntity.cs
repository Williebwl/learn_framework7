using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BIStudio.Framework.UI
{
    using BIStudio.Framework;
    using BIStudio.Framework.Data;
    using BIStudio.Framework.Domain;

    /// <summary>
    /// 应用服务
    /// </summary>
    /// <typeparam name="TEntity">数据对象</typeparam>
    /// <typeparam name="TQuery">查询对象</typeparam>
    /// <typeparam name="TViewModel">数据交互模型</typeparam>
    public abstract class ApplicationService<TViewModel, TQuery, TEntity> : ApplicationService<TViewModel, TQuery>
        where TViewModel : class, IViewModel, new()
        where TQuery : Query, new()
        where TEntity : class, IEntity, new()
    {
        #region 内部属性

        /// <summary>
        /// 数据操作对象
        /// </summary>
        protected IRepository<TEntity> _repository;
        /// <summary>
        /// 模糊查询字段
        /// </summary>
        protected readonly string[] searchFields;

        /// <summary>
        /// 需要查询的字段
        /// </summary>
        /// <param name="searchFields">字段名称</param>
        public ApplicationService(params string[] searchFields)
            : base()
        {
            this.searchFields = searchFields;
        }

        #endregion

        #region Query

        /// <summary>
        /// 获得视图模型查询表达式
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual ISpecification<TEntity> GetQueryParams<T>(T info) where T : TQuery
        {
            var spec = new TrueSpec<TEntity>();

            if (info == null) goto End;

            info.Key = (info.Key ?? string.Empty).Trim();

            if (!string.IsNullOrEmpty(info.Key))
                spec.Sql.Append("AND (" + string.Join("OR", searchFields.Select(d => d + " LIKE @KEY ").ToArray()) + ")", new { KEY = "%" + info.Key + "%" });

            End: return spec;
        }
        /// <summary>
        /// 获得视图模型查询表达式
        /// </summary>
        /// <param name="info"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        protected virtual ISpecification<TEntity> GetQueryParams<T>(T info, out int pageIndex, out int pageSize) where T : TQuery
        {
            pageIndex = 1; pageSize = 15;

            var spec = GetQueryParams(info);

            if (info == null) goto End;

            if (info is PagedQuery)
            {
                var qInfo = info as PagedQuery;
                pageIndex = qInfo.PageIndex.HasValue ? qInfo.PageIndex.Value : 1;
                pageSize = qInfo.PageSize.HasValue ? qInfo.PageSize.Value : 15;
            }

            End: return spec;
        }
        
        public override IEnumerable<TViewModel> GetAll([FromUri]TQuery info)
        {
            return _repository.GetAll(GetQueryParams(info)).Map<TEntity, TViewModel>().ToList();
        }

        public override PagedList<TViewModel> Get([FromUri]TQuery info)
        {
            int pageIndex, pageSize;

            return _repository.GetPaged(GetQueryParams(info, out pageIndex, out pageSize), pageIndex, pageSize)
                .Map<PagedList<TEntity>, PagedList<TViewModel>>();
        }

        public override TViewModel Get(long id)
        {
            return _repository.Get(id).Map<TEntity, TViewModel>();
        }

        #endregion 

        #region Command

        public override TViewModel Post([FromBody]TViewModel vm)
        {
            if (vm == null)
                return default(TViewModel);
            this.Validate(vm);
            var entity = vm.Map<TViewModel, TEntity>();
            _repository.Add(entity);
            return _repository.Get(entity.ID.Value).Map<TEntity, TViewModel>();
        }
        public override TViewModel Put(long id, [FromBody]TViewModel vm)
        {
            if (vm == null)
                return default(TViewModel);
            this.Validate(vm);
            var entity = vm.Map<TViewModel, TEntity>();
            entity.ID = id;
            _repository.Modify(entity);
            return _repository.Get(entity.ID.Value).Map<TEntity, TViewModel>();
        }
        public override bool Delete(string id)
        {
            return _repository.Remove(new IDSpec<TEntity>(id));
        }

        public override bool Sequence([FromBody]List<SortVM> vm)
        {
            if (vm == null)
                return false;
            var infos = vm.Map<List<SortVM>, List<TEntity>>();
            using (var dbContext = BoundedContext.Create())
            {
                var bo = dbContext.Resolve<Repository<TEntity>>();
                infos.ForEach(info => bo.Modify(info));
                dbContext.Commit();
            }
            return true;
        }

        #endregion
    }

}
