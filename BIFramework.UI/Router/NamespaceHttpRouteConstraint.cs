using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace BIStudio.Framework.UI
{
    /// <summary>
    /// 智能匹配路由规则中的action和id参数
    /// </summary>
    public class NamespaceHttpRouteConstraint : HttpMethodConstraint, IRouteConstraint
    {
        public NamespaceHttpRouteConstraint() : base(
            HttpMethod.Get.Method,
            HttpMethod.Post.Method,
            HttpMethod.Put.Method,
            HttpMethod.Delete.Method,
            HttpMethod.Head.Method,
            HttpMethod.Options.Method,
            HttpMethod.Trace.Method
            )
        {

        }
        private static Dictionary<string, string> defaultActions = new Dictionary<string, string>
        {
            { HttpMethod.Get.Method, "Get" },
            { HttpMethod.Post.Method, "Post" },
            { HttpMethod.Put.Method, "Put" },
            { HttpMethod.Delete.Method, "Delete"},
        };
        private static Regex isID = new Regex(@"^[\d,]+$", RegexOptions.Compiled);
        protected override bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (routeDirection == RouteDirection.IncomingRequest && defaultActions.ContainsKey(httpContext.Request.HttpMethod) &&
                parameterName == "action" && values.ContainsKey("action") && values.ContainsKey("id"))
            {
                var action = values["action"];
                var id = values["id"];
                if (action == RouteParameter.Optional && id == RouteParameter.Optional)
                {
                    values["action"] = defaultActions[httpContext.Request.HttpMethod];
                }
                else if (action != RouteParameter.Optional && id == RouteParameter.Optional && isID.IsMatch(action.ToString()))
                {
                    values["id"] = values["action"];
                    values["action"] = defaultActions[httpContext.Request.HttpMethod];
                }
            }
            return base.Match(httpContext, route, parameterName, values, routeDirection);
        }
    }
}
