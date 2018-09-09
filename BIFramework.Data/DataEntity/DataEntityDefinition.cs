using BIStudio.Framework.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Data
{
    /// <summary>
    /// 数据实体定义
    /// </summary>
    public class DataEntityDefinition : ICloneable
    {
        public DataEntityDefinition()
        {
        }
        public DataEntityDefinition(Type type)
        {
            this.Type = type;
            this.PropertyInfos = type.GetProperties();
            this.TableAttribute = type.GetCustomAttribute<TableAttribute>(false) ?? new TableAttribute
            {
                TableName = type.Name.EndsWith("Info") ? type.Name.Substring(0, type.Name.Length - 4) : type.Name,
                PrimaryKey = "ID",
                PrimaryKeyType = PKIDType.Custom,
                IsForcein = false,
                AutoLog = false
            };
            this.ColumnAttributes = new Dictionary<string, ColumnAttribute>();
            this.DbParameters = new DBParameterList();
            Array.ForEach<PropertyInfo>(this.PropertyInfos, pi =>
            {
                var fieldAttr = pi.GetCustomAttribute<ColumnAttribute>(false) ?? new ColumnAttribute();
                fieldAttr.IsInherit = (pi.DeclaringType != type && type.BaseType.GetCustomAttributes<TableAttribute>(false).Count() > 0);
                this.ColumnAttributes.Add(pi.Name, fieldAttr);
            });
        }

        #region 数据表映射
        
        /// <summary>
        /// 数据实体实例
        /// </summary>
        public IDataEntity Instance { get; set; }
        /// <summary>
        /// 数据实体实例的类型
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// 数据实体实例的所有属性
        /// </summary>
        public PropertyInfo[] PropertyInfos { get; set; }
        /// <summary>
        /// 在数据实体中设置的表的自定义属性，运行后可以动态更改表的属性
        /// </summary>
        public TableAttribute TableAttribute { get; set; }
        /// <summary>
        /// 在数据实体中设置的表的自定义字段属性的数据字典集合，运行后可以动态更改字段属性
        /// </summary>
        public Dictionary<string, ColumnAttribute> ColumnAttributes { get; set; }
        /// <summary>
        /// 当此数据实体作为查询对象时，需要的额外参数
        /// </summary>
        public DBParameterList DbParameters { get; set; }

        #endregion

        #region 复制副本
        
        public DataEntityDefinition Clone()
        {
            //复制字段定义
            Dictionary<string, ColumnAttribute> fields = new Dictionary<string, ColumnAttribute>();
            foreach (var kv in this.ColumnAttributes)
                fields.Add(kv.Key, kv.Value.Clone());

            return new DataEntityDefinition
            {
                Type = this.Type,
                PropertyInfos = this.PropertyInfos,
                TableAttribute = this.TableAttribute.Clone(),
                ColumnAttributes = fields,
                DbParameters = new DBParameterList(this.DbParameters),
            };
        }

        public DataEntityDefinition Clone(IDataEntity dataEntity)
        {
            DataEntityDefinition definition = this.Clone();
            definition.Instance = dataEntity;
            return definition;
        }
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        #region 字段赋值
        
        /// <summary>
        /// 获取数据实体字段属性值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public object this[string fieldName]
        {
            get
            {
                return this.GetValue(fieldName);
            }
            set
            {
                this.SetValue(fieldName, value);
            }
        }
        
        /// <summary>
        /// 转化类型包括可空类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        private object ChangeType(object value, Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null || value == DBNull.Value) return null;
                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, conversionType);
        }

        #endregion

        #region 字段操作
        
        /// <summary>
        /// 根据字段名取值(DAO需要用的)
        /// 修改时会把数据库中相应字段置NULL
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public object GetValue(string fieldName)
        {
            PropertyInfo p = this.Type.GetProperty(fieldName);
            if (p == null) return null;
            if (this.ColumnAttributes[fieldName].IsDBNull == true)
                return DBNull.Value;
            else
                return p.GetValue(this.Instance, null);
        }
 
        /// <summary>
        /// 根据字段名附值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public void SetValue(string fieldName, object value)
        {
            PropertyInfo p = this.Type.GetProperty(fieldName);
            if (p != null)
            {
                if (value == DBNull.Value)
                {
                    //this.FieldAttributes[fieldName].IsDBNull = true;//是否可以去掉?
                }
                else
                    p.SetValue(this.Instance, ChangeType(value, p.PropertyType), null);
            }
        }

        /// <summary>
        /// 为了给文本框付值，如果字段为空会返回空字符串
        /// 如果是日期类型只返回短日期字符串
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public string GetValueForTextBox(string fieldName)
        {
            PropertyInfo p = this.Type.GetProperty(fieldName);
            object value = p == null ? null : p.GetValue(this.Instance, null);
            if (value == null) return "";
            if (value is DateTime)
                return ((DateTime)value).ToString("yyyy年M月d日");
            else return value.ToString();
        }

        /// <summary>
        /// 主要为了从TextBox中取值如果是空字符串那么修改时会把数据库中相应字段置NULL
        /// 如果格式错误时抛出异常，异常的Source为TextBox的客户端ID为了出错时把相应输入框设置焦点
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public void SetValueByTextBox(string fieldName, object value, string textBoxClientID)
        {
            PropertyInfo p = this.Type.GetProperty(fieldName);
            if (p == null) return;
            if (value == DBNull.Value || value.ToString() == string.Empty)
                this.ColumnAttributes[fieldName].IsDBNull = true;
            else
            {
                try
                {
                    p.SetValue(this.Instance, ChangeType(value, p.PropertyType), null);
                }
                catch (FormatException ex)
                {
                    ex.Source = textBoxClientID;
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 获取字段是否为空
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public bool IsDBNull(string fieldName)
        {
            return this.ColumnAttributes[fieldName].IsDBNull;
        }
        /// <summary>
        /// 设置字段是否为空
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="isDBNull"></param>
        public void IsDBNull(string fieldName, bool isDBNull)
        {
            this.ColumnAttributes[fieldName].IsDBNull = isDBNull;
        }

        #endregion

        #region 查询条件
        
        /// <summary>
        /// 获取字段的查询条件
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public string Where(string fieldName)
        {
            return this.ColumnAttributes[fieldName].WhereCondition;
        }
        /// <summary>
        /// 设置字段的查询条件
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="whereCondition"></param>
        /// <returns></returns>
        public void Where(string fieldName, string whereCondition)
        {
            this.ColumnAttributes[fieldName].WhereCondition = whereCondition;
        }
        /// <summary>
        /// 设置字段的查询条件
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="whereCondition"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public void Where(string fieldName, string whereCondition, object param)
        {
            this.ColumnAttributes[fieldName].WhereCondition = whereCondition;
            this.DbParameters.AddExpandoParams(param);
        }

        #endregion
        
    }
}
