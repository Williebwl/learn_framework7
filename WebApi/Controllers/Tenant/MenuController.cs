using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using BIStudio.Framework;
using BIStudio.Framework.Data;
using BIStudio.Framework.Tenant;
using BIStudio.Framework.UI;

namespace WebApi.Controllers.Tenant
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class MenuController : ApplicationService<MenuVM, PagedQuery, SYSMenu>
    {
        protected IMenuService _moduleBO;

        /// <summary>
        /// 获取一级菜单信息
        /// </summary>
        /// <returns>菜单信息</returns>
        public virtual IList<MenuVM> GetRoot()
        {
            return _moduleBO.GetRoot(1).Map<SYSMenu, MenuVM>().ToList();
        }


        /// <summary>
        /// 获取当前用户页面路由
        /// </summary>
        /// <returns>用户页面路由</returns>
        public virtual RouteInfoVM GetRoute()
        {
            var infos = _moduleBO.GetRoute(1);

            if (infos == null) return null;

            var vm = new RouteInfoVM { Routes = infos.Select(GetRoute).ToList() };

            if (infos.Any())
            {
                var info = infos.First();
                var vmInfo = vm.Routes.First().Clone();

                vm.Routes.Insert(0, vmInfo);

                vm.RedirectTo = string.Concat(vmInfo.Route, "/", info.ID.ToString());
                vmInfo.Route = string.Concat(vmInfo.Route, "/:id");
            }

            return vm;
        }

        #region Other

        /// <summary>
        /// 获取路由信息
        /// </summary>
        /// <param name="module">菜单信息</param>
        /// <returns>路由信息</returns>
        protected virtual RouteVM GetRoute(SYSMenu module)
        {
            return new RouteVM
            {
                Title = string.IsNullOrEmpty((module.ShortName ?? string.Empty).Trim()) ? module.MenuName : module.ShortName,
                Route = GetRouteTag(module),
                navTemplateUrl = GetTemplateUrl(module.NavUrl),
                navControllerUrl = GetControllerUrl(module.NavUrl),
                toolBarTemplateUrl = GetTemplateUrl(module.ToolBarUrl),
                toolBarControllerUrl = GetControllerUrl(module.ToolBarUrl),
                containerTemplateUrl = GetTemplateUrl(module.ContainerUrl),
                containerControllerUrl = GetControllerUrl(module.ContainerUrl),
                IsApp = module.Layer == 0 || !module.Layer.HasValue
            };
        }

        /// <summary>
        /// 获取模版路径
        /// </summary>
        /// <param name="url">源路径</param>
        /// <returns>模版路径</returns>
        protected virtual string GetTemplateUrl(string url)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrWhiteSpace(url)) return null;

            var ext = Path.GetExtension(url);
            if (".html".Equals(ext, StringComparison.CurrentCultureIgnoreCase)) return url;
            else if (".js".Equals(ext, StringComparison.CurrentCultureIgnoreCase)) url = url.Substring(0, url.Length - 3);

            return url + ".html";
        }

        /// <summary>
        /// 获取控制器路径
        /// </summary>
        /// <param name="url">源路径</param>
        /// <returns>控制器路径</returns>
        protected virtual string GetControllerUrl(string url)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrWhiteSpace(url)) return null;

            var ext = Path.GetExtension(url);
            if (".js".Equals(ext, StringComparison.CurrentCultureIgnoreCase)) return url;
            else if (".html".Equals(ext, StringComparison.CurrentCultureIgnoreCase)) url = url.Substring(0, url.Length - 5);

            return url + ".js";
        }

        /// <summary>
        /// 获取路由标示
        /// </summary>
        /// <param name="menu">菜单信息</param>
        /// <returns>路由标示</returns>
        protected virtual string GetRouteTag(SYSMenu menu)
        {
            return string.IsNullOrEmpty((menu.PageRoute ?? string.Empty).Trim()) ? menu.ID.ToString() : menu.PageRoute;
        }

        #endregion Other

        /// <summary>
        /// 根据应用id获取菜单信息
        /// </summary>
        /// <param name="id">菜单id</param>
        /// <returns>应用菜单信息</returns>
        [HttpGet]
        public virtual IList<MenuVM> GetInfoByAppId(long id)
        {
            return _moduleBO.GetInfoByAppId(id).Map<SYSMenu, MenuVM>().ToList();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="vm">菜单信息</param>
        /// <returns>菜单id</returns>
        [HttpPost]
        public virtual long Post(MenuVM vm)
        {
            return _moduleBO.Save(vm.Map<MenuVM, SYSMenu>());
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="vm">菜单信息</param>
        /// <returns>菜单id</returns>
        [HttpPut]
        public virtual long Put(MenuVM vm)
        {
            return _moduleBO.Save(vm.Map<MenuVM, SYSMenu>());
        }
    }
}