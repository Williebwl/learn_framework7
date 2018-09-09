using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIStudio.Framework.UI;

namespace WebApi.Controllers.Tag
{
    public class TagVM : ViewModel
    {
        public long? ID { get; set; }

        /// <summary>
        /// 标签类型名称,必须
        /// </summary>
        public string TagClass { get; set; }

        /// <summary>
        /// 标签类型ID,必须
        /// </summary>
        public long? TagClassID { get; set; }

        /// <summary>
        /// 标签名称,必须
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// code标示符
        /// </summary>
        public string TagCode { get; set; }

        /// <summary>
        /// 标签项值
        /// </summary>
        public string TagValue { get; set; }

        /// <summary>
        /// 标签的全路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 当前所在层
        /// </summary>
        public int? Layer { get; set; }

        /// <summary>
        /// 多级分类标签时上级标签id，默认为0
        /// </summary>
        public long? ParentID { get; set; }

        /// <summary>
        /// 多级分类标签时上级标签Name
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// 是否叶子，1表示是叶子，不是为0，只有没有子节点时才是叶子
        /// </summary>
        public int? IsLeaf { get; set; }

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

        /// <summary>
        /// 已排序的序列号
        /// </summary>
        public string ComputedSequence { get; set; }
        /// <summary>
        /// 单位ID
        /// </summary>
        public long? SystemID { get; set; }
    }
}