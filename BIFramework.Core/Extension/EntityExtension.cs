using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework
{
    public static class EntityExtension
    {
        /// <summary>
        /// 对象的浅复制
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static TEntity ShallowClone<TEntity>(this TEntity entity) where TEntity : ICloneable
        {
            return (TEntity) entity.Clone();
        }

        /// <summary>
        /// 对指定对象执行委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="work"></param>
        /// <returns></returns>
        public static T With<T>(this T item, Action<T> work)
        {
            work(item);
            return item;
        }
    }
}
