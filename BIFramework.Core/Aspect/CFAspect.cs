using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace BIStudio.Framework
{
    /// <summary>
    /// IOC容器
    /// </summary>
    public static class CFAspect
    {
        #region 属性

        static CFAspect()
        {
            //创建容器  
            Current = new UnityContainer();
            //添加unity扩展,扩展类型是一个拦截器
            Current.AddNewExtension<Interception>();
        }

        /// <summary>
        ///     Get the current configured container
        /// </summary>
        /// <returns>Configured container</returns>
        private static IUnityContainer Current { get; set; }

        #endregion

        #region 注册类型

        /// <summary>
        /// 注册类型
        /// </summary>
        public static void RegisterType(Type target, Type source)
        {
            Current.RegisterType(target, source);
        }
        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="source">源</param>
        /// <param name="lifetimeManager">生命周期</param>
        /// <param name="injectionMembers">注入aop方法</param>
        private static void RegisterType(Type target, Type source, LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
        {
            if (lifetimeManager == null && injectionMembers == null)
                Current.RegisterType(target, source);
            else if (lifetimeManager == null)
                Current.RegisterType(target, source, injectionMembers);
            else if (injectionMembers == null)
                Current.RegisterType(target, source, lifetimeManager);
            else
                Current.RegisterType(target, source, lifetimeManager, injectionMembers);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        public static void RegisterType<TTarget, TSource>() where TSource : TTarget
        {
            Current.RegisterType<TTarget, TSource>();
        }
        /// <summary>
        /// 注册类型
        /// </summary>
        public static void RegisterType<TTarget, TSource>(string name) where TSource : TTarget
        {
            Current.RegisterType<TTarget, TSource>(name);
        }
        /// <summary>
        /// 注册类型
        /// </summary>
        private static void RegisterType<TTarget, TSource>(LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers) where TSource : TTarget
        {
            if (lifetimeManager == null && injectionMembers == null)
                Current.RegisterType<TTarget, TSource>();
            else if (lifetimeManager == null)
                Current.RegisterType<TTarget, TSource>(injectionMembers);
            else if (injectionMembers == null)
                Current.RegisterType<TTarget, TSource>(lifetimeManager);
            else
                Current.RegisterType<TTarget, TSource>(lifetimeManager, injectionMembers);
        }

        #endregion

        #region 创建实例

        /// <summary>
        /// 定义构造函数
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IAspectResolver Constructor(params dynamic[] values)
        {
            return new AspectResolver(Current, values);
        }

        /// <summary>
        /// 定义构造函数
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IAspectResolver<T> Constructor<T>(params dynamic[] values)
        {
            return new AspectResolver<T>(Current, values);
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="target"></param>
        /// <param name="values">构造函数值</param>
        /// <returns></returns>
        public static object Resolve(Type target, params dynamic[] values)
        {
            return Constructor(values).Resolve(target);
        }

        /// <summary>
        /// 创建泛型实例
        /// </summary>
        /// <param name="values">构造函数值</param>
        /// <returns></returns>
        public static T Resolve<T>(params dynamic[] values)
        {
            return Constructor(values).Resolve<T>();
        }


        #endregion

        #region 注册AOP

        /// <summary>
        /// 扫描并注册AOP容器
        /// </summary>
        /// <param name="config"></param>
        internal static void RegisterAopContainer(CFConfig config)
        {
            config.ScanAttributes<IocAttribute>((type, attribute) =>
            {
                CFAspect.RegisterType(attribute.RegisterType, type, CFAspect.GetLifetimeManager(attribute.LifetimeManagerType), CFAspect.GetInjectionMembers(attribute.AopType, attribute.RegisterType));
            });
        }

        /// <summary>
        /// 获取生命周期
        /// </summary>
        /// <param name="lifetimeManagerType"></param>
        /// <returns></returns>
        private static LifetimeManager GetLifetimeManager(LifetimeManagerType lifetimeManagerType)
        {
            switch (lifetimeManagerType)
            {
                case LifetimeManagerType.Transient:
                    return new TransientLifetimeManager();
                case LifetimeManagerType.ContainerControlled:
                    return new ContainerControlledLifetimeManager();
                case LifetimeManagerType.Hierarchica:
                    return new HierarchicalLifetimeManager();
                case LifetimeManagerType.Externally:
                    return new ExternallyControlledLifetimeManager();
                case LifetimeManagerType.PerThread:
                    return new PerThreadLifetimeManager();
                case LifetimeManagerType.PerResolve:
                    return new PerResolveLifetimeManager();
                default:
                    return new TransientLifetimeManager();
            }
        }

        /// <summary>
        /// 注入aop方法
        /// </summary>
        /// <param name="aopType"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static InjectionMember[] GetInjectionMembers(AopType aopType, Type type)
        {
            var members = new List<InjectionMember>();
            switch (aopType)
            {
                case AopType.VirtualMethodInterceptor:
                    members.Add(new Interceptor<VirtualMethodInterceptor>());
                    break;
                case AopType.InterfaceInterceptor:
                    members.Add(new Interceptor<InterfaceInterceptor>());
                    break;
                case AopType.TransparentProxyInterceptor:
                    members.Add(new Interceptor<TransparentProxyInterceptor>());
                    break;
            }
            members.AddRange(type.GetCustomAttributes()
                   .Where(item => item.GetType().IsSubclassOf(typeof(AopAttribute)))
                   .Cast<AopAttribute>()
                   .Select(item => new InterceptionBehavior(item)));
            return members.ToArray();
        }

        #endregion

    }
}
