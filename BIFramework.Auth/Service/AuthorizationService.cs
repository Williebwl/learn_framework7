using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIStudio.Framework;

using BIStudio.Framework.Utils;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Cache;
using BIStudio.Framework.Tenant;

namespace BIStudio.Framework.Auth
{
    [Ioc(typeof(IAuthorizationService))]
    public class AuthorizationService : DomainService, IAuthorizationService
    {
        #region 初始化

        private IRepository<SYSPassport> _passportRepository;
        private IRepository<SYSAccount> _accountRepository;
        private IRepository<SYSToken> _tokenRepository;
        private IRepository<SYSSystem> _systemRepository;
        private IRepository<SYSSystemCertificate> _certificateRepository;
        private ITokenGenerator _tokenGenerator;

        /// <summary>
        /// 令牌管理系统ID
        /// </summary>
        private const int MasterSystemID = 1;
        /// <summary>
        /// 主令牌有效时间
        /// </summary>
        private const int MasterTokenDurationMinutes = 60 * 24;
        /// <summary>
        /// 应用令牌有效时间
        /// </summary>
        private const int AppTokenDurationMinutes = 60 * 2;

        #endregion
        
        #region 账户

        /// <summary>
        /// 注册系统账号
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public SYSAccount AccountRegist(SYSAccountRegistDTO dto)
        {
            if (string.IsNullOrEmpty(dto.SystemCode) || string.IsNullOrEmpty(dto.UID))
                throw CFException.Create(SYSAccountRegistResult.UIDInvalid);

            try
            {
                SYSSystem system = _systemRepository.Get(item => item.SystemCode == dto.SystemCode);
                if (system.ID == null)
                    throw CFException.Create(SYSAccountRegistResult.SystemCodeInvalid);

                var prevAccount = _accountRepository.Get(item => item.SystemID == system.ID && item.UID == dto.UID);
                var entity = dto.Map<SYSAccountRegistDTO, SYSAccount>();
                if (prevAccount.ID.HasValue)
                {
                    entity.ID = prevAccount.ID;
                    _accountRepository.Modify(entity);
                }
                else
                {
                    entity.SystemID = system.ID;
                    entity.SystemName = system.SystemName;
                    entity.InputTime = DateTime.Now;
                    entity.InputIP = CFContext.User.IP;
                    _accountRepository.Add(entity);
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw CFException.Create(SYSAccountRegistResult.Fail, ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除系统账号
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="uId"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public void UnRegisterAccount(long systemId, string uId)
        {
            var entity = _accountRepository.Get(item => item.SystemID == systemId && item.UID == uId);
            if (!entity.ID.HasValue)
                throw CFException.Create(OperateResult.NotFound);
            try
            {
                _accountRepository.Remove(entity);
                return;
            }
            catch (Exception ex)
            {

                throw CFException.Create(OperateResult.Fail, ex.Message, ex);
            }
        }

        /// <summary>
        /// 注册通行证
        /// </summary>
        /// <param name="passport"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public SYSPassport PassportRegist(SYSPassportRegistDTO passport)
        {
            if (string.IsNullOrEmpty(passport.LoginName) || !ALValidator.IsLengthStr(passport.LoginName, 4, 50) || !ALValidator.IsNormalChar(passport.LoginName))
                throw CFException.Create(SYSPassportRegistResult.LoginNameInvalid);
            if (string.IsNullOrEmpty(passport.Password) || !ALValidator.IsLengthStr(passport.Password, 6, 16))
                throw CFException.Create(SYSPassportRegistResult.PasswordTooWeak);
            if (string.IsNullOrEmpty(passport.RePassword) || passport.Password != passport.RePassword)
                throw CFException.Create(SYSPassportRegistResult.RePasswordIncorrect);
            if (string.IsNullOrEmpty(passport.Email) || !ALValidator.IsEmail(passport.Email))
                throw CFException.Create(SYSPassportRegistResult.EmailInvalid);

            try
            {
                if (_passportRepository.Get(item => item.LoginName == passport.LoginName).ID != null)
                    throw CFException.Create(SYSPassportRegistResult.LoginNameAlreadyExists);

                if (_passportRepository.Get(item => item.Email == passport.Email).ID != null)
                    throw CFException.Create(SYSPassportRegistResult.EmailAlreadyExists);

                var now = DateTime.Now;
                var entity = new SYSPassport
                {
                    LoginName = passport.LoginName,
                    Email = passport.Email,
                    Remarks = passport.Remarks,
                    LastLoginTime = now,
                    LastLoginError = 0,
                    IsValid = true,
                    IsLocked = false,
                    Inputer = CFContext.User.UserName,
                    InputerID = CFContext.User.ID,
                    InputTime = now
                };
                entity.Password = entity.ComputePassword(passport.LoginName, passport.Password);
                _passportRepository.Add(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw CFException.Create(SYSPassportRegistResult.Fail, ex.Message, ex);
            }
        }

        /// <summary>
        /// 注销通行证
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public void PassportUnRegister(string loginName)
        {
            var entity = _passportRepository.Get(item => item.LoginName == loginName);
            if (!entity.ID.HasValue)
                throw CFException.Create(OperateResult.NotFound);
            try
            {
                _passportRepository.Remove(entity);
                return;
            }
            catch (Exception ex)
            {
                throw CFException.Create(OperateResult.Fail, ex.Message, ex);
            }
        }

        /// <summary>
        /// 通行证绑定系统账号
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public void PassportLink(SYSPassportLinkDTO link)
        {
            if (string.IsNullOrEmpty(link.LoginName) || string.IsNullOrEmpty(link.SystemCode) || string.IsNullOrEmpty(link.UID))
                throw CFException.Create(SYSPassportLinkResult.LoginNameOrUIDNotFound);

            try
            {
                var passport = _passportRepository.Get(item => item.LoginName == link.LoginName);
                if (passport.ID == null)
                    throw CFException.Create(SYSPassportLinkResult.LoginNameInvalid);

                var system = _systemRepository.Get(item => item.SystemCode == link.SystemCode);
                if (system.ID == null)
                    throw CFException.Create(SYSPassportLinkResult.SystemCodeInvalid);

                var account = _accountRepository.Get(item => item.UID == link.UID && item.SystemID == system.ID);
                if (account.ID == null || account.PassportID != null)
                    throw CFException.Create(SYSPassportLinkResult.UIDInvalid);

                account.PassportID = passport.ID;
                _accountRepository.Modify(new SYSAccount { ID = account.ID, PassportID = passport.ID });
                return;
            }
            catch (Exception ex)
            {
                throw CFException.Create(SYSPassportLinkResult.Fail, ex.Message);
            }
        }

        /// <summary>
        /// 通行证解绑系统账号
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public void PassportUnlink(SYSPassportLinkDTO link)
        {
            if (string.IsNullOrEmpty(link.LoginName) || string.IsNullOrEmpty(link.SystemCode) || string.IsNullOrEmpty(link.UID))
                throw CFException.Create(SYSPassportLinkResult.LoginNameOrUIDNotFound);

            try
            {
                var passport = _passportRepository.Get(item => item.LoginName == link.LoginName);
                if (passport.ID == null)
                    throw CFException.Create(SYSPassportLinkResult.LoginNameInvalid);

                var system = _systemRepository.Get(item => item.SystemCode == link.SystemCode);
                if (system.ID == null)
                    throw CFException.Create(SYSPassportLinkResult.SystemCodeInvalid);

                var account = _accountRepository.Get(item => item.SystemID == system.ID && item.UID == link.UID);
                if (account.ID == null || account.PassportID == null)
                    throw CFException.Create(SYSPassportLinkResult.UIDInvalid);

                account.Property.IsDBNull("PassportID", true);
                _accountRepository.Remove(item => item.SystemID == system.ID && item.PassportID == passport.ID);
                return;
            }
            catch (Exception ex)
            {
                throw CFException.Create(SYSPassportLinkResult.Fail, ex.Message, ex);
            }
        }

        #endregion

        #region 账户服务

        /// <summary>
        /// 更改通行证密码
        /// </summary>
        /// <param name="changePassword"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public void PassportChangePassword(SYSPassportChangePasswordDTO dto)
        {
            if (string.IsNullOrEmpty(dto.LoginName))
                throw CFException.Create(SYSPassportChangePasswordResult.LoginNameDoesNotExist);

            try
            {
                var passport = _passportRepository.Get(item => item.LoginName == dto.LoginName);
                //检查用户信息
                if (!passport.ID.HasValue)
                    throw CFException.Create(SYSPassportChangePasswordResult.LoginNameDoesNotExist);
                //检查密码
                if (string.IsNullOrEmpty(dto.OldPassword) || (ALEncrypt.InstanceKey != dto.OldPassword && BitConverter.ToString(passport.Password) != BitConverter.ToString(passport.ComputePassword(dto.LoginName, dto.OldPassword))))
                    throw CFException.Create(SYSPassportChangePasswordResult.PasswordIncorrect);
                //检查新密码
                if (string.IsNullOrEmpty(dto.NewPassword) || !ALValidator.IsLengthStr(dto.NewPassword, 6, 16))
                    throw CFException.Create(SYSPassportChangePasswordResult.PasswordTooWeak);
                if (string.IsNullOrEmpty(dto.ReNewPassword) || dto.NewPassword != dto.ReNewPassword)
                    throw CFException.Create(SYSPassportChangePasswordResult.RePasswordIncorrect);

                //更改密码
                passport.Password = passport.ComputePassword(dto.LoginName, dto.NewPassword);
                _passportRepository.Modify(passport);
                return;
            }
            catch (Exception ex)
            {
                throw CFException.Create(SYSPassportChangePasswordResult.Fail, ex.Message, ex);
            }
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public SYSPassworkForgetDTO PassportForgot(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    throw CFException.Create(STDPassworkForgetResult.EmailIncorrect);
                var passport = _passportRepository.Get(item => item.Email == email);

                if (passport == null)
                    throw CFException.Create(STDPassworkForgetResult.EmailIncorrect);
                passport.VerificationCode = ALUtils.GetGUIDShort();
                _passportRepository.Modify(passport);
                return new SYSPassworkForgetDTO(email, passport.LoginName, passport.VerificationCode);
            }
            catch (Exception ex)
            {
                throw CFException.Create(STDPassworkForgetResult.Fail, ex.Message, ex);
            }

        }


        /// <summary>
        /// 找回密码时候验证状态
        /// </summary>
        /// <param name="account"></param>
        /// <param name="code">用户触发找回密码的时候会生成一个验证码，用于找回密码</param>
        /// <returns></returns>
        public bool VerifyCode(string account, string code)
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(code))
                return false;
            try
            {
                var passport = _passportRepository.Get(item => item.LoginName == account && item.VerificationCode == code);
                if (passport == null)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 找回通行证密码
        /// </summary>
        /// <param name="retrievePassword"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public void PassportRetrievePassword(SYSPassportRetrievePasswordDTO dto)
        {
            if (string.IsNullOrEmpty(dto.LoginName))
                throw CFException.Create(SYSPassportChangePasswordResult.LoginNameDoesNotExist);

            try
            {
                var passport = _passportRepository.Get(item => item.LoginName == dto.LoginName);
                //检查用户信息
                if (!passport.ID.HasValue)
                    throw CFException.Create(SYSPassportChangePasswordResult.LoginNameDoesNotExist);
                //检查新密码
                if (string.IsNullOrEmpty(dto.Password) || !ALValidator.IsLengthStr(dto.Password, 6, 16))
                    throw CFException.Create(SYSPassportChangePasswordResult.PasswordTooWeak);
                if (string.IsNullOrEmpty(dto.ConfirmedPassword) || dto.Password != dto.ConfirmedPassword)
                    throw CFException.Create(SYSPassportChangePasswordResult.RePasswordIncorrect);

                //更改密码
                passport.Password = passport.ComputePassword(dto.LoginName, dto.Password);
                _passportRepository.Modify(passport);
                return;
            }
            catch (Exception ex)
            {
                throw CFException.Create(SYSPassportChangePasswordResult.Fail, ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新账户状态
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="isValid"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public void PassportValid(string loginName, bool isValid)
        {
            var passport = _passportRepository.Get(item => item.LoginName == loginName);
            if (!passport.ID.HasValue)
                throw CFException.Create(OperateResult.NotFound);
            passport.IsValid = isValid;
            try
            {
                _passportRepository.Modify(passport);
                return;
            }
            catch (Exception ex)
            {
                throw CFException.Create(OperateResult.Fail, ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新密码锁定状态
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="isLocked"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public void PassportLock(string loginName, bool isLocked)
        {
            var passport = _passportRepository.Get(item => item.LoginName == loginName);
            if (!passport.ID.HasValue)
                throw CFException.Create(OperateResult.NotFound);
            passport.IsLocked = isLocked;
            try
            {
                _passportRepository.Modify(passport);
                return;
            }
            catch (Exception ex)
            {
                throw CFException.Create(OperateResult.Fail, ex.Message, ex);
            }
        }
        #endregion

        #region Oauth2鉴权

        /// <summary>
        /// 请求用户授权Token
        /// </summary>
        /// <param name="authorize"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public SYSAuthorize Authorize(SYSAuthorizeDTO authorize)
        {
            //检查请求参数
            if (string.IsNullOrEmpty(authorize.response_type))
                throw CFException.Create(STDAuthorizeResult.ResponseTypeInvalid);
            if (string.IsNullOrEmpty(authorize.client_id))
                throw CFException.Create(STDAuthorizeResult.ClientIDInvalid);

            try
            {
                //检查应用代码
                var certificate = _certificateRepository.Get(item => item.ApiKey == authorize.client_id);
                if (certificate.ID == null)
                    throw CFException.Create(STDAuthorizeResult.ClientIDInvalid);
                //创建授权码
                string code = ALUtils.GetGUIDShort();
                CacheService.Default.GetOrAdd("STDAuthorizeDTO_" + code, authorize, 10);
                //HttpRuntime.Cache.Add("STDAuthorizeDTO_" + code, authorize, null, DateTime.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
                return new SYSAuthorize(code, authorize.state);
            }
            catch (Exception ex)
            {
                throw CFException.Create(STDAuthorizeResult.Fail, ex.Message, ex);
            }
        }

        /// <summary>
        /// 使用授权Token登录
        /// </summary>
        /// <param name="authorizeLogin"></param>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public void AuthorizeLogin(SYSAuthorizeLoginDTO authorizeLogin)
        {
            //检查请求参数
            if (string.IsNullOrEmpty(authorizeLogin.code))
                throw CFException.Create(STDAuthorizeLoginResult.AuthorizeCodeInvalid);
            if (string.IsNullOrEmpty(authorizeLogin.username) || string.IsNullOrEmpty(authorizeLogin.password))
                throw CFException.Create(STDAuthorizeLoginResult.AccountOrPasswordInvalid);

            try
            {
                //验证授权码
                var authorize = CacheService.Default.Get<SYSAuthorizeDTO>("STDAuthorizeDTO_" + authorizeLogin.code);
                if (authorize.Equals(default(SYSAuthorizeDTO)))
                    throw CFException.Create(STDAuthorizeLoginResult.AuthorizeCodeInvalid);
                //验证用户账号
                SYSPassport passport;
                if (!Login(authorizeLogin.username, authorizeLogin.password, out passport))
                    throw CFException.Create(STDAuthorizeLoginResult.AccountOrPasswordInvalid);

                CacheService.Default.Add("STDAuthorizeDTO_" + authorizeLogin.code, new SYSAuthorizeDTO
                {
                    response_type = authorize.response_type,
                    client_id = authorize.client_id,
                    redirect_uri = authorize.redirect_uri,
                    scope = authorize.scope,
                    state = authorize.state,
                    uid = passport.ID,
                });
                return;
            }
            catch (Exception ex)
            {
                throw CFException.Create(STDAuthorizeLoginResult.Fail, ex.Message, ex);
            }
        }

        /// <summary>
        /// 使用通行证登录系统
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <param name="passport">登陆成功以后返回实体</param>
        /// <returns></returns>
        private bool Login(string loginName, string password, out SYSPassport passport)
        {
            passport = null;
            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(password))
                return false;
            //检查用户信息
            passport = _passportRepository.Get(item => item.LoginName == loginName);
            if (passport.ID == null || passport.IsValid == false)
                return false;
            //检查登录锁定
            if (passport.CheckLock())
                return false;
            passport.UnLock();
            //检查密码
            if (passport.CheckPassword(password))
            {
                passport.LastLoginTime = DateTime.Now;
                _passportRepository.Modify(passport);
                return true;
            }
            passport.PasswordError();
            _passportRepository.Modify(passport);
            return false;
        }

        /// <summary>
        ///  获取授权过的Access Token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public SYSAccessToken AccessToken(SYSAccessTokenDTO accessToken)
        {
            return _tokenGenerator.GeneralTocken(accessToken);
        }
        /// <summary>
        /// 验证tocken
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public SYSToken VerifyAccess(SYSVerifyAccessTokenDTO dto)
        {
            try
            {
                var tocken = _tokenRepository.Get(item => item.AccessToken == dto.access_token);
                if (!tocken.ID.HasValue||tocken.AccessToken != dto.access_token)
                    throw CFException.Create(STDVerifyAccessTokenResult.AccessTockenInvalid);
                if (!(tocken.Scope ?? "").Contains((dto.scope ?? "")))
                    throw CFException.Create(STDVerifyAccessTokenResult.AccessTockenAccessDenied);
                if (tocken.ExpiresIn.HasValue && DateTime.Now > tocken.ExpiresIn.Value)
                    throw CFException.Create(STDVerifyAccessTokenResult.AcceccTokenExpired);
                return tocken;
            }
            catch (Exception ex)
            {
                throw CFException.Create(STDVerifyAccessTokenResult.Fail, ex.Message, ex);
            }
        }

        /// <summary>
        /// 注销访问令牌
        /// </summary>
        /// <param name="accessToken"></param>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public void DestroyToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                throw CFException.Create(OperateResult.NotFound);

            var entity = _tokenRepository.Get(item => item.AccessToken == accessToken);
            if (!entity.ID.HasValue)
                throw CFException.Create(OperateResult.NotFound);
            try
            {
                _tokenRepository.Remove(entity);
                return;
            }
            catch (Exception ex)
            {
                throw CFException.Create(OperateResult.Fail, ex.Message, ex);
            }
        }

        /// <summary>
        /// 注销访问令牌
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="passportId"></param>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public void DestroyToken(long systemId, long passportId)
        {
            var entity = _tokenRepository.Get(item => item.SystemID == systemId && item.PassportID == passportId);
            if (!entity.ID.HasValue)
                throw CFException.Create(OperateResult.NotFound);
            try
            {
                _tokenRepository.Remove(entity);
                return;
            }
            catch (Exception ex)
            {
                throw CFException.Create(OperateResult.Fail, ex.Message, ex);
            }
        }
        #endregion

    }
}
