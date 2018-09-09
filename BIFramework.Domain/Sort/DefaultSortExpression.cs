using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 空的排序表达式
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class DefaultSortExpression<TEntity> : SortExpression<TEntity> where TEntity : IEntity
    {
        public override IQueryable<TEntity> Lambda(IQueryable<TEntity> query)
        {
            string empty = null;
            return query.OrderBy(item => empty);
        }
        public override string Sql()
        {
            return "(SELECT NULL)";
        }
    }
}
