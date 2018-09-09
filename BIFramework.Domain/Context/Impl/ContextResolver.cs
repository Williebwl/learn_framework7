using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 瞬态依赖解析器
    /// </summary>
    public abstract class ContextResolver : IAspectResolver<ITransientDependency>
    {
        private IAspectResolver<ITransientDependency> resolver = CFAspect.Constructor<ITransientDependency>();

        public virtual ITransientDependency Resolve(Type target)
        {
            return resolver.Resolve(target);
        }

        public virtual T Resolve<T>() where T : ITransientDependency
        {
            return resolver.Resolve<T>();
        }

        public IAspectResolver<ITransientDependency> Resolve<T>(out T instance) where T : ITransientDependency
        {
            instance = Resolve<T>();
            return this;
        }

        public IAspectResolver<ITransientDependency> Resolve<T1, T2>(out T1 t1, out T2 t2)
            where T1 : ITransientDependency
            where T2 : ITransientDependency
        {
            return Resolve(out t1).Resolve(out t2);
        }

        public IAspectResolver<ITransientDependency> Resolve<T1, T2, T3>(out T1 t1, out T2 t2, out T3 t3)
            where T1 : ITransientDependency
            where T2 : ITransientDependency
            where T3 : ITransientDependency
        {
            return Resolve(out t1).Resolve(out t2).Resolve(out t3);
        }

        public IAspectResolver<ITransientDependency> Resolve<T1, T2, T3, T4>(out T1 t1, out T2 t2, out T3 t3, out T4 t4)
            where T1 : ITransientDependency
            where T2 : ITransientDependency
            where T3 : ITransientDependency
            where T4 : ITransientDependency
        {
            return Resolve(out t1).Resolve(out t2).Resolve(out t3).Resolve(out t4);
        }

        public IAspectResolver<ITransientDependency> Resolve<T1, T2, T3, T4, T5>(out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5)
            where T1 : ITransientDependency
            where T2 : ITransientDependency
            where T3 : ITransientDependency
            where T4 : ITransientDependency
            where T5 : ITransientDependency
        {
            return Resolve(out t1).Resolve(out t2).Resolve(out t3).Resolve(out t4).Resolve(out t5);
        }

        public IAspectResolver<ITransientDependency> Resolve<T1, T2, T3, T4, T5, T6>(out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6)
            where T1 : ITransientDependency
            where T2 : ITransientDependency
            where T3 : ITransientDependency
            where T4 : ITransientDependency
            where T5 : ITransientDependency
            where T6 : ITransientDependency
        {
            return Resolve(out t1).Resolve(out t2).Resolve(out t3).Resolve(out t4).Resolve(out t5).Resolve(out t6);
        }

        public IAspectResolver<ITransientDependency> Resolve<T1, T2, T3, T4, T5, T6, T7>(out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, out T7 t7)
            where T1 : ITransientDependency
            where T2 : ITransientDependency
            where T3 : ITransientDependency
            where T4 : ITransientDependency
            where T5 : ITransientDependency
            where T6 : ITransientDependency
            where T7 : ITransientDependency
        {
            return Resolve(out t1).Resolve(out t2).Resolve(out t3).Resolve(out t4).Resolve(out t5).Resolve(out t6).Resolve(out t7);
        }

        public IAspectResolver<ITransientDependency> Resolve<T1, T2, T3, T4, T5, T6, T7, T8>(out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, out T7 t7, out T8 t8)
            where T1 : ITransientDependency
            where T2 : ITransientDependency
            where T3 : ITransientDependency
            where T4 : ITransientDependency
            where T5 : ITransientDependency
            where T6 : ITransientDependency
            where T7 : ITransientDependency
            where T8 : ITransientDependency
        {
            return Resolve(out t1).Resolve(out t2).Resolve(out t3).Resolve(out t4).Resolve(out t5).Resolve(out t6).Resolve(out t7).Resolve(out t8);
        }
    }
}
