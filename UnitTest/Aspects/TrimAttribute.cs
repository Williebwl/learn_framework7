using System;
using System.Reflection;

namespace BIFramework.Test.Aspects
{
    public class TrimAttribute : Attribute
    {
        public bool NetAspectAttribute = true;

        public void BeforeMethodForParameter(ref string parameterValue)
        {
            if (string.IsNullOrEmpty(parameterValue))
                return;
            parameterValue = parameterValue.Trim();
        }
    }

}