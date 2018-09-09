using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Utils;
using BIStudio.Framework;

namespace BIStudio.Framework.Auth
{
    /// <summary>
    /// 更新令牌生成器
    /// </summary>
    [Description(SYSAccessTokenDTO.RefreshToken)]
    public class RefreshTokenGenerator : DomainService, ITokenGenerator
    {
        private IRepository<SYSToken> _tokenRepository;

        /// <summary>
        /// 生成更新令牌
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public SYSAccessToken GeneralTocken(SYSAccessTokenDTO accessToken)
        {
            //检查更新令牌
            var refreshToken = _tokenRepository.Get(item => item.RefreshToken == accessToken.refresh_token);
            if (refreshToken.ID == null)
                throw CFException.Create(STDAccessTokenResult.ClientIDOrSecretInvalid);
            //创建访问令牌
            var token = new SYSAccessToken(refreshToken.SystemID.Value)
            {
                uid = refreshToken.PassportID,
                scope = refreshToken.Scope,
            };
            return token;
        }
    }
}
