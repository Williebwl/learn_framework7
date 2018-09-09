using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Data
{
    /// <summary>
    /// 数据库请求，仅供开发人员使用
    /// </summary>
    public interface IDBQuery : IDisposable
    {
        /// <summary>
        /// 执行SQL语句并获取返回值
        /// </summary>
        /// <param name="dbBuilder"></param>
        /// <returns></returns>
        DataTable ToDataTable(DBBuilder dbBuilder);
        /// <summary>
        /// 执行SQL语句并获取返回值
        /// </summary>
        /// <param name="dbBuilder"></param>
        /// <returns></returns>
        PagedList ToDataTable(DBBuilder dbBuilder, int pageIndex = 1, int pageSize = 15, string sortExpression = null);
        /// <summary>
        /// 执行SQL语句并获取返回值
        /// </summary>
        /// <param name="dbBuilder"></param>
        /// <returns></returns>
        GetPagedList ToDataTableAsync(DBBuilder dbBuilder);
        /// <summary>
        /// 执行SQL语句并获取返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbBuilder"></param>
        /// <returns></returns>
        List<T> ToList<T>(DBBuilder dbBuilder) where T : new();
        /// <summary>
        /// 执行SQL语句并获取返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbBuilder"></param>
        /// <returns></returns>
        List<T> ToList<T>(DBBuilder dbBuilder, string sortExpression = null) where T : new();
        /// <summary>
        /// 执行SQL语句并获取返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbBuilder"></param>
        /// <returns></returns>
        PagedList<T> ToList<T>(DBBuilder dbBuilder, int pageIndex = 1, int pageSize = 15, string sortExpression = null) where T : new();
        /// <summary>
        /// 执行SQL语句并获取返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbBuilder"></param>
        /// <returns></returns>
        GetPagedList<T> ToListAsync<T>(DBBuilder dbBuilder) where T : new();
        /// <summary>
        /// 执行SQL语句并获取返回值
        /// </summary>
        /// <param name="dbBuilder"></param>
        /// <returns></returns>
        object ToScalar(DBBuilder dbBuilder);
        /// <summary>
        /// 执行SQL语句并获取返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbBuilder"></param>
        /// <returns></returns>
        T ToScalar<T>(DBBuilder dbBuilder);
        /// <summary>
        /// 执行SQL语句并返回流
        /// </summary>
        /// <param name="dbBuilder"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        void ToReader(DBBuilder dbBuilder, Action<IDataReader> action);
        /// <summary>
        /// 执行SQL语句并获取影响的行数
        /// </summary>
        /// <param name="dbBuilder"></param>
        /// <returns></returns>
        int Execute(DBBuilder dbBuilder);
        
        /// <summary>
        /// 使用现有连接开启事务
        /// </summary>
        /// <param name="transaction"></param>
        void Begin(IDbTransaction transaction);
        /// <summary>
        /// 创建新连接开启事务
        /// </summary>
        void Begin(string connectionName = null);
        /// <summary>
        /// 执行事务
        /// </summary>
        void Commit();
        /// <summary>
        /// 取消事务
        /// </summary>
        void Rollback();
    }
}
