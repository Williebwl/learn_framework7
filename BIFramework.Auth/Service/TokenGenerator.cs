using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BIStudio.Framework;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Utils;

namespace BIStudio.Framework.Auth
{
    /// <summary>
    /// 令牌生成服务
    /// </summary>
    [Ioc(typeof(ITokenGenerator))]
    public class TokenGenerator : DomainService, ITokenGenerator
    {
        #region 查找令牌生成器

        private static ConcurrentDictionary<string, Type> _authTypes;
        /// <summary>
        /// 是否允许并发会话
        /// </summary>
        private static bool enableConcurrentSessions = true;

        static TokenGenerator()
        {
            _authTypes = new ConcurrentDictionary<string, Type>(
                Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(item => item.GetInterfaces().Contains(typeof(ITokenGenerator)))
                    .ToDictionary(item =>
                    {
                        var desc = item.GetCustomAttribute<DescriptionAttribute>();
                        return desc != null ? desc.Description.ToLower() : string.Empty;
                    }, item => item));
        }

        #endregion

        private IRepository<SYSAccount> _accountRepository;
        private IRepository<SYSToken> _tokenRepository;

        [MethodImpl(MethodImplOptions.Synchronized)]
        private ITokenGenerator CreateAuth(string granttype)
        {
            if (!_authTypes.ContainsKey(granttype.ToLower()))
                throw new NotSupportedException("granttype");
            ITokenGenerator tokenGenerator = (ITokenGenerator)Activator.CreateInstance(_authTypes[granttype.ToLower()]);
            tokenGenerator.DependOn(this.Context);
            return tokenGenerator;
        }

        private bool Exist(string granttype)
        {
            return _authTypes != null && _authTypes.ContainsKey(granttype);
        }
        /// <summary>
        /// 生成令牌
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public SYSAccessToken GeneralTocken(SYSAccessTokenDTO dto)
        {
            if (!Exist(dto.grant_type))
                throw CFException.Create(STDAccessTokenResult.GrantTypeInvalid);

            try
            {
                //获得访问密钥
                var auth = CreateAuth(dto.grant_type);
                var token = auth.GeneralTocken(dto);
                //保存访问密钥
                var appToken = SaveToken(token);
                if (appToken.ID == null)
                    throw CFException.Create(STDAccessTokenResult.AccountNotFount);
                return token;
            }
            catch (DefinedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw CFException.Create(STDAccessTokenResult.Fail, ex.Message, ex);
            }
        }

        /// <summary>
        /// 生成accesstocken之后操作存储
        /// </summary>
        /// <param name="accessToken">访问令牌</param>
        /// <returns></returns>
        private SYSToken SaveToken(SYSAccessToken accessToken)
        {
            //获得应用账号
            SYSAccount account = _accountRepository.Get(item => item.SystemID == accessToken.system_id && item.PassportID == accessToken.uid);
            //获得应用令牌
            SYSToken token = _tokenRepository.Get(new TokenSpecification(accessToken.system_id, accessToken.uid));
            if (enableConcurrentSessions || token.ID == null)
            {
                token.PassportID = accessToken.uid;
                token.SystemID = accessToken.system_id;
                token.AccountID = account.ID;
                token.UID = account.UID;
                token.AccessToken = accessToken.access_token;
                token.RefreshToken = ALUtils.GetGUIDShort();
                token.RequestTime = DateTime.Now;
                token.RequestIP = CFContext.User.IP;
                token.Scope = accessToken.scope;
                token.ExpiresIn = DateTime.Now.AddSeconds(accessToken.expires_in);
                _tokenRepository.Add(token);
            }
            else
            {
                token.AccessToken = accessToken.access_token;
                token.Scope = accessToken.scope;
                token.ExpiresIn = DateTime.Now.AddSeconds(accessToken.expires_in);
                _tokenRepository.Modify(token);
            }
            return token;
        }
    }
}
