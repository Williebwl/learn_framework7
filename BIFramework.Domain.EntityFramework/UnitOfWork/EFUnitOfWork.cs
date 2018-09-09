using BIStudio.Framework;
using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain.EntityFramework
{
    /// <summary>
    /// 工作单元
    /// </summary>
    [Ioc(typeof(IUnitOfWork))]
    public sealed class EFUnitOfWork : UnitOfWorkBase, IUnitOfWork
    {
        #region IUnitOfWork

        internal protected readonly DynamicContext dbContext;
        public EFUnitOfWork(UnitOfWorkOptions options = null)
            : base(options)
        {
            options = options ?? UnitOfWorkOptions.Default;
            dbContext = new DynamicContext(options.ConnectionName);
            if (options.IsTransactional)
                dbQuery.Begin(dbContext.Database.BeginTransaction(IsolationLevel.Serializable).UnderlyingTransaction);
        }

        /// <summary>
        /// 执行事务
        /// </summary>
        override public void Commit()
        {
            if (this.dbContext.Database.CurrentTransaction != null)
                this.dbContext.Database.CurrentTransaction.Commit();
        }

        /// <summary>
        /// 取消事务
        /// </summary>
        override public void Rollback()
        {
            if (this.dbContext.Database.CurrentTransaction != null)
                this.dbContext.Database.CurrentTransaction.Rollback();
        }

        override public void Dispose()
        {
            if (this.dbContext.Database.CurrentTransaction != null)
                this.dbContext.Dispose();
            this.dbQuery.Dispose();
        }

        #endregion

        //#region IUnitOfWork

        ///// <summary>
        ///// 获得指定实体类型的仓储
        ///// </summary>
        ///// <typeparam name="TEntity">实体类型</typeparam>
        ///// <returns></returns>
        //public override IRepository<TEntity> Repository<TEntity>()
        //{
        //    IRepository<TEntity> rp = CFAspect.Repository<TEntity>() ?? CFAspect.Resolve<IRepositoryProvider>().Instance<TEntity>();
        //    this.Inject(rp);
        //    return rp;
        //}

        //#endregion
    }
}
