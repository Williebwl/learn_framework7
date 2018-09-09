using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIStudio.Framework.Utils;

namespace BIStudio.Framework.Auth
{
    /// <summary>
    /// 客户端请求访问令牌返回值，用户委托客户端访问平台的令牌
    /// </summary>
    public struct SYSAccessToken
    {
        public SYSAccessToken(long appid)
            : this()
        {
            this.access_token = ALUtils.GetGUIDShort();
            this.token_type = "bearer";
            this.expires_in = 60 * 60 * 24;
            this.system_id = appid;
        }
        /// <summary>
        /// 表示访问令牌
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 表示令牌类型，该值大小写不敏感bearer
        /// </summary>
        public string token_type { get; set; }
        /// <summary>
        /// 表示过期时间，单位为秒
        /// </summary>
        public long expires_in { get; set; }
        /// <summary>
        /// 表示更新令牌
        /// </summary>
        public string refresh_token { get; set; }
        /// <summary>
        /// 表示权限范围
        /// </summary>
        public string scope { get; set; }
        /// <summary>
        /// 当前授权系统的id
        /// </summary>
        public long system_id { get; set; }
        /// <summary>
        ///  当前授权用户的id，不填则为系统令牌
        /// </summary>
        public long? uid { get; set; }
    }
}
