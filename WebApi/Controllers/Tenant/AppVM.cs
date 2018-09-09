using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BIStudio.Framework.UI;

namespace WebApi.Controllers.Tenant
{
    /// <summary>
    /// 应用
    /// </summary>
    public class AppVM : ViewModel
    {
        /// <summary>
        /// 数据标示
        /// </summary>
        public long? ID { get; set; }

        /// <summary>
        /// 系统标示
        /// </summary>
        public long? SystemID { get; set; }

        /// <summary>
        /// 标签组名称
        /// </summary>
        [Required, Display(Name = "应用名称")]
        public string AppName { get; set; }

        /// <summary>
        /// 标签组前缀
        /// </summary>
        [Required, Display(Name = "应用编号")]
        public string AppCode { get; set; }

        /// <summary>
        /// 类目编号
        /// </summary>
        [Required, Display(Name = "应用类型")]
        public long? AppTypeID { get; set; }

        /// <summary>
        /// 类目名称
        /// </summary>
        [Required, Display(Name = "应用类型")]
        public string AppType { get; set; }

        /// <summary>
        /// 是否内置（1-是；0-否）
        /// </summary>
        public int? IsBuiltIn { get; set; }

        /// <summary>
        /// 是否有效（1-有效，0-停用）
        /// </summary>
        public int? IsValid { get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
        public int? Sequence { get; set; }

        /// <summary>
        /// 点击量
        /// </summary>
        public int? Views { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}