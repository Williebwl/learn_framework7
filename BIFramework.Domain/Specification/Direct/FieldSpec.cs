
using BIStudio.Framework.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 按属性值查询
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class FieldSpec<TEntity, TProperty>
        : SpecificationBase<TEntity>
        where TEntity : class
    {
        public FieldSpec(string propertyName, params TProperty[] propertyValues)
            : this(propertyName, propertyValues.AsEnumerable())
        {
        }
        public FieldSpec(string propertyName, IEnumerable<TProperty> propertyValues)
        {
            if (propertyValues == null || !propertyValues.Any())
            {
                var spec = new FalseSpec<TEntity>();
                Lambda = spec.Lambda;
                Sql = new FalseSpec<TEntity>().Sql;
            }
            else if (propertyValues.Count() == 1)
            {
                var prop = typeof(TEntity).GetProperty(propertyName);
                var entity = Expression.Parameter(typeof(TEntity), "entity");
                var left = Expression.Constant(propertyValues.First());
                var right = IsNullableType(prop.PropertyType) ? Expression.Property(Expression.Property(entity, prop), "Value") : Expression.Property(entity, prop);
                var exp = Expression.Equal(left, right);
                Lambda = Expression.Lambda<Func<TEntity, bool>>(exp, entity);
                //Sql = DBBuilder.Define().Field(DataEntityUtils.Entity(typeof(TEntity)).TableAttribute.TableName, propertyName).Eq().Value(propertyValues.First());
                Sql = DBBuilder.Define().Field(null, propertyName).Eq().Value(propertyValues.First());
            }
            else
            {
                var prop = typeof(TEntity).GetProperty(propertyName);
                var entity = Expression.Parameter(typeof(TEntity), "entity");
                var left = Expression.Constant(propertyValues.ToList());
                var right = IsNullableType(prop.PropertyType) ? Expression.Property(Expression.Property(entity, prop), "Value") : Expression.Property(entity, prop);
                var exp = Expression.Call(left, "Contains", null, right);
                Lambda = Expression.Lambda<Func<TEntity, bool>>(exp, entity);
                //Sql = DBBuilder.Define().Field(DataEntityUtils.Entity(typeof(TEntity)).TableAttribute.TableName, propertyName).In(d => d.Value(propertyValues));
                Sql = DBBuilder.Define().Field(null, propertyName).In(d => d.Value(propertyValues));
            }
        }
        private bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }
    }
}
