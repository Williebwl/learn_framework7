using BIStudio.Framework.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Auth
{
    /// <summary>
    /// 创建应用令牌
    /// </summary>
    public struct SYSAccessTokenDTO
    {
        public const string AuthorizationCode = "authorization_code";
        public const string ClientCredentials = "client_credentials";
        public const string RefreshToken = "refresh_token";
        /// <summary>
        /// 使用授权码换取访问令牌
        /// </summary>
        /// <param name="client_id"></param>
        /// <param name="client_secret"></param>
        /// <param name="code"></param>
        public SYSAccessTokenDTO(string client_id, string client_secret, string code)
            : this()
        {
            this.grant_type = AuthorizationCode;
            this.client_id = client_id;
            this.client_secret = client_secret;
            this.code = code;
        }
        /// <summary>
        /// 使用证书换取访问令牌
        /// </summary>
        /// <param name="client_id"></param>
        /// <param name="client_secret"></param>
        public SYSAccessTokenDTO(string client_id, string client_secret)
            : this()
        {
            this.grant_type = ClientCredentials;
            this.client_id = client_id;
            this.client_secret = client_secret;
        }
        /// <summary>
        /// 使用更新令牌换取访问令牌
        /// </summary>
        /// <param name="refresh_token"></param>
        public SYSAccessTokenDTO(string refresh_token)
            : this()
        {
            this.grant_type = RefreshToken;
            this.refresh_token = refresh_token;
        }
        /// <summary>
        /// 使用的授权模式
        /// </summary>
        public string grant_type { get; set; }
        /// <summary>
        /// 申请应用时分配的AppKey
        /// </summary>
        public string client_id { get; set; }
        /// <summary>
        /// 申请应用时分配的AppSecret
        /// </summary>
        public string client_secret { get; set; }
        /// <summary>
        /// 上一步获得的授权码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 重定向URI
        /// </summary>
        public string redirect_uri { get; set; }
        /// <summary>
        /// 更新令牌
        /// </summary>
        public string refresh_token { get; set; }
    }
    public enum STDAccessTokenResult
    {
        /// <summary>
        /// 操作成功
        /// </summary>
        [Description("操作成功")]
        Success = 0,
        /// <summary>
        /// 操作失败
        /// </summary>
        [Description("{0}")]
        Fail,
        /// <summary>
        /// GrantType无效
        /// </summary>
        [Description("GrantType无效")]
        GrantTypeInvalid,
        /// <summary>
        /// ClientID或ClientSecret无效
        /// </summary>
        [Description("ClientID或ClientSecret无效")]
        ClientIDOrSecretInvalid,
        /// <summary>
        /// Code无效
        /// </summary>
        [Description("Code无效")]
        CodeInvalid,
        /// <summary>
        /// RedirectUri无效
        /// </summary>
        [Description("RedirectUri无效")]
        RedirectUriInvalid,
        /// <summary>
        /// UID无效
        /// </summary>
        [Description("UID无效")]
        UIDInvalid,
        /// <summary>
        /// 未找到有效账号
        /// </summary>
        [Description("未找到有效账号")]
        AccountNotFount,

    }
}
