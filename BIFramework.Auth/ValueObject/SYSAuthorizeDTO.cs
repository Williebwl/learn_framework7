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
    /// 请求授权码
    /// </summary>
    public struct SYSAuthorizeDTO
    {
        public const string Code = "code";
        /// <summary>
        /// 授权码模式
        /// </summary>
        /// <param name="client_id"></param>
        /// <param name="redirect_uri"></param>
        /// <param name="scope"></param>
        /// <param name="state"></param>
        public SYSAuthorizeDTO(string client_id, string redirect_uri = null, string scope = null, string state = null)
            : this()
        {
            this.response_type = Code;
            this.client_id = client_id;
            this.redirect_uri = redirect_uri;
            this.scope = scope;
            this.state = state;
        }
        /// <summary>
        /// 授权类型
        /// </summary>
        public string response_type { get; set; }
        /// <summary>
        /// 申请应用时分配的AppKey
        /// </summary>
        public string client_id { get; set; }
        /// <summary>
        /// 重定向URI
        /// </summary>
        public string redirect_uri { get; set; }
        /// <summary>
        /// 申请的权限范围
        /// </summary>
        public string scope { get; set; }
        /// <summary>
        /// 客户端的当前状态
        /// </summary>
        public string state { get; set; }
        /// <summary>
        ///  当前授权用户的id
        /// </summary>
        public long? uid { get; set; }
    }
    public enum STDAuthorizeResult
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
        /// ResponseType无效
        /// </summary>
        [Description("ResponseType无效")]
        ResponseTypeInvalid,
        /// <summary>
        /// ClientID无效
        /// </summary>
        [Description("ClientID无效")]
        ClientIDInvalid,

    }
}
