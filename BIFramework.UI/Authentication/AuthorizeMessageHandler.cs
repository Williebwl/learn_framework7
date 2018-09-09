using System;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using Newtonsoft.Json;
using System.Text;

namespace BIStudio.Framework.UI
{
    using BIStudio.Framework.Auth;
    public class AuthorizeMessageHandler : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var reqContext = request.GetRequestContext();

            if (reqContext == null) throw new ArgumentException("Request_RequestContextMustNotBeNull", "");

            if (reqContext.Principal == null || !(reqContext.Principal is IContextUser)) reqContext.Principal = GetPrincipal(request);

            return await base.SendAsync(request, cancellationToken);
        }

        protected virtual IPrincipal GetPrincipal(HttpRequestMessage request)
        {
            if (request == null || request.Headers == null || request.Headers.Authorization == null ||
                !"Basic".Equals(request.Headers.Authorization.Scheme) ||
                string.IsNullOrEmpty((request.Headers.Authorization.Parameter ?? string.Empty).Trim())) return null;

            var token = JsonConvert.DeserializeObject<SYSAccessToken>(Encoding.UTF8.GetString(Convert.FromBase64String(request.Headers.Authorization.Parameter)));

            CFContext.SetCurrent(new WebApiContext(request, token));

            return CFContext.User;
        }
    }
}
