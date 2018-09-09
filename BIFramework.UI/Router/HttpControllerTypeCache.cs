using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace BIStudio.Framework.UI
{
    internal sealed class HttpControllerTypeCache
    {
        private readonly Lazy<Dictionary<string, ILookup<string, Type>>> _cache;
        private readonly HttpConfiguration _configuration;

        public HttpControllerTypeCache(HttpConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            this._configuration = configuration;
            this._cache = new Lazy<Dictionary<string, ILookup<string, Type>>>(new Func<Dictionary<string, ILookup<string, Type>>>(this.InitializeCache));
        }

        private Dictionary<string, ILookup<string, Type>> InitializeCache()
        {
            IAssembliesResolver assembliesResolver = this._configuration.Services.GetAssembliesResolver();
            return this._configuration.Services.GetHttpControllerTypeResolver()
                                               .GetControllerTypes(assembliesResolver)
                                               .GroupBy(t => t.Namespace.Substring(t.Namespace.LastIndexOf(Type.Delimiter) + 1) + "." + t.Name.Substring(0, t.Name.Length - DefaultHttpControllerSelector.ControllerSuffix.Length), StringComparer.OrdinalIgnoreCase)
                                               .ToDictionary(g => g.Key, g => g.ToLookup<Type, string>(t => (t.Namespace ?? string.Empty), StringComparer.OrdinalIgnoreCase), StringComparer.OrdinalIgnoreCase);
        }

        internal Dictionary<string, ILookup<string, Type>> Cache { get { return this._cache.Value; } }
    }
}
