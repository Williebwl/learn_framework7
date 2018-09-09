using System;
using System.Linq;
using System.Linq.Expressions;
using BIStudio.Framework.Utils;
using BIStudio.Framework.Permission;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Data;

namespace BIStudio.Framework.Permission
{
    /// <summary>
    /// 定义基础过滤器
    /// </summary>
    [FilterProvider(EnumFilterOperation.LessThan)]
    [FilterProvider(EnumFilterOperation.LessThanOrEqual)]
    [FilterProvider(EnumFilterOperation.GreaterThan)]
    [FilterProvider(EnumFilterOperation.GreaterThanOrEqual)]
    [FilterProvider(EnumFilterOperation.Equal)]
    [FilterProvider(EnumFilterOperation.NotEqual)]
    [FilterProvider(EnumFilterOperation.Like)]
    [FilterProvider(EnumFilterOperation.NotLike)]
    public class BasicFilterProvider : IFilterProvider
    {
        private EnumFilterOperation _operationType;

        private SYSFilter _config;

        private string _propertyName;

        public void Init(SYSFilter config)
        {
            _operationType = (EnumFilterOperation)ALEnumDescription.GetEnumValueByDisplayText(typeof(EnumFilterOperation), config.FilterOperation);
            _config = config;
            _propertyName = config.PropertyName;
        }

        /// <summary>
        /// 这里需要处理object的类型
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public ISpecification<TEntity> Compile<TEntity>() where TEntity : class
        {
            string value = _config.FilterValue;
            Expression<Func<TEntity, bool>> lambda = item => true;
            DBBuilder sql = DBBuilder.Define("1=1");
            var param = Expression.Parameter(typeof(TEntity), "item");
            var prorerty = Expression.Property(param, _propertyName);
            var valueExpress = Convert(value, typeof(TEntity).GetProperty(_propertyName).PropertyType);

            switch (_operationType)
            {
                case EnumFilterOperation.Equal:
                    lambda = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(prorerty, valueExpress), param);
                    sql = DBBuilder.Define().Field(null, _propertyName).Eq().Value(value);
                    break;
                case EnumFilterOperation.NotEqual:
                    lambda = Expression.Lambda<Func<TEntity, bool>>(Expression.NotEqual(prorerty, valueExpress), param);
                    sql = DBBuilder.Define().Field(null, _propertyName).Ne().Value(value);
                    break;
                case EnumFilterOperation.GreaterThan:
                    lambda = Expression.Lambda<Func<TEntity, bool>>(Expression.GreaterThan(prorerty, valueExpress), param);
                    sql = DBBuilder.Define().Field(null, _propertyName).Gt().Value(value);
                    break;
                case EnumFilterOperation.GreaterThanOrEqual:
                    lambda = Expression.Lambda<Func<TEntity, bool>>(Expression.GreaterThanOrEqual(prorerty, valueExpress), param);
                    sql = DBBuilder.Define().Field(null, _propertyName).Ge().Value(value);
                    break;
                case EnumFilterOperation.LessThan:
                    lambda = Expression.Lambda<Func<TEntity, bool>>(Expression.LessThan(prorerty, valueExpress), param);
                    sql = DBBuilder.Define().Field(null, _propertyName).Lt().Value(value);
                    break;
                case EnumFilterOperation.LessThanOrEqual:
                    lambda = Expression.Lambda<Func<TEntity, bool>>(Expression.LessThanOrEqual(prorerty, valueExpress), param);
                    sql = DBBuilder.Define().Field(null, _propertyName).Le().Value(value);
                    break;
                case EnumFilterOperation.Like:
                    lambda = Expression.Lambda<Func<TEntity, bool>>(Expression.Call(prorerty, typeof(string).GetMethod("Contains"), valueExpress), param);
                    sql = DBBuilder.Define().Field(null, _propertyName).Like().Value(value);
                    break;
                case EnumFilterOperation.NotLike:
                    lambda = Expression.Lambda<Func<TEntity, bool>>(Expression.IsFalse(Expression.Call(prorerty, typeof(string).GetMethod("Contains"), valueExpress)), param);
                    sql = DBBuilder.Define().Field(null, _propertyName).Not().Like().Value(value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new Spec<TEntity>(lambda, sql);
        }

        private UnaryExpression Convert(string value, Type type)
        {
            if(!type.IsValueType)
                return Expression.Convert(Expression.Constant(value), type);

            object changedValue = ALCommon.ConvertTo(value, type);
            if (type.GenericTypeArguments != null && !type.GenericTypeArguments.Any())
                type = type.GenericTypeArguments[0];
            return Expression.Convert(Expression.Constant(changedValue), type);
        }
    }
}
