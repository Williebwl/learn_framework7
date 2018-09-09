using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace BIStudio.Framework.UI
{
    /// <summary>
    /// 指定用于验证请求的 System.Security.Principal.IPrincipal 的授权筛选器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    {
        #region 内部属性

        private static readonly string[] _emptyArray = new string[0];
        private string _roles;
        private string[] _rolesSplit = _emptyArray;
        private string _users;
        private string[] _usersSplit = _emptyArray;
        private bool _noPermission;

        internal static string[] SplitString(string original)
        {
            if (string.IsNullOrEmpty(original)) return _emptyArray;

            return (from piece in original.Split(',')
                    let trimmed = piece.Trim()
                    where !string.IsNullOrEmpty(trimmed)
                    select trimmed).ToArray();
        }

        /// <summary>
        /// 获取或设置授权角色
        /// </summary>
        public new string Roles
        {
            get { return (this._roles ?? string.Empty); }
            set
            {
                this._roles = value;
                this._rolesSplit = SplitString(value);
            }
        }

        /// <summary>
        /// 获取或设置授权用户
        /// </summary>
        public new string Users
        {
            get { return (this._users ?? string.Empty); }
            set
            {
                this._users = value;
                this._usersSplit = SplitString(value);
            }
        }

        /// <summary>
        /// 指定当前操作代码
        /// </summary>
        public string Operation
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// 处理授权失败的请求
        /// </summary>
        /// <param name="actionContext">上下文</param>
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (actionContext == null) throw new ArgumentNullException("actionContext");

            if (_noPermission) actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.MethodNotAllowed, "RequestNotAuthorized");
            else base.HandleUnauthorizedRequest(actionContext);
        }

        /// <summary>
        /// 指示指定的控件是否已获得授权
        /// </summary>
        /// <param name="actionContext">上下文</param>
        /// <returns>如果控件已获得授权，则为 true；否则为 false</returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (actionContext == null) throw new ArgumentNullException("actionContext");

            IPrincipal principal = actionContext.ControllerContext.RequestContext.Principal;

            if (((principal == null) || (principal.Identity == null)) || !principal.Identity.IsAuthenticated) return false;

            //检查用户权限
            if (this._usersSplit.Any() && !this._usersSplit.Contains(principal.Identity.Name, StringComparer.OrdinalIgnoreCase)) return !(_noPermission = true);
            //检查用户组权限
            if (this._rolesSplit.Any() && !this._rolesSplit.Any(principal.IsInRole)) return !(_noPermission = true);
            //检查操作权限
            if (!string.IsNullOrEmpty(this.Operation) && !CFContext.User.Identity.HasPermission(this.Operation)) return !(_noPermission = true);

            return true;
        }
    }
}
