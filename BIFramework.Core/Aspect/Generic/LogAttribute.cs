using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework
{
    public class LogAttribute : AopAttribute
    {
        protected override void OnAfter(IMethodInvocation input)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var inp in input.Inputs)
                sb.Append(sb.Length > 0 ? "," + inp : inp);
            Console.WriteLine("After " + input.Target.GetType().FullName + "." + input.MethodBase.Name + "(" + sb.ToString() + ")");
        }
        protected override void OnBefore(IMethodInvocation input)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var inp in input.Inputs)
                sb.Append(sb.Length > 0 ? "," + inp : inp);
            Console.WriteLine("Before " + input.Target.GetType().FullName + "." + input.MethodBase.Name + "(" + sb.ToString() + ")");
        }
    }
}
