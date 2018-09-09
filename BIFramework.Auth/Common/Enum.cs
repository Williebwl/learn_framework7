
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Auth
{
    using BIStudio.Framework.Utils;

    /// <summary>
    /// 通行证状态
    /// </summary>
    [ALEnumDescription("通行证状态")]
    public enum AccountStatusEnum
    {
        /// <summary>
        /// 锁定
        /// </summary>
        [ALEnumDescription("锁定", 2)]
        Locking,

        /// <summary>
        /// 有效
        /// </summary>
        [ALEnumDescription("有效", 0)]
        Valid = 1,

        /// <summary>
        /// 停用
        /// </summary>
        [ALEnumDescription("停用", 1)]
        Disable
    }
}
