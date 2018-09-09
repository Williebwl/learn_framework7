using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    public static class RepositoryExtension
    {
        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, ISpecification<TSource> specification)
            where TSource : class
        {
            if (specification == null || specification.Lambda == null)
                CFException.Create(OperateResult.NotFound, "指定的规约未定义Lambda表达式");
            return source.Where(specification.Lambda);
        }
        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, ISpecification<TSource> specification)
            where TSource : class
        {
            if (specification == null || specification.Lambda == null)
                CFException.Create(OperateResult.NotFound, "指定的规约未定义Lambda表达式");
            return source.Where(specification.Lambda.Compile());
        }

        public static bool Remove<TEntity>(this IRepository<TEntity> bo, string ids)
            where TEntity : class, IEntity
        {
            if (bo == null) throw new ArgumentNullException(nameof(bo));

            return bo.Remove(new IDSpec<TEntity>(ids));
        }
    }
}
