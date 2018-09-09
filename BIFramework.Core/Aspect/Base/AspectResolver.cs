using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework
{
    /// <summary>
    /// 解析器
    /// </summary>
    public interface IAspectResolver
    {
        object Resolve(Type target);
        T Resolve<T>();
        IAspectResolver Resolve<T>(out T instance);
        IAspectResolver Resolve<T1, T2>(out T1 t1, out T2 t2);
        IAspectResolver Resolve<T1, T2, T3>(out T1 t1, out T2 t2, out T3 t3);
        IAspectResolver Resolve<T1, T2, T3, T4>(out T1 t1, out T2 t2, out T3 t3, out T4 t4);
        IAspectResolver Resolve<T1, T2, T3, T4, T5>(out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5);
        IAspectResolver Resolve<T1, T2, T3, T4, T5, T6>(out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6);
        IAspectResolver Resolve<T1, T2, T3, T4, T5, T6, T7>(out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, out T7 t7);
        IAspectResolver Resolve<T1, T2, T3, T4, T5, T6, T7, T8>(out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, out T7 t7, out T8 t8);
    }
    /// <summary>
    /// 解析器
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    public interface IAspectResolver<TInterface>
    {
        TInterface Resolve(Type target);
        T Resolve<T>() 
            where T : TInterface;
        IAspectResolver<TInterface> Resolve<T>(out T instance) 
            where T : TInterface;
        IAspectResolver<TInterface> Resolve<T1, T2>(out T1 t1, out T2 t2)
            where T1 : TInterface
            where T2 : TInterface;
        IAspectResolver<TInterface> Resolve<T1, T2, T3>(out T1 t1, out T2 t2, out T3 t3)
            where T1 : TInterface
            where T2 : TInterface
            where T3 : TInterface;
        IAspectResolver<TInterface> Resolve<T1, T2, T3, T4>(out T1 t1, out T2 t2, out T3 t3, out T4 t4)
            where T1 : TInterface
            where T2 : TInterface
            where T3 : TInterface
            where T4 : TInterface;
        IAspectResolver<TInterface> Resolve<T1, T2, T3, T4, T5>(out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5)
            where T1 : TInterface
            where T2 : TInterface
            where T3 : TInterface
            where T4 : TInterface
            where T5 : TInterface;
        IAspectResolver<TInterface> Resolve<T1, T2, T3, T4, T5, T6>(out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6)
            where T1 : TInterface
            where T2 : TInterface
            where T3 : TInterface
            where T4 : TInterface
            where T5 : TInterface
            where T6 : TInterface;
        IAspectResolver<TInterface> Resolve<T1, T2, T3, T4, T5, T6, T7>(out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, out T7 t7)
            where T1 : TInterface
            where T2 : TInterface
            where T3 : TInterface
            where T4 : TInterface
            where T5 : TInterface
            where T6 : TInterface
            where T7 : TInterface;
        IAspectResolver<TInterface> Resolve<T1, T2, T3, T4, T5, T6, T7, T8>(out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, out T7 t7, out T8 t8)
            where T1 : TInterface
            where T2 : TInterface
            where T3 : TInterface
            where T4 : TInterface
            where T5 : TInterface
            where T6 : TInterface
            where T7 : TInterface
            where T8 : TInterface;
    }

    internal sealed class AspectResolver : IAspectResolver
    {
        private IUnityContainer current;
        private dynamic[] values;
        internal AspectResolver(IUnityContainer current, params dynamic[] values)
        {
            this.current = current;
            this.values = values;
        }
        public object Resolve(Type target)
        {
            if (this.current.IsRegistered(target))
                return this.current.Resolve(target, new OrderedParametersOverride(current, target, values));
            else if (target.IsClass)
                return Activator.CreateInstance(target);
            else
                return null;
        }
        public T Resolve<T>()
        {
            if (this.current.IsRegistered<T>())
                return this.current.Resolve<T>(new OrderedParametersOverride(current, typeof(T), this.values));
            else if (typeof(T).IsClass)
                return Activator.CreateInstance<T>();
            else
                return default(T);
        }
        public IAspectResolver Resolve<T>(out T instance)
        {
            instance = Resolve<T>();
            return this;
        }
        public IAspectResolver Resolve<T1, T2>(out T1 t1, out T2 t2)
        {
            return Resolve(out t1).Resolve(out t2);
        }
        public IAspectResolver Resolve<T1, T2, T3>(out T1 t1, out T2 t2, out T3 t3)
        {
            return Resolve(out t1).Resolve(out t2).Resolve(out t3);
        }
        public IAspectResolver Resolve<T1, T2, T3, T4>(out T1 t1, out T2 t2, out T3 t3, out T4 t4)
        {
            return Resolve(out t1).Resolve(out t2).Resolve(out t3).Resolve(out t4);
        }
        public IAspectResolver Resolve<T1, T2, T3, T4, T5>(out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5)
        {
            return Resolve(out t1).Resolve(out t2).Resolve(out t3).Resolve(out t4).Resolve(out t5);
        }
        public IAspectResolver Resolve<T1, T2, T3, T4, T5, T6>(out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6)
        {
            return Resolve(out t1).Resolve(out t2).Resolve(out t3).Resolve(out t4).Resolve(out t5).Resolve(out t6);
        }
        public IAspectResolver Resolve<T1, T2, T3, T4, T5, T6, T7>(out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, out T7 t7)
        {
            return Resolve(out t1).Resolve(out t2).Resolve(out t3).Resolve(out t4).Resolve(out t5).Resolve(out t6).Resolve(out t7);
        }
        public IAspectResolver Resolve<T1, T2, T3, T4, T5, T6, T7, T8>(out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, out T7 t7, out T8 t8)
        {
            return Resolve(out t1).Resolve(out t2).Resolve(out t3).Resolve(out t4).Resolve(out t5).Resolve(out t6).Resolve(out t7).Resolve(out t8);
        }
    }
    internal sealed class AspectResolver<TInterface> : IAspectResolver<TInterface>
    {
        private IUnityContainer current;
        private dynamic[] values;
        internal AspectResolver(IUnityContainer current, params dynamic[] values)
        {
            this.current = current;
            this.values = values;
        }

        public TInterface Resolve(Type target)
        {
            if (this.current.IsRegistered(target))
                return (TInterface)this.current.Resolve(target, new OrderedParametersOverride(current, target, values));
            else if (target.IsClass)
                return (TInterface)Activator.CreateInstance(target);
            else
                return default(TInterface);
        }

        public T Resolve<T>() where T : TInterface
        {
            if (this.current.IsRegistered<T>())
                return this.current.Resolve<T>(new OrderedParametersOverride(current, typeof(T), this.values));
            else if (typeof(T).IsClass)
                return Activator.CreateInstance<T>();
            else
                return default(T);
        }

        public IAspectResolver<TInterface> Resolve<T>(out T instance) where T : TInterface
        {
            instance = Resolve<T>();
            return this;
        }

        public IAspectResolver<TInterface> Resolve<T1, T2>(out T1 t1, out T2 t2)
            where T1 : TInterface
            where T2 : TInterface
        {
            return Resolve(out t1).Resolve(out t2);
        }

        public IAspectResolver<TInterface> Resolve<T1, T2, T3>(out T1 t1, out T2 t2, out T3 t3)
            where T1 : TInterface
            where T2 : TInterface
            where T3 : TInterface
        {
            return Resolve(out t1).Resolve(out t2).Resolve(out t3);
        }

        public IAspectResolver<TInterface> Resolve<T1, T2, T3, T4>(out T1 t1, out T2 t2, out T3 t3, out T4 t4)
            where T1 : TInterface
            where T2 : TInterface
            where T3 : TInterface
            where T4 : TInterface
        {
            return Resolve(out t1).Resolve(out t2).Resolve(out t3).Resolve(out t4);
        }

        public IAspectResolver<TInterface> Resolve<T1, T2, T3, T4, T5>(out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5)
            where T1 : TInterface
            where T2 : TInterface
            where T3 : TInterface
            where T4 : TInterface
            where T5 : TInterface
        {
            return Resolve(out t1).Resolve(out t2).Resolve(out t3).Resolve(out t4).Resolve(out t5);
        }

        public IAspectResolver<TInterface> Resolve<T1, T2, T3, T4, T5, T6>(out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6)
            where T1 : TInterface
            where T2 : TInterface
            where T3 : TInterface
            where T4 : TInterface
            where T5 : TInterface
            where T6 : TInterface
        {
            return Resolve(out t1).Resolve(out t2).Resolve(out t3).Resolve(out t4).Resolve(out t5).Resolve(out t6);
        }

        public IAspectResolver<TInterface> Resolve<T1, T2, T3, T4, T5, T6, T7>(out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, out T7 t7)
            where T1 : TInterface
            where T2 : TInterface
            where T3 : TInterface
            where T4 : TInterface
            where T5 : TInterface
            where T6 : TInterface
            where T7 : TInterface
        {
            return Resolve(out t1).Resolve(out t2).Resolve(out t3).Resolve(out t4).Resolve(out t5).Resolve(out t6).Resolve(out t7);
        }

        public IAspectResolver<TInterface> Resolve<T1, T2, T3, T4, T5, T6, T7, T8>(out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, out T7 t7, out T8 t8)
            where T1 : TInterface
            where T2 : TInterface
            where T3 : TInterface
            where T4 : TInterface
            where T5 : TInterface
            where T6 : TInterface
            where T7 : TInterface
            where T8 : TInterface
        {
            return Resolve(out t1).Resolve(out t2).Resolve(out t3).Resolve(out t4).Resolve(out t5).Resolve(out t6).Resolve(out t7).Resolve(out t8);
        }
    }
}
