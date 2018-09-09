using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Data;

namespace BIStudio.Framework.Domain
{
    public class SortExpression<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// key:属性名，value,true为升序，false为降序
        /// </summary>
        private List<KeyValuePair<string, bool>> SortList { get; set; }
        public SortExpression()
        {

        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sortExpression">排序表达式，例如：ID Desc, Sequence Asc</param>
        public SortExpression(string sortExpression)
        {
            SortList = new List<KeyValuePair<string, bool>>();
            foreach (var field in sortExpression.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                var kv = field.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (kv.Length == 2)
                    SortList.Add(new KeyValuePair<string, bool>(kv.First().Trim(), kv.Last().Trim().ToLower() == "asc"));
                else if (kv.Length == 1)
                    SortList.Add(new KeyValuePair<string, bool>(kv.First().Trim(), true));
            }
        }

        public virtual IQueryable<TEntity> Lambda(IQueryable<TEntity> query)
        {
            if (SortList == null || SortList.Count == 0)
                return query;

            //创建表达式变量参数 item
            var parameter = Expression.Parameter(typeof(TEntity), "item");
            SortList.ForEach(item =>
            {
                //根据属性名获取属性 
                var property = typeof(TEntity).GetProperty(item.Key);
                //创建一个访问属性的表达式 item.property
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                //创建表达式 item=>item.property
                var orderByExp = Expression.Lambda(propertyAccess, parameter);

                var resultExp = Expression.Call(typeof(Queryable),
                    item.Value ? "OrderByDescending" : "OrderBy",
                    new[] { typeof(TEntity), property.PropertyType }, query.Expression, Expression.Quote(orderByExp));
                query = query.Provider.CreateQuery<TEntity>(resultExp);
            });
            return query;
        }

        public virtual string Sql()
        {
            if (SortList == null || SortList.Count == 0)
                return string.Empty;
            return string.Join(",", SortList.Select(item => string.Format("{0} {1}", DBBuilder.Define().Field(null, item.Key), item.Value ? "asc" : "desc")));
        }
    }
}