using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;

namespace BIStudio.Framework.UI
{
    public class NamespaceHttpControllerSelector : IHttpControllerSelector
    {
        private const string NamespaceKey = "namespace";
        private const string ControllerKey = "controller";

        private readonly HttpConfiguration _configuration;
        private readonly Lazy<ConcurrentDictionary<string, HttpControllerDescriptor>> _controllerInfoCache;
        private readonly HttpControllerTypeCache _controllerTypeCache;
        private readonly HashSet<string> _duplicates;

        public NamespaceHttpControllerSelector(HttpConfiguration config)
        {
            _configuration = config;
            _duplicates = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _controllerInfoCache = new Lazy<ConcurrentDictionary<string, HttpControllerDescriptor>>(InitializeControllerInfoCache);
            _controllerTypeCache = new HttpControllerTypeCache(_configuration);
        }

        private ConcurrentDictionary<string, HttpControllerDescriptor> InitializeControllerInfoCache()
        {
            var dictionary = new ConcurrentDictionary<string, HttpControllerDescriptor>(StringComparer.OrdinalIgnoreCase);

            foreach (var pair in this._controllerTypeCache.Cache)
            {
                string key = pair.Key;
                foreach (var grouping in pair.Value)
                {
                    foreach (Type type in grouping)
                    {
                        if (dictionary.Keys.Contains(key))
                        {
                            _duplicates.Add(key);
                            break;
                        }
                        dictionary.TryAdd(key, new HttpControllerDescriptor(this._configuration, key, type));
                    }
                }
            }

            foreach (string str2 in _duplicates)
            {
                HttpControllerDescriptor descriptor;
                dictionary.TryRemove(str2, out descriptor);
            }
            return dictionary;
        }

        // Get a value from the route data, if present.
        private static T GetRouteVariable<T>(IHttpRouteData routeData, string name)
        {
            object result = null;
            if (routeData.Values.TryGetValue(name, out result))
            {
                return (T)result;
            }
            return default(T);
        }
        
        public HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            IHttpRouteData routeData = request.GetRouteData();
            if (routeData == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            // Get the namespace and controller variables from the route data.
            string namespaceName = GetRouteVariable<string>(routeData, NamespaceKey);
            if (namespaceName == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            string controllerName = GetRouteVariable<string>(routeData, ControllerKey);
            if (controllerName == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            // Find a matching controller.
            string key = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", namespaceName, controllerName);

            HttpControllerDescriptor controllerDescriptor;
            if (_controllerInfoCache.Value.TryGetValue(key, out controllerDescriptor))
            {
                return controllerDescriptor;
            }
            else if (_duplicates.Contains(key))
            {
                throw new HttpResponseException(
                    request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                    "Multiple controllers were found that match this request."));
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        public IDictionary<string, HttpControllerDescriptor> GetControllerMapping() => this._controllerInfoCache.Value.ToDictionary(c => c.Key, c => c.Value, StringComparer.OrdinalIgnoreCase);

    }
}
