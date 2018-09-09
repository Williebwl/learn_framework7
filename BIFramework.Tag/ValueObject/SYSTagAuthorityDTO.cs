using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIStudio.Framework.Tag
{
    /// <summary>
    /// 权限所有者
    /// </summary>
    public struct SYSTagAuthorityDTO
    {
        /// <summary>
        /// 权限所有者
        /// </summary>
        public string AuthorityText { get; set; }
        /// <summary>
        /// 权限所有者编号
        /// </summary>
        public long? AuthorityValue { get; set; }

    }
}
