using System;

namespace BIStudio.Framework.Permission
{
    /// <summary>
    /// 表示当前类是过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Field, AllowMultiple = true)]
    public class FilterProviderAttribute : Attribute
    {
        public FilterProviderAttribute(EnumFilterOperation filterOperation)
        {
            FilterOperation = filterOperation.ToString();
        }
        public FilterProviderAttribute(string filterOperation)
        {
            FilterOperation = filterOperation;
        }

        public string FilterOperation { get; set; }
    }
}
