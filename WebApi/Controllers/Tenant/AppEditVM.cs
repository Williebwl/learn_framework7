using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIStudio.Framework.UI;

namespace WebApi.Controllers.Tenant
{
    /// <summary>
    /// 模块提交
    /// </summary>
    public class AppEditVM : ViewModel
    {
        public AppVM App { get; set; }


        public MenuVM Menu { get; set; }

        /// <summary>
        /// 模块权限
        /// </summary>
        public List<AppAccessVM> AppGroups { get; set; }
    }
}