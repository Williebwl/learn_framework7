using System;

namespace BIStudio.Framework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
    public class IocAttribute:Attribute
    {
        public IocAttribute() { }
        public IocAttribute(Type registerType) { this.RegisterType = registerType; }

        /// <summary>
        /// 依赖注入的类型
        /// </summary>
        public Type RegisterType { get; set; }
        
        /// <summary>
        /// 注册条件
        /// </summary>
        public RegisterCondition Condition { get; set; }

        /// <summary>
        /// aop类型
        /// </summary>
        public AopType AopType { get; set; }

        /// <summary>
        /// 生命周期类型
        /// </summary>
        public LifetimeManagerType LifetimeManagerType { get; set; }
    }

    [Flags]
    public enum RegisterCondition
    {
        IsRequire=1,
    }
    /// <summary>
    /// 拦截器类型
    /// </summary>
    public enum AopType
    {
        None,
        /// <summary>
        /// 对virtual函数进行拦截。缺点是如果被拦截类型没有virtual函数则无法拦截，这个时候如果类型实现了某个特定接口可以改用
        /// </summary>
        VirtualMethodInterceptor,
        /// <summary>
        /// 只能对一个接口做拦截，好处是只要目标类型实现了指定接口就可以拦截。
        /// </summary>
        InterfaceInterceptor,
        /// <summary>
        /// 代理实现基于.NET Remoting技术，它可拦截对象的所有函数。缺点是被拦截类型必须派生于MarshalByRefObject。
        /// </summary>
        TransparentProxyInterceptor,
        //这里可以添加自定义
    }
    /// <summary>
    /// 生命周期
    /// </summary>
    public enum LifetimeManagerType
    {
        /// <summary>
        /// 短暂的
        /// </summary>
        Transient,
        /// <summary>
        /// 容器受控制的
        /// </summary>
        ContainerControlled,
        /// <summary>
        /// 分层的
        /// </summary>
        Hierarchica,
        /// <summary>
        /// 外形上
        /// </summary>
        Externally,
        /// <summary>
        ///  每人
        /// </summary>
        PerThread,
        /// <summary>
        /// 坚决
        /// </summary>
        PerResolve,
    }
}
