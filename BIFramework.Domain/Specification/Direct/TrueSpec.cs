using BIStudio.Framework.Data;
using System;
using System.Linq.Expressions;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    ///     永远为真的查询
    /// </summary>
    /// <typeparam name="TEntity">Type of entity in this specification</typeparam>
    public sealed class TrueSpec<TEntity>
        : SpecificationBase<TEntity>
        where TEntity : class
    {
        public TrueSpec() : base(DBBuilder.Define("1=1"), exp => true) { }
    }
}