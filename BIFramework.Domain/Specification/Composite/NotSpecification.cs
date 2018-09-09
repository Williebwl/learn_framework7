using BIStudio.Framework.Data;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 表示否定规约
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public sealed class NotSpecification<TEntity>
        : SpecificationBase<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// 初始化否定规约
        /// </summary>
        /// <param name="originalSpecification"></param>
        public NotSpecification(ISpecification<TEntity> originalSpecification)
        {
            if (originalSpecification == null)
                throw new ArgumentNullException("originalSpecification");

            this.Sql = GetSql(originalSpecification);
            this.Lambda = GetLambda(originalSpecification);
        }
        protected DBBuilder GetSql(ISpecification<TEntity> originalSpecification)
        {
            var original = originalSpecification.Sql;

            if (original != null)
                return DBBuilder.Define(original).Not(original);
            else
                return original;
        }
        protected Expression<Func<TEntity, bool>> GetLambda(ISpecification<TEntity> originalSpecification)
        {
            var original = originalSpecification.Lambda;

            if (original != null)
                return Expression.Lambda<Func<TEntity, bool>>(Expression.Not(original.Body), original.Parameters.Single());
            else
                return original;
        }
    }
}