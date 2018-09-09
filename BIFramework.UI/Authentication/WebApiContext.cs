using BIStudio.Framework.Auth;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Domain.EntityFramework;
using BIStudio.Framework.Institution;
using BIStudio.Framework.Permission;
using BIStudio.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BIStudio.Framework.UI
{
    public sealed class WebApiContext : DomainService, IDomainService, IContext
    {
        private IAuthorizationService authorizationService;
        private IRepository<SYSAccount> accountRP;
        private IRepository<SYSPassport> passportRP;
        private IGroupService groupService;
        private IPolicyService policyService;

        public WebApiContext(HttpRequestMessage request, SYSAccessToken token)
        {
            try
            {
                var sysToken = authorizationService.VerifyAccess(new SYSVerifyAccessTokenDTO(token.access_token, token.scope));
                var sysAccount = accountRP.Get(item => item.SystemID == token.system_id && item.PassportID == token.uid);
                if (sysAccount != null && sysAccount.ID != null)
                {
                    var sysPassport = passportRP.Get(sysAccount.PassportID ?? 0);
                    if (sysPassport != null && sysPassport.ID != null)
                    {
                        var groups = groupService.GetUserGroups(sysAccount.ID.Value).Select(d => d.GroupCode).ToArray();
                        var operations = policyService.GetOperations(sysAccount.ID.Value, DateTime.Now, HttpContext.Current.Request.UserHostAddress).Select(d => d.OperationCode).ToArray();
                        this.User = new ContextUser(token.system_id, token.uid.Value, HttpContext.Current.Request.UserHostAddress, sysPassport.LoginName, sysAccount.RealName, groups, operations);
                        this.Server = new ContextServer();
                    }
                }
            }
            catch (DefinedException ex)
            {
                this.User = new ContextUser();
                this.Server = new ContextServer();
                this.Page = new ContextPage(request.RequestUri);
            }
            catch (Exception ex)
            {

            }
        }
        public IContextUser User
        {
            get;
            private set;
        }
        public IContextServer Server
        {
            get;
            private set;
        }
        public IContextPage Page
        {
            get;
            private set;
        }
    }
}
