using BIStudio.Framework.Data;
using System;
using System.Linq.Expressions;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 定义一个规约
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public sealed class Spec<TEntity>
        : SpecificationBase<TEntity>
        where TEntity : class
    {
        #region Constructor

        /// <summary>
        /// 定义基于Lambda表达式的规约
        /// </summary>
        /// <param name="matchingCriteria"></param>
        public Spec(Expression<Func<TEntity, bool>> lambda)
        {
            Lambda = lambda;
        }
        /// <summary>
        /// 定义基于Sql表达式的规约
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        public Spec(string sql = null, dynamic param = null)
        {
            Sql = DBBuilder.Define(sql, param);
        }
        /// <summary>
        /// 定义基于Sql表达式的规约
        /// </summary>
        /// <param name="sql"></param>
        public Spec(DBBuilder sql)
        {
            Sql = sql;
        }        
        /// <summary>
        /// 定义基于Lambda表达式和Sql表达式的规约
        /// </summary>
        /// <param name="sql"></param>
        public Spec(Expression<Func<TEntity, bool>> lambda, DBBuilder sql)
        {
            Lambda = lambda;
            Sql = sql;
        }
        #endregion        
    }

}