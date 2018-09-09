using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Data
{
    /// <summary>
    /// 数据实体工具类
    /// </summary>
    public abstract class DataEntityUtils
    {
        protected static ConcurrentDictionary<Type, DataEntityDefinition> Definitions = new ConcurrentDictionary<Type, DataEntityDefinition>();
        /// <summary>
        /// 获取数据实体定义
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DataEntityDefinition Entity<T>() where T : IDataEntity
        {
            return Entity(typeof(T));
        }
        /// <summary>
        /// 获取数据实体定义
        /// </summary>
        /// <returns></returns>
        public static DataEntityDefinition Entity(Type entityType)
        {
            return Definitions.GetOrAdd(entityType, type => new DataEntityDefinition(type));
        }
    }
}
