using System;
using System.Text;
using System.Web.Http;
using BIStudio.Framework.Auth;
using BIStudio.Framework.UI;
using Newtonsoft.Json;

namespace WebApi.Controllers.Auth
{

    public class LoginController : ApplicationService
    {
        private const string ApiKey = "PAAS_Master";
        private const string Secret = "44678314ba0efa0c";

        private IAuthorizationService auth;

        [AllowAnonymous, HttpPost]
        public virtual string Login([FromBody]LoginVM vm)
        {
            if (vm == null) throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);

            var authorize = auth.Authorize(new SYSAuthorizeDTO(ApiKey));
            auth.AuthorizeLogin(new SYSAuthorizeLoginDTO(authorize.code, vm.LoginName, vm.Password));
            var token = auth.AccessToken(new SYSAccessTokenDTO(ApiKey, Secret, authorize.code));

            return "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(token)));
        }
    }
}