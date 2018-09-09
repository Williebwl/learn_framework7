using BIStudio.Framework.Data;
using System;
using System.Linq.Expressions;
namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 组合规约
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class CompositeSpecification<TEntity>
        : SpecificationBase<TEntity>
        where TEntity : class
    {
        public CompositeSpecification(ISpecification<TEntity> leftSide, ISpecification<TEntity> rightSide)
            : base()
        {
            if (leftSide == null)
                throw new ArgumentNullException("leftSide");
            if (rightSide == null)
                throw new ArgumentNullException("rightSide");

            this.Sql = GetSql(leftSide, rightSide);
            this.Lambda = GetLambda(leftSide, rightSide);
        }
        abstract protected DBBuilder GetSql(ISpecification<TEntity> leftSide, ISpecification<TEntity> rightSide);
        abstract protected Expression<Func<TEntity, bool>> GetLambda(ISpecification<TEntity> leftSide, ISpecification<TEntity> rightSide);
    }
}