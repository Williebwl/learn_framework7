using System.Web.Http;
using Newtonsoft.Json;
using BIStudio.Framework.UI;
using System.Web.Http.Cors;
using System.Net.Http.Headers;
using System.Web.Http.Dispatcher;
using System.Web.Routing;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            // Web API 配置和服务
            var jsonSettings = config.Formatters.JsonFormatter.SerializerSettings;
            jsonSettings.NullValueHandling = NullValueHandling.Ignore;
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            //jsonSet.PreserveReferencesHandling = PreserveReferencesHandling.All;
            //jsonSet.Formatting = Formatting.Indented;

            //config.SuppressHostPrincipal();

            config.MessageHandlers.Add(new AuthorizeMessageHandler());

            // Web API 路由
            config.MapHttpAttributeRoutes();
            //todo
            //config.Filters.Add(new AppAuthFilter());
            config.Routes.MapHttpRoute("DefaultApi",
                "{namespace}/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional, action = RouteParameter.Optional },
                new { action = new NamespaceHttpRouteConstraint() });
            config.Services.Replace(typeof(IHttpControllerSelector), new NamespaceHttpControllerSelector(config));
            //config.Filters.Add(new DefinedExceptionFilterAttribute());
        }
    }
}
