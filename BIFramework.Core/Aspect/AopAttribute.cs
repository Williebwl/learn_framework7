using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;

namespace BIStudio.Framework
{
    /// <summary>
    /// 拦截器
    /// </summary>
    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Class|AttributeTargets.Interface)]
    public abstract class AopAttribute : HandlerAttribute, ICallHandler, IInterceptionBehavior
    {
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return this;
        }
        /// <summary>
        /// 获取当前行为需要拦截的对象类型接口。
        /// </summary>
        /// <returns>所有需要拦截的对象类型接口。</returns>
        public System.Collections.Generic.IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        /// <summary>
        /// 调用之后的实现逻辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual void OnAfter(IMethodInvocation input)
        {
            
        }


        /// <summary>
        /// 调用之前的实现逻辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual void OnBefore(IMethodInvocation input)
        {
            
        }

        /// <summary>
        /// 调用出现异常的实现逻辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual void OnException(IMethodInvocation input, Exception ex)
        {
            throw ex;
        }

        /// <summary>
        /// 接口注入时候的拦截方法(通过实现此方法来拦截调用并执行所需的拦截行为。)
        /// </summary>
        /// <param name="input">调用拦截目标时的输入信息。</param>
        /// <param name="nextMethod">通过行为链来获取下一个拦截行为的委托。</param>
        /// <returns>从拦截目标获得的返回信息。</returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate nextMethod)
        {
            OnBefore(input);
            IMethodReturn  result = nextMethod()(input, nextMethod);
            #region 异常部分
            if (result.Exception != null)
            {
                OnException(input, result.Exception);
            }
            else
            {
                OnAfter(input);
            }
            #endregion
          
            return result;
        }

        /// <summary>
        /// 虚方法注入的拦截方法
        /// </summary>
        /// <param name="input"></param>
        /// <param name="nextMethod"></param>
        /// <returns></returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate nextMethod)
        {
            OnBefore(input);
            IMethodReturn result = nextMethod()(input, nextMethod);
            #region 异常部分
            if (result.Exception != null)
            {
                OnException(input, result.Exception);
            }
            else
            {
                OnAfter(input);
            }
            #endregion

            return result;
        }
        /// <summary>
        /// 该值表示当前拦截行为被调用时，是否真的需要执行某些操作。
        /// </summary>
        public bool WillExecute
        {
            get { return true; }
        }
    }
}
