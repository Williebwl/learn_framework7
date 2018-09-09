using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 表示多租户支持
    /// </summary>
    public interface ITenantAudited
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        long? SystemID { get; set; }
    }
}
