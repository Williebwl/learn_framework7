using BIStudio.Framework;
using BIStudio.Framework.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Configuration;

namespace BIStudio.Framework.Data
{
    [Ioc(typeof(IDBQuery))]
    public sealed class DBQuery : IDBQuery, IDisposable
    {
        #region 内部属性

        private readonly ConnectionStringSettings connection;
        public DBQuery(string connectionName = null)
        {
            connection = CFConfig.GetConnection(connectionName);
        }

        protected T Do<T>(DBBuilder dbBuilder, Func<IDbConnection, T> fn)
        {
            if (dbBuilder.Transaction != null)
            {
                return fn(dbBuilder.Transaction.Connection);
            }
            else if (this.Transaction != null)
            {
                return fn(dbBuilder.SetTransaction(this.Transaction).Transaction.Connection);
            }
            else
            {
                using (var connection = DBAdapter.CreateConnection(this.connection.ConnectionString))
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();
                    T result = fn(connection);
                    if (connection.State != ConnectionState.Closed)
                        connection.Close();
                    return result;
                }
            }
        }

        private IDBAdapter _dbAdapter;
        protected IDBAdapter DBAdapter
        {
            get
            {
                return _dbAdapter = _dbAdapter ?? DBUtils.GetDBAdapter(connection.ProviderName);
            }
        }

        private IDBAnalyzer _dbAnalyzer;
        protected IDBAnalyzer DBAnalyzer
        {
            get
            {
                return _dbAnalyzer = _dbAnalyzer ?? DBUtils.GetDBAnalyzer(connection.ProviderName);
            }
        }

        #endregion

        #region 事务管理

        private bool disposeConnection = false;
        private IDbTransaction Transaction;
        /// <summary>
        /// 使用现有连接开启事务
        /// </summary>
        /// <param name="transaction"></param>
        public void Begin(IDbTransaction transaction)
        {
            this.Transaction = transaction;
        }
        /// <summary>
        /// 创建新连接开启事务
        /// </summary>
        public void Begin(string connectionName = null)
        {
            IDbConnection connection = DBAdapter.CreateConnection(CFConfig.GetConnection(connectionName).ConnectionString);
            connection.Open();
            this.Transaction = connection.BeginTransaction(IsolationLevel.Serializable);
            disposeConnection = true;
        }
        /// <summary>
        /// 执行事务
        /// </summary>
        public void Commit()
        {
            if (this.Transaction != null)
                this.Transaction.Commit();
        }
        /// <summary>
        /// 取消事务
        /// </summary>
        public void Rollback()
        {
            if (this.Transaction != null)
                this.Transaction.Rollback();
        }
        public void Dispose()
        {
            if (!disposeConnection)
                return;

            IDbConnection cn = (this.Transaction != null ? this.Transaction.Connection : null);

            if (this.Transaction != null)
                this.Transaction.Dispose();
            if (cn != null)
                cn.Dispose();
        }

        #endregion

        public DataTable ToDataTable(DBBuilder dbBuilder)
        {
            return this.Do(dbBuilder, connection =>
            {
                DataTable dt = new DataTable();
                using (var reader = connection.ExecuteReader(dbBuilder.GetCommand(DBAdapter)))
                {
                    dt.Load(reader);
                }
                return dt;
            });
        }
        public PagedList ToDataTable(DBBuilder dbBuilder, int pageIndex, int pageSize, string sortExpression = null)
        {
            dbBuilder.Compile(this.DBAdapter);
            var dbAnalyzer = DBAnalyzer.Init(dbBuilder.Sql, pageIndex, pageSize, sortExpression);
            dbBuilder.Sql = dbAnalyzer.Count();
            var count = Convert.ToInt32(ToScalar(dbBuilder));
            dbBuilder.Sql = dbAnalyzer.Select();
            return new PagedList
            {
                CurrentPage = pageIndex,
                ItemsPerPage = pageSize,
                TotalItems = count,
                TotalPages = count % pageSize == 0 ? count / pageSize : (count / pageSize) + 1,
                Items = ToDataTable(dbBuilder).AsEnumerable().ToList(),
            };
        }

