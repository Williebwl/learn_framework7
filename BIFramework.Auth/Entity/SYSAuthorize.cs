using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Auth
{
    /// <summary>
    /// 客户端请求授权码返回值
    /// </summary>
    public struct SYSAuthorize
    {
        public SYSAuthorize(string code, string state)
            : this()
        {
            this.code = code;
            this.state = state;
        }
        /// <summary>
        /// 授权回调地址
        /// </summary>
        public string redirect_uri { get; set; }
        /// <summary>
        /// 用于调用access_token，接口获取授权后的access token
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 如果传递参数，会回传该参数
        /// </summary>
        public string state { get; set; }
    }
}
