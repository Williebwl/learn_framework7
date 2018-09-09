using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIStudio.Framework.UI;

namespace WebApi.Controllers.Tag
{
    public class TagClassVM : ViewModel
    {
        public long? ID { get; set; }

        /// <summary>
        /// 类型名称，必须
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// code标示符
        /// </summary>
        public string ClassCode { get; set; }

        /// <summary>
        /// 标签值
        /// </summary>
        public string ClassValue { get; set; }

        /// <summary>
        /// 标签组编号,必须
        /// </summary>
        public long? TagGroupID { get; set; }

        /// <summary>
        /// 标签类型
        /// </summary>
        public int? DisplayLevel { get; set; }

        /// <summary>
        /// 标签等级
        /// </summary>
        public int? IsBuiltIn { get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
        public int? Sequence { get; set; }

        /// <summary>
        /// 访问频率
        /// </summary>
        public int? Views { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

    }
}