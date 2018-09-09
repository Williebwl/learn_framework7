using BIStudio.Framework.Data;
using System;
using System.Linq.Expressions;

namespace BIStudio.Framework.Domain
{
    public interface ISpecification<TEntity>
        where TEntity : class
    {
        /// <summary>
        ///     内存中的查询，或者ef/hn这种orm的查询方式
        /// </summary>
        /// <returns></returns>
        Expression<Func<TEntity, bool>> Lambda { get; }

        /// <summary>
        ///     将契约编译为查询表达式
        /// </summary>
        /// <returns></returns>
        DBBuilder Sql { get; }
    }
}