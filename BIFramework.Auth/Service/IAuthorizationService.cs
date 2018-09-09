using BIStudio.Framework.Domain;
using BIStudio.Framework.Tenant;
using BIStudio.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Auth
{
    public interface IAuthorizationService : IDomainService
    {

        #region 账户

        /// <summary>
        /// 注册系统账号
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        SYSAccount AccountRegist(SYSAccountRegistDTO account);

        /// <summary>
        /// 删除系统账号
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="uId"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        void UnRegisterAccount(long systemId, string uId);

        /// <summary>
        /// 注册通行证
        /// </summary>
        /// <param name="passport"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        SYSPassport PassportRegist(SYSPassportRegistDTO passport);        
        /// <summary>
        /// 注销通行证
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        void PassportUnRegister(string loginName);

        /// <summary>
        /// 通行证绑定系统账号
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        void PassportLink(SYSPassportLinkDTO link);
        /// <summary>
        /// 通行证解绑系统账号
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        void PassportUnlink(SYSPassportLinkDTO link);
        #endregion
        
        #region 账户服务
        /// <summary>
        /// 更改通行证密码
        /// </summary>
        /// <param name="changePassword"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        void PassportChangePassword(SYSPassportChangePasswordDTO changePassword);

        /// <summary>
        /// 触发找回密码
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        SYSPassworkForgetDTO PassportForgot(string email);

        /// <summary>
        /// 找回密码的时候验证请求是否存在
        /// </summary>
        /// <param name="account"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        bool VerifyCode(string account, string code);

        /// <summary>
        /// 找回通行证密码
        /// </summary>
        /// <param name="retrievePassword"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        void PassportRetrievePassword(SYSPassportRetrievePasswordDTO retrievePassword);

        /// <summary>
        /// 更新账户状态
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="isValid"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        void PassportValid(string loginName, bool isValid);

        /// <summary>
        /// 更新密码锁定状态
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="isLocked"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        void PassportLock(string loginName, bool isLocked);

        #endregion

        #region Oauth2鉴权

        /// <summary>
        /// 请求用户授权Token
        /// </summary>
        /// <param name="authorize"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        SYSAuthorize Authorize(SYSAuthorizeDTO authorize);    
    
        /// <summary>
        /// 使用授权Token登录
        /// </summary>
        /// <param name="authorizeLogin"></param>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        void AuthorizeLogin(SYSAuthorizeLoginDTO authorizeLogin);

        /// <summary>
        /// 获取授权过的Access Token
        /// </summary>
        /// <param name="authorize"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        SYSAccessToken AccessToken(SYSAccessTokenDTO authorize);

        /// <summary>
        /// 验证令牌
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        SYSToken VerifyAccess(SYSVerifyAccessTokenDTO dto);

        /// <summary>
        /// 销毁访问令牌
        /// </summary>
        /// <param name="accessToken"></param>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        void DestroyToken(string accessToken);

        /// <summary>
        /// 注销令牌
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="passportId"></param>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        void DestroyToken(long systemId, long passportId);

        #endregion
    }
}
