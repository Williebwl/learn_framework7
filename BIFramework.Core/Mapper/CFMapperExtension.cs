using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework
{
    public static class CFMapperExtension
    {
        public static IEnumerable<TTarget> Map<TSource, TTarget>(this IEnumerable<TSource> source)
            where TTarget : new()
        {
            if (source == null) return null;

            return source.Select(d => CFMapper.Map<TSource, TTarget>(d));
        }
        public static TTarget Map<TSource, TTarget>(this TSource source)
            where TTarget : new()
        {
            if (source == null) return default(TTarget);

            return CFMapper.Map<TSource, TTarget>(source);
        }


        public static IEnumerable<TTarget> Map<TSource, TTarget>(this IEnumerable<TSource> source, TTarget target, params string[] ignoreMembers)
            where TTarget : new()
        {
            if (source == null) return null;

            return source.Select(src => CFMapper.Map<TSource, TTarget>(src, target, ignoreMembers));
        }
        public static TTarget Map<TSource, TTarget>(this TSource source, TTarget target, params string[] ignoreMembers)
            where TTarget : new()
        {
            if (source == null) return default(TTarget);

            return CFMapper.Map<TSource, TTarget>(source, target, ignoreMembers);
        }
    }
}
