using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Tenant
{
    /// <summary>
    /// 模块权限
    /// </summary>
    [Table("SYSAppAccess")]
    public class SYSAppAccess : Entity
    {
        /// <summary>
        /// 对应得模块ID
        /// </summary>
        public long? AppID { get; set; }

        /// <summary>
        /// 用户或角色ID
        /// </summary>
        public long? GroupID { get; set; }
    }

}
