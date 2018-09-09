using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Utils;
using BIStudio.Framework;
using BIStudio.Framework.Tenant;

namespace BIStudio.Framework.Auth
{
    /// <summary>
    /// 客户端授权令牌生成器
    /// </summary>
    [Description(SYSAccessTokenDTO.ClientCredentials)]
    public class ClientCredentialsGenerator : DomainService, ITokenGenerator
    {

        private IRepository<SYSSystemCertificate> _certificateRepository;
        private IRepository<SYSToken> _tokenRepository;

        public SYSAccessToken GeneralTocken(SYSAccessTokenDTO accessToken)
        {
            //检查应用代码和密钥
            var certificate = _certificateRepository.Get(item => item.ApiKey == accessToken.client_id && item.Secret == accessToken.client_secret);
            if (certificate.ID == null)
                throw CFException.Create(STDAccessTokenResult.ClientIDOrSecretInvalid);
            //创建访问令牌
            var token = new SYSAccessToken(certificate.SystemID.Value);    //令牌有效期：2小时
            return token;
        }
    }
}
