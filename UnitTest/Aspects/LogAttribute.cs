using System;
using System.Reflection;

namespace BIFramework.Test.Aspects
{
    public class LogAttribute : Attribute
    {
        public interface ILogger
        {
            void Info(string text);

            void Error(string text);
        }

        public static ILogger Logger;

        public bool NetAspectAttribute = true;

        private static string ComputeLog(string eventName, MethodInfo method)
        {
            return string.Format("{0} {1} ", eventName, method.Name);
        }

        public void BeforeMethod(MethodInfo method)
        {
            Logger.Info(ComputeLog("On enter :", method));
        }

        public void AfterMethod(MethodInfo method)
        {
            Logger.Info(ComputeLog("On exit :", method));
        }

        public void OnExceptionMethod(MethodInfo method, Exception exception)
        {
            Logger.Error(ComputeLog(exception.Message, method));
        }
    }

}