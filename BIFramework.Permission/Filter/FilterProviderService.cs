using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BIStudio.Framework;

namespace BIStudio.Framework.Permission
{
    /// <summary>
    /// 查找过滤器
    /// </summary>
    public class FilterProviderService
    {
        public readonly static FilterProviderService Default = new FilterProviderService();

        private ConcurrentDictionary<string, IFilterProvider> _filters = new ConcurrentDictionary<string, IFilterProvider>();
        private FilterProviderService()
        {
            CFConfig.Default.ScanAttributes<FilterProviderAttribute>((item, attr) =>
            {
                dynamic validator = Activator.CreateInstance(item);
                _filters.TryAdd(attr.FilterOperation, validator);
            });
        }
        
        public List<string> GetFilterOperations()
        {
            return _filters.Keys.ToList();
        }

        public IFilterProvider GetFilter(string filterOperation)
        {
            IFilterProvider validator;
            if (_filters.TryGetValue(filterOperation, out validator))
                return validator;
            return new BasicFilterProvider();
        }
    }
}
