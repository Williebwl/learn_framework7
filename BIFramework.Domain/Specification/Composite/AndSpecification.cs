using BIStudio.Framework.Data;
using System;
using System.Linq.Expressions;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    ///     A logic AND Specification
    /// </summary>
    /// <typeparam name="T">Type of entity that check this specification</typeparam>
    public sealed class AndSpecification<T>
        : CompositeSpecification<T>
        where T : class
    {
        /// <summary>
        ///     Default constructor for AndSpecification
        /// </summary>
        /// <param name="leftSide">Left side specification</param>
        /// <param name="rightSide">Right side specification</param>
        public AndSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
            : base(leftSide, rightSide)
        {
        }
        override protected DBBuilder GetSql(ISpecification<T> leftSide, ISpecification<T> rightSide)
        {
            var left = leftSide.Sql;
            var right = rightSide.Sql;

            if (left != null && right != null)
                return /*left.FragmentExists(DBBuilder.FragmentKey.Where) ?
                    DBBuilder.Define(left).FragmentAppend(DBBuilder.FragmentKey.Select, DBBuilder.Define().And(right)) :
                    */DBBuilder.Define(left).And(right);
            else
                return left;
        }
        override protected Expression<Func<T, bool>> GetLambda(ISpecification<T> leftSide, ISpecification<T> rightSide)
        {
            var left = leftSide.Lambda;
            var right = rightSide.Lambda;

            if (left != null && right != null)
                return left.And(right);
            else
                return left;
        }
    }
}