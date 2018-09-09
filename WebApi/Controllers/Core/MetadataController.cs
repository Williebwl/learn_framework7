using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace WebApi.Controllers.Core
{
    public class MetadataController : ApiController
    {
        protected IDictionary<string, HttpControllerDescriptor> _controllers = null;

        public MetadataController() { if (_controllers == null) _controllers = (GlobalConfiguration.Configuration.Services.GetService(typeof(IHttpControllerSelector)) as IHttpControllerSelector)?.GetControllerMapping(); }

        public ICollection<string> GetControllers() => _controllers?.Keys;

        public IList<string> GetActions([FromUri]string controller)
        {
            var q = from d in _controllers?[controller].ControllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    let name = d.Name.ToLower()
                    let attrs = d.GetCustomAttributes(true)
                    where !attrs.Any(a => a is NonActionAttribute) &&
                          ((name.StartsWith("get") && !name.StartsWith("get_") && !name.Equals("gethashcode") && !name.Equals("gettype")) ||
                          name.StartsWith("post") ||
                          name.StartsWith("put") ||
                          name.StartsWith("delete") ||
                          attrs.Any(a => a is IActionHttpMethodProvider))
                    select d.Name;

            return q.Distinct().ToArray();
        }

        public ActionModel[] GetAction([FromUri]string controller, [FromUri]string action)
        {
            var controllerType = _controllers?[controller].ControllerType;

            var q = from d in controllerType?.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    where d.Name.Equals(action)
                    select GetAction(controllerType, controller, action, d);

            return q.ToArray();
        }

        private ActionModel GetAction(Type controllerType, string controller, string action, MethodInfo method)
        {
            var param = method?.GetParameters().Select(d => new ParamsModel { Name = d.Name, Type = d.ParameterType.FullName, PostType = d.GetCustomAttributes().FirstOrDefault(a => a is ParameterBindingAttribute)?.ToString() ?? (typeof(IConvertible).IsAssignableFrom(d.ParameterType) ? string.Empty : ".FromBodyAttribute") }).ToArray();

            return GetLink(controllerType, method, controller, new ActionModel
            {
                Name = action,
                Controller = controllerType.FullName,
                PostMethod = GetPostMethod(method),
                RouteParams = param.Where(d => string.IsNullOrEmpty(d.PostType)).ToArray(),
                UrlParams = param.Where(d => d.PostType.EndsWith(".FromUriAttribute")).ToArray(),
                FromBody = param.LastOrDefault(d => d.PostType.EndsWith(".FromBodyAttribute"))
            });
        }

        private ActionModel GetLink(Type controllerType, MethodInfo method, string controller, ActionModel actionModel)
        {
            var link = method?.GetCustomAttributes().OfType<RouteAttribute>().OrderByDescending(d => d.Order).FirstOrDefault()?.Template;

            actionModel.Link = (controllerType?.GetCustomAttribute<RoutePrefixAttribute>()?.Prefix ?? (link ?? (controller.Replace('.', '/') + (!",post,put,delete,head,options,trace,get,".Contains("," + actionModel.Name.ToLower() + ",") ? "/" + actionModel.Name : string.Empty)))).TrimStart('/');

            if (string.IsNullOrEmpty(link))
            {
                if (actionModel.RouteParams.Any()) actionModel.Link += "/{" + string.Join("}/{", actionModel.RouteParams.Select(d => d.Name).ToArray()) + "}";

                if (actionModel.UrlParams.Any()) actionModel.Link += "?" + string.Join("&", actionModel.UrlParams.Select(d => d.Name + "=").ToArray());
            }

            return actionModel;
        }


        private string GetPostMethod(MethodInfo method)
        {
            var methods = method?.GetCustomAttributes().OfType<IActionHttpMethodProvider>().SelectMany(d => d.HttpMethods).Select(d => d.Method).Distinct().ToArray();

            if (methods.Any()) return string.Join(",", methods);
            else {
                var name = method.Name.ToLower();

                return name.StartsWith("post") ? "post" : name.StartsWith("put") ? "put" : name.StartsWith("delete") ? "delete" : name.StartsWith("head") ? "head" : name.StartsWith("options") ? "options" : name.StartsWith("trace") ? "trace" : "get";
            }
        }

        public struct ActionModel
        {
            public string Controller { get; set; }

            public string Name { get; set; }

            public string Link { get; set; }

            public string PostMethod { get; set; }

            public IList<ParamsModel> RouteParams { get; set; }

            public IList<ParamsModel> UrlParams { get; set; }

            public ParamsModel FromBody { get; set; }
        }

        public class ParamsModel
        {
            public string Type { get; set; }

            public string PostType { get; set; }

            public string Name { get; set; }
        }

    }
}
