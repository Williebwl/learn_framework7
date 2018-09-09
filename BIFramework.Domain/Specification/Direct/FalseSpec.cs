using BIStudio.Framework.Data;
using System;
using System.Linq.Expressions;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    ///     永远为假的查询
    /// </summary>
    /// <typeparam name="TEntity">Type of entity in this specification</typeparam>
    public sealed class FalseSpec<TEntity>
        : SpecificationBase<TEntity>
        where TEntity : class
    {
        public FalseSpec() : base(DBBuilder.Define("1=0"), exp => false) { }

    }
}