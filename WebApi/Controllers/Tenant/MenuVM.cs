using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIStudio.Framework.Domain;
using BIStudio.Framework.UI;

namespace WebApi.Controllers.Tenant
{
    [Mark]
    public class MenuVM : ViewModel, ITreeVM
    {
        /// <summary>
        ///  服务提供商ID
        /// </summary>
        public long? SystemID { get; set; }

        /// <summary>
        ///  服务提供商ID
        /// </summary>
        public long? AppID { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 模块简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MenuCode { get; set; }

        /// <summary>
        /// 父节点ID
        /// </summary>
        public long? ParentID { get; set; }

        /// <summary>
        /// 处于第几层，第一层为1，最多5层
        /// </summary>
        public int? Layer { get; set; }

        /// <summary>
        /// 路径，格式为,1,2,3,4,5,
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 图标背景
        /// </summary>
        public string IconBackGround { get; set; }

        /// <summary>
        /// 简要说明
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sequence { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool? IsShow { get; set; }

        /// <summary>
        /// 导航
        /// </summary>
        public string NavUrl { get; set; }

        /// <summary>
        /// 工具栏
        /// </summary>
        public string ToolBarUrl { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string ContainerUrl { get; set; }

        /// <summary>
        /// 路由标示
        /// </summary>
        public string PageRoute { get; set; }

        public IList<ITreeVM> Children { get; set; }

        public long? ID { get; set; }

        /// <summary>
        /// 显示模式 : 0:默认 ; 1:主页型 ;2:自定义导航 ;3: 二级菜单导航
        /// </summary>
        public int? DisplayModeID { get; set; }

        /// <summary>
        /// 显示模式：默认 / 主页型 / 自定义导航 / 二级菜单导航
        /// </summary>
        public string DisplayMode { get; set; }

        /// <summary>
        /// 是否弹出
        /// </summary>
        public bool? IsPopUp { get; set; }

        /// <summary>
        /// 外部地址
        /// </summary>
        public string OutsideUrl { get; set; }

        /// <summary>
        /// 是否有Toolbar
        /// </summary>
        public bool? IsToolbar { get; set; }
    }
}