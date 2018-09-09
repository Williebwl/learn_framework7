using System.Collections.Generic;

namespace BIStudio.Framework.Permission
{
    public class PermissionDTO
    {
        /// <summary>
        /// 状态
        /// </summary>
        public EnumPermissionState State { get; set; }

        /// <summary>
        /// key标识类型名称，
        /// filter用来做筛选
        /// </summary>
        public Dictionary<string, List<SYSFilter>> Filters { get; set; }
    }
}
