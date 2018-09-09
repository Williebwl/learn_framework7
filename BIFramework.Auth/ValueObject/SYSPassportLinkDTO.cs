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
    /// 通行证绑定系统账号
    /// </summary>
    public struct SYSPassportLinkDTO
    {
        
        public string LoginName { get; set; }
        public string SystemCode { get; set; }
        public string UID { get; set; }
    }
    public enum SYSPassportLinkResult
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
        /// 未指定账号或用户标识
        /// </summary>
        [Description("未指定账号或用户标识")]
        LoginNameOrUIDNotFound,
        /// <summary>
        /// 账号不存在
        /// </summary>
        [Description("账号不存在")]
        LoginNameInvalid,
        /// <summary>
        /// 系统代码无效
        /// </summary>
        [Description("系统代码无效")]
        SystemCodeInvalid,
        /// <summary>
        /// 用户标识无效
        /// </summary>
        [Description("用户标识无效")]
        UIDInvalid,
    }
}
