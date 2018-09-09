using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Tenant
{
    public struct SYSAppRegistDTO
    {
        public SYSApp App { get; set; }

        public SYSMenu Menu { get; set; }

        public List<SYSAppAccess> AppGroups { get; set; }

    }

    /// <summary>
    /// 应用编辑操作
    /// </summary>
    public enum SYSAppRegistResult
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
