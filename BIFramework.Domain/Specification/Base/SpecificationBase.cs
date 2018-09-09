using BIStudio.Framework.Data;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 规约
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class SpecificationBase<TEntity>
        : ISpecification<TEntity>
        where TEntity : class
    {
        #region ISpecification<TEntity> Members

        /// <summary>
        ///     按照sql的方式去查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual DBBuilder Sql { get; protected set; }
        /// <summary>
        ///     Specification 的表达式模式,
        /// </summary>
        /// <returns>Expression that satisfy this specification</returns>
        public virtual Expression<Func<TEntity, bool>> Lambda { get; protected set; }

        public SpecificationBase()
        {
        }
        public SpecificationBase(DBBuilder sql = null, Expression<Func<TEntity, bool>> labmda = null)
        {
            this.Sql = sql;
            this.Lambda = labmda;
        }

        #endregion

        #region Override Operators

        /// <summary>
        ///     And operator
        /// </summary>
        /// <param name="leftSideSpecification">left operand in this AND operation</param>
        /// <param name="rightSideSpecification">right operand in this AND operation</param>
        /// <returns>New specification</returns>
        public static SpecificationBase<TEntity> operator &(
            SpecificationBase<TEntity> leftSideSpecification, SpecificationBase<TEntity> rightSideSpecification)
        {
            return new AndSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
        }

        /// <summary>
        ///     Or operator
        /// </summary>
        /// <param name="leftSideSpecification">left operand in this OR operation</param>
        /// <param name="rightSideSpecification">left operand in this OR operation</param>
        /// <returns>New specification </returns>
        public static SpecificationBase<TEntity> operator |(
            SpecificationBase<TEntity> leftSideSpecification, SpecificationBase<TEntity> rightSideSpecification)
        {
            return new OrSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
        }

        /// <summary>
        ///     Not specification
        /// </summary>
        /// <param name="specification">Specification to negate</param>
        /// <returns>New specification</returns>
        public static SpecificationBase<TEntity> operator !(SpecificationBase<TEntity> specification)
        {
            return new NotSpecification<TEntity>(specification);
        }

        /// <summary>
        ///     Override operator false, only for support AND OR operators
        /// </summary>
        /// <param name="specification">Specification instance</param>
        /// <returns>See False operator in C#</returns>
        public static bool operator false(SpecificationBase<TEntity> specification)
        {
            return false;
        }

        /// <summary>
        ///     Override operator True, only for support AND OR operators
        /// </summary>
        /// <param name="specification">Specification instance</param>
        /// <returns>See True operator in C#</returns>
        public static bool operator true(SpecificationBase<TEntity> specification)
        {
            return false;
        }

        #endregion
    }
}