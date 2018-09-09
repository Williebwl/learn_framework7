using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Data
{
    /// <summary>
    /// 参数列表
    /// </summary>
    public class DBParameterList
    {
        internal DynamicParameters Instance = new DynamicParameters();
        public DBParameterList()
            : base()
        {
            
        }
        public DBParameterList(object template)
            : base()
        {
            AddExpandoParams(template);
        }

        public IEnumerable<string> ParameterNames
        {
            get
            {
                return this.Instance.ParameterNames;
            }
        }

        public T Get<T>(string name)
        {
            return this.Instance.Get<T>(name);
        }

        public void Add(string name, object value = null, DbType? dbType = null, ParameterDirection? direction = null, int? size = null, byte? precision = null, byte? scale = null)
        {
            this.Instance.Add(name, value, dbType, direction, size, precision, scale);
        }

        /// <summary>
        /// 添加动态参数
        /// </summary>
        /// <param name="param"></param>
        public DBParameterList AddExpandoParams(dynamic param)
        {
            param = param is DBParameterList ? (param as DBParameterList).Instance : param;
            var obj = param as object;
            if (obj != null)
            {
                var subDynamic = obj as DynamicParameters;
                if (subDynamic == null)
                {
                    var dictionary = obj as IEnumerable<KeyValuePair<string, object>>;
                    if (dictionary == null)
                    {
                        IDictionary<string, object> result = new ExpandoObject();
                        foreach (PropertyDescriptor pro in TypeDescriptor.GetProperties(param.GetType()))
                            result.Add(pro.Name, pro.GetValue(param));
                        this.Instance.AddDynamicParams(result as ExpandoObject);
                    }
                    else
                    {
                        this.Instance.AddDynamicParams(param);
                    }
                }
                else
                {
                    this.Instance.AddDynamicParams(param);
                }
            }
            return this;
        }

    }
}
