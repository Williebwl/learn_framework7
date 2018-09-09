using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIStudio.Framework.UI;

namespace WebApi.Controllers.Tenant
{
    /// <summary>
    /// 路由
    /// </summary>
    public class RouteVM : ViewModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 路由标示
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// 导航模板
        /// </summary>
        public string navTemplateUrl { get; set; }

        /// <summary>
        /// 导航控制器
        /// </summary>
        public string navControllerUrl { get; set; }

        /// <summary>
        /// 工具栏模板
        /// </summary>
        public string toolBarTemplateUrl { get; set; }

        /// <summary>
        /// 工具栏控制器
        /// </summary>
        public string toolBarControllerUrl { get; set; }

        /// <summary>
        /// 内容模板
        /// </summary>
        public string containerTemplateUrl { get; set; }

        /// <summary>
        ///内容控制器
        /// </summary>
        public string containerControllerUrl { get; set; }

        public bool IsApp { get; set; }
    }
}