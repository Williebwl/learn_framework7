using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIStudio.Framework.Utils;
using System.ComponentModel;

namespace BIStudio.Framework.Auth
{
    public class SYSVerifyAccessTokenDTO
    {
        public SYSVerifyAccessTokenDTO(string access_token, string scope)
        {
            this.access_token = access_token;
            this.scope = scope;
        }

        /// <summary>
        /// 表示访问令牌
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 表示权限范围
        /// </summary>
        public string scope { get; set; }
    }

    /// <summary>
    /// 系统注册结果
    /// </summary>
    [ALEnumDescription("系统注册结果")]
    public enum STDVerifyAccessTokenResult
    {
        /// <summary>
        /// 操作成功
        /// </summary>
        [Description("操作成功")]
        Success = 0,
        /// <summary>
        /// 操作失败
        /// </summary>
        [Description("操作失败")]
        Fail,
        /// <summary>
        /// 证书已过期
        /// </summary>
        [Description("证书已过期")]
        AcceccTokenExpired,
        /// <summary>
        /// 证书无效
        /// </summary>
        [Description("证书无效")]
        AccessTockenInvalid,
        /// <summary>
        /// 拒绝访问
        /// </summary>
        [Description("拒绝访问")]
        AccessTockenAccessDenied,
    }
   
}
