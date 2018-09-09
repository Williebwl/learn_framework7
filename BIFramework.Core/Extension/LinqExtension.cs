using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework
{
    public static class LinqExtension
    {
        /// <summary>
        /// 扩展List的ForEach到IEnumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }
    }
}
