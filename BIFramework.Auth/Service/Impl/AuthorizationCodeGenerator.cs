using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Utils;
using BIStudio.Framework;
using BIStudio.Framework.Cache;
using BIStudio.Framework.Tenant;

namespace BIStudio.Framework.Auth
{
    /// <summary>
    /// 访问令牌生成器
    /// </summary>
    [Description(SYSAccessTokenDTO.AuthorizationCode)]
    public class AuthorizationCodeGenerator : DomainService, ITokenGenerator
    {
        private IRepository<SYSSystemCertificate> _certificateRepository;
        private IRepository<SYSToken> _tokenRepository;
        
        /// <summary>
        /// 生成访问令牌
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public SYSAccessToken GeneralTocken(SYSAccessTokenDTO accessToken)
        {
            //检查应用代码和密钥
            var certificate = _certificateRepository.Get(item => item.ApiKey == accessToken.client_id && item.Secret == accessToken.client_secret);
            if (certificate.ID == null)
                throw CFException.Create(STDAccessTokenResult.ClientIDOrSecretInvalid);
            //验证授权码
            var authorize = CacheService.Default.Get<SYSAuthorizeDTO>("STDAuthorizeDTO_" + accessToken.code);
            if (authorize.Equals(default(SYSAuthorizeDTO)))
                throw CFException.Create(STDAccessTokenResult.CodeInvalid);

            if (authorize.redirect_uri != accessToken.redirect_uri)
                throw CFException.Create(STDAccessTokenResult.RedirectUriInvalid);
            //验证用户登录
            if (authorize.uid == null)
                throw CFException.Create(STDAccessTokenResult.UIDInvalid);
            //创建访问令牌
            var token = new SYSAccessToken(certificate.SystemID.Value)     //令牌有效期：2小时
            {
                uid = authorize.uid.Value,
                scope = authorize.scope,
            };
            return token;
        }
    }
}
