using BIStudio.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Tenant
{
    /// <summary>
    /// 系统运行状态
    /// </summary>
    [ALEnumDescription("系统运行状态")]
    public enum EnumSYSSystemStatus
    {
        /// <summary>
        /// 未安装
        /// </summary>
        [ALEnumDescription("未安装")]
        NotInstalled = 1,
        /// <summary>
        /// 停用
        /// </summary>
        [ALEnumDescription("停用")]
        Stop = 2,
        /// <summary>
        /// 启用
        /// </summary>
        [ALEnumDescription("启用")]
        Running = 3,
    }
}
