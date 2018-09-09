using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace BIStudio.Framework
{
    internal class OrderedParametersOverride : ResolverOverride
    {
        private readonly Queue<InjectionParameterValue> parameterValues;

        public OrderedParametersOverride(IUnityContainer current, Type target, params dynamic[] parameterValues)
        {
            this.parameterValues = new Queue<InjectionParameterValue>();
            if (parameterValues == null || parameterValues.Length == 0)
            {
                //使用构造函数默认值
                var registration = current.Registrations.FirstOrDefault(item => item.RegisteredType == target && item.Name == null);
                if (registration == null)
                    throw CFException.Create(OperateResult.NotFound, "未找到 " + target.FullName+" 的实现类");
                var constructor = registration.MappedToType.GetTypeInfo().DeclaredConstructors.First();
                constructor.GetParameters().Select(parameter => parameter.DefaultValue).ForEach(parameterValue =>
                {
                    this.parameterValues.Enqueue(parameterValue == null ? new InjectionParameter(typeof(object), null) : InjectionParameterValue.ToParameter(parameterValue));
                });
            }
            else
            {
                //使用自定义构造函数
                parameterValues.ForEach(parameterValue =>
                {
                    this.parameterValues.Enqueue(parameterValue == null ? new InjectionParameter(typeof(object), null) : InjectionParameterValue.ToParameter(parameterValue));
                });
            }
        }

        public override IDependencyResolverPolicy GetResolver(IBuilderContext context, Type dependencyType)
        {
            if (parameterValues.Count < 1)
                return null;

            var value = this.parameterValues.Dequeue();
            return value.GetResolverPolicy(dependencyType);
        }

    }
}
