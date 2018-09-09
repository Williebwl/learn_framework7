using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BIStudio.Framework;
using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using System.Data;
using BIStudio.Framework.Utils;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 按数据实体查询
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntitySpec<TEntity> : SpecificationBase<TEntity>
        where TEntity : class ,IEntity
    {
        private TEntity query;
        public EntitySpec(TEntity query)
        {
            IDataEntity info = query as IDataEntity;
            if (info != null)
                Sql = GetWhereCondition(info);
            else
                Sql = DBBuilder.Define();
        }
        public EntitySpec(Action<TEntity> matchingCriteria)
        {
            query = Activator.CreateInstance<TEntity>();
            matchingCriteria(query);
            IDataEntity info = query as IDataEntity;
            if (info != null)
                Sql = GetWhereCondition(info);
            else
                Sql = DBBuilder.Define();
        }

        protected DBBuilder GetWhereCondition(IDataEntity dataEntity)
        {
            DBBuilder builder = DBBuilder.Define("1=1 ");
            if (dataEntity == null) return builder;
            string tableName = dataEntity.Property.TableAttribute.TableName;
            foreach (PropertyInfo p in dataEntity.Property.PropertyInfos)
            {
                if (dataEntity.Property.ColumnAttributes[p.Name].IsExtend)
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(dataEntity.Property.ColumnAttributes[p.Name].WhereCondition))
                {
                    //如果设置了查询表达式则用这个查询表达式并且忽略其本身的值
                    builder.Append(string.Format("and ({0}) ", dataEntity.Property.ColumnAttributes[p.Name].WhereCondition));
                    continue;
                }

                object value = p.GetValue(dataEntity, null);
                if (value != null) //只查询付值的字段
                {
                    if (Type.GetTypeCode(p.PropertyType) == TypeCode.String || p.PropertyType.FullName.Contains("Guid"))//是字符串时使用参数方式查询
                    {
                        if (dataEntity.Property.ColumnAttributes[p.Name].IsExact)//精确查询
                        {
                            builder.Append(dbAdapter => string.Format("and {0}.{1} = {2} ", dbAdapter.FormatTable(tableName), dbAdapter.FormatField(p.Name), dbAdapter.FormatParameter(p.Name)));
                            builder.Parameters.Add(p.Name, value);
                        }
                        else //模糊查询
                        {
                            builder.Append(dbAdapter => string.Format("and {0}.{1} like {2} ", dbAdapter.FormatTable(tableName), dbAdapter.FormatField(p.Name), dbAdapter.FormatParameter(p.Name)));
                            builder.Parameters.Add(p.Name, string.Format("%{0}%", value));
                        }
                    }
                    else if (p.PropertyType.FullName.Contains("DateTime"))
                    {
                        builder.Append(dbAdapter => string.Format("and " + dbAdapter.FormatFunction("datediff", "{0}", "{1}.{2}", "{3}") + "=0 ", dataEntity.Property.ColumnAttributes[p.Name].DateTimePart, dbAdapter.FormatTable(tableName), dbAdapter.FormatField(p.Name), dbAdapter.FormatParameter(p.Name)));
                        builder.Parameters.Add(p.Name, value, DbType.DateTime);
                    }
                    else if (p.PropertyType.FullName.Contains("Boolean"))
                    {
                        builder.Append(dbAdapter => string.Format("and {0}.{1} = {2} ", dbAdapter.FormatTable(tableName), dbAdapter.FormatField(p.Name), Convert.ToBoolean(value) ? 1 : 0));
                    }
                    else
                    {
                        builder.Append(dbAdapter => string.Format("and {0}.{1} = {2} ", dbAdapter.FormatTable(tableName), dbAdapter.FormatField(p.Name), value));
                    }
                }
            }
            if (dataEntity.Property.DbParameters != null && dataEntity.Property.DbParameters.ParameterNames.Count() > 0)
                builder.Parameters.AddExpandoParams(dataEntity.Property.DbParameters);
            return builder;
        }
    }

    public static class EntitySpecExtensions
    {
        /// <summary>
        /// 将数据实体转换为规约
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static EntitySpec<TEntity> AsSpec<TEntity>(this TEntity entity) where TEntity : class ,IEntity
        {
            return new EntitySpec<TEntity>(entity);
        }
    }
}
