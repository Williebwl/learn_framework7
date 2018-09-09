using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIStudio.Framework.Domain;
using BIStudio.Framework.UI;

namespace WebApi.Controllers.Institution
{

    public class DeptVM : ViewModel, ITreeVM
    {
        public IList<ITreeVM> Children { get; set; }

        public long? ID { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 部门名称简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 上级部门id
        /// </summary>
        public long? ParentID { get; set; }

        /// <summary>
        /// 层级 1开始计算
        /// </summary>
        public int? Layer { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sequence { get; set; }

        /// <summary>
        /// 部门上级领导id
        /// </summary>
        public long? LeaderID { get; set; }

        /// <summary>
        /// 部门负责人
        /// </summary>
        public long? ManagerID { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 是否停用 1启用 0 停用
        /// </summary>
        public int? IsStop { get; set; }

        /// <summary>
        /// 单位id
        /// </summary>
        public long? SystemID { get; set; }

        /// <summary>
        /// 是否单位 0部门 1单位
        /// </summary>
        public int? IsUnit { get; set; }


    }
}