        public GetPagedList ToDataTableAsync(DBBuilder dbBuilder)
        {
            return delegate(int pageIndex, int pageSize, out int count, string sortExpression)
            {
                dbBuilder.Compile(this.DBAdapter);
                var dbAnalyzer = DBAnalyzer.Init(dbBuilder.Sql, pageIndex, pageSize, sortExpression);
                dbBuilder.Sql = dbAnalyzer.Count();
                count = Convert.ToInt32(ToScalar(dbBuilder));
                dbBuilder.Sql = dbAnalyzer.Select();
                return new PagedList
                {
                    CurrentPage = pageIndex,
                    ItemsPerPage = pageSize,
                    TotalItems = count,
                    TotalPages = count % pageSize == 0 ? count / pageSize : (count / pageSize) + 1,
                    Items = ToDataTable(dbBuilder).AsEnumerable().ToList(),
                };
            };
        }


        public List<T> ToList<T>(DBBuilder dbBuilder) where T : new()
        {
            return ToList<T>(dbBuilder, null);
        }
        public List<T> ToList<T>(DBBuilder dbBuilder, string sortExpression = null) where T : new()
        {
            if (sortExpression == null)
                return this.Do(dbBuilder, connection => connection.Query<T>(dbBuilder.GetCommand(DBAdapter)).ToList());

            dbBuilder.Compile(this.DBAdapter);
            var dbAnalyzer = DBAnalyzer.Init(dbBuilder.Sql, null, null, sortExpression);
            dbBuilder.Sql = dbAnalyzer.Select();
            return this.Do(dbBuilder, connection => connection.Query<T>(dbBuilder.GetCommand(DBAdapter)).ToList());
        }
        /// <summary>
        /// 执行SQL语句并获取返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbBuilder"></param>
        /// <returns></returns>
        public PagedList<T> ToList<T>(DBBuilder dbBuilder, int pageIndex = 1, int pageSize = 15, string sortExpression = null) where T : new()
        {
            dbBuilder.Compile(this.DBAdapter);
            var dbAnalyzer = DBAnalyzer.Init(dbBuilder.Sql, pageIndex, pageSize, sortExpression);
            dbBuilder.Sql = dbAnalyzer.Count();
            var count = Convert.ToInt32(ToScalar(dbBuilder));
            dbBuilder.Sql = dbAnalyzer.Select();
            return new PagedList<T>
            {
                CurrentPage = pageIndex,
                ItemsPerPage = pageSize,
                TotalItems = count,
                TotalPages = count % pageSize == 0 ? count / pageSize : (count / pageSize) + 1,
                Items = ToList<T>(dbBuilder)
            };
        }
        /// <summary>
        /// 执行SQL语句并获取返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbBuilder"></param>
        /// <returns></returns>
        public GetPagedList<T> ToListAsync<T>(DBBuilder dbBuilder) where T : new()
        {
            return delegate(int pageIndex, int pageSize, out int count, string sortExpression)
            {
                dbBuilder.Compile(this.DBAdapter);
                var dbAnalyzer = DBAnalyzer.Init(dbBuilder.Sql, pageIndex, pageSize, sortExpression);
                dbBuilder.Sql = dbAnalyzer.Count();
                count = Convert.ToInt32(ToScalar(dbBuilder));
                dbBuilder.Sql = dbAnalyzer.Select();
                return new PagedList<T>
                {
                    CurrentPage = pageIndex,
                    ItemsPerPage = pageSize,
                    TotalItems = count,
                    TotalPages = count % pageSize == 0 ? count / pageSize : (count / pageSize) + 1,
                    Items = ToList<T>(dbBuilder)
                };
            };
        }
        public T ToScalar<T>(DBBuilder dbBuilder)
        {
            return this.Do(dbBuilder, connection => connection.ExecuteScalar<T>(dbBuilder.GetCommand(DBAdapter)));
        }
        public object ToScalar(DBBuilder dbBuilder)
        {
            return this.Do(dbBuilder, connection => connection.ExecuteScalar(dbBuilder.GetCommand(DBAdapter)));
        }
        public void ToReader(DBBuilder dbBuilder, Action<IDataReader> action)
        {
            this.Do(dbBuilder, connection =>
            {
                var reader = connection.ExecuteReader(dbBuilder.GetCommand(DBAdapter));
                action(reader);
                return reader;
            });
        }

        public int Execute(DBBuilder dbBuilder)
        {
            return this.Do(dbBuilder, connection => connection.Execute(dbBuilder.GetCommand(DBAdapter)));
        }
    }
}
