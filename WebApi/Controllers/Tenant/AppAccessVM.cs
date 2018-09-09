using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIStudio.Framework.UI;

namespace WebApi.Controllers.Tenant
{
    /// <summary>
    /// 模块权限
    /// </summary>
    public class AppAccessVM : ViewModel
    {
        /// <summary>
        /// 数据唯一标示
        /// </summary>
        public long? ID { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public long? AppID { get; set; }

        /// <summary>
        /// 用户组ID
        /// </summary>
        public long? GroupID { get; set; }

        /// <summary>
        /// 用户组Code
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// 用户组Name
        /// </summary>
        public string GroupName { get; set; }
    }
}