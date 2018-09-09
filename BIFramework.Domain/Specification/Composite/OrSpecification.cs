using BIStudio.Framework.Data;
using System;
using System.Linq.Expressions;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    ///     A Logic OR Specification
    /// </summary>
    /// <typeparam name="T">Type of entity that check this specification</typeparam>
    public sealed class OrSpecification<T>
        : CompositeSpecification<T>
        where T : class
    {

        /// <summary>
        ///     Default constructor for AndSpecification
        /// </summary>
        /// <param name="leftSide">Left side specification</param>
        /// <param name="rightSide">Right side specification</param>
        public OrSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
            : base(leftSide, rightSide)
        {
        }
        override protected DBBuilder GetSql(ISpecification<T> leftSide, ISpecification<T> rightSide)
        {
            var left = leftSide.Sql;
            var right = rightSide.Sql;

            if (left != null && right != null)
                return /*left.FragmentExists(DBBuilder.FragmentKey.Where) ?
                    DBBuilder.Define(left).FragmentAppend(DBBuilder.FragmentKey.Select, DBBuilder.Define().Or(right)) :
                    */DBBuilder.Define(left).Or(right);
            else
                return left;
        }
        override protected Expression<Func<T, bool>> GetLambda(ISpecification<T> leftSide, ISpecification<T> rightSide)
        {
            var left = leftSide.Lambda;
            var right = rightSide.Lambda;

            if (left != null && right != null)
                return left.Or(right);
            else
                return left;
        }
    }
}