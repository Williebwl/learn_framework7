using BIStudio.Framework;
using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        #region IDBQuery

        internal protected readonly IDBQuery dbQuery;
        public UnitOfWorkBase(UnitOfWorkOptions options)
        {
            dbQuery = CFAspect.Resolve<IDBQuery>(options.ConnectionName);
        }

        abstract public void Commit();

        abstract public void Rollback();

        void IDBQuery.Begin(IDbTransaction transaction)
        {
            throw new InvalidOperationException("不允许在工作单元中显式开启事务");
        }

        void IDBQuery.Begin(string connectionName = null)
        {
            throw new InvalidOperationException("不允许在工作单元中显式开启事务");
        }

        abstract public void Dispose();

        public DataTable ToDataTable(DBBuilder dbBuilder)
        {
            return this.dbQuery.ToDataTable(dbBuilder);
        }
        public PagedList ToDataTable(DBBuilder dbBuilder, int pageIndex = 1, int pageSize = 15, string sortExpression = null)
        {
            return this.dbQuery.ToDataTable(dbBuilder, pageIndex, pageSize, sortExpression);
        }
        public GetPagedList ToDataTableAsync(DBBuilder dbBuilder)
        {
            return this.dbQuery.ToDataTableAsync(dbBuilder);
        }

        public List<T> ToList<T>(DBBuilder dbBuilder) where T : new()
        {
            return this.dbQuery.ToList<T>(dbBuilder);
        }
        public List<T> ToList<T>(DBBuilder dbBuilder, string sortExpression = null) where T : new()
        {
            return this.dbQuery.ToList<T>(dbBuilder, sortExpression);
        }
        public PagedList<T> ToList<T>(DBBuilder dbBuilder, int pageIndex = 1, int pageSize = 15, string sortExpression = null) where T : new()
        {
            return this.dbQuery.ToList<T>(dbBuilder, pageIndex, pageSize, sortExpression);
        }
        public GetPagedList<T> ToListAsync<T>(DBBuilder dbBuilder) where T : new()
        {
            return this.dbQuery.ToListAsync<T>(dbBuilder);
        }

        public T ToScalar<T>(DBBuilder dbBuilder)
        {
            return this.dbQuery.ToScalar<T>(dbBuilder);
        }
        public object ToScalar(DBBuilder dbBuilder)
        {
            return this.dbQuery.ToScalar(dbBuilder);
        }
        public void ToReader(DBBuilder dbBuilder, Action<IDataReader> action)
        {
            this.dbQuery.ToReader(dbBuilder, action);
        }

        public int Execute(DBBuilder dbBuilder)
        {
            return this.dbQuery.Execute(dbBuilder);
        }

        #endregion
    }
}
