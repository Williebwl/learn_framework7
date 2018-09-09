using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.UI.Models
{
    using Newtonsoft.Json;
    using BIStudio.Framework.Domain;

    /// <summary>
    /// 树数据结构
    /// </summary>
    public class TreeVM : ViewModel, ITreeVM
    {
        /// <summary>
        /// 节点的子节点数据集合。 默认值：无
        /// </summary>
        [JsonProperty("children")]
        public IList<ITreeVM> Children { get; set; }

        /// <summary>
        /// 设置节点的 checkbox / radio 是否禁用 默认值：false
        /// </summary>
        [JsonProperty("chkDisabled")]
        public bool? CHKDisabled { get; set; }

        /// <summary>
        /// 强制节点的 checkBox / radio 的 半勾选状态 默认值：false
        /// </summary>
        [JsonProperty("halfCheck")]
        public bool? HalfCheck { get; set; }

        /// <summary>
        /// 记录 treeNode 节点是否为父节点
        /// </summary>
        [JsonProperty("isParent")]
        public bool? IsParent { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 节点值
        /// </summary>
        [JsonProperty("value")]
        public object Value { get; set; }

        /// <summary>
        /// 记录 treeNode 节点的 展开 / 折叠 状态 默认值：false
        /// </summary>
        [JsonProperty("open")]
        public bool? Open { get; set; }

        /// <summary>
        /// 节点的 checkBox / radio 的 勾选状态 默认值：false
        /// </summary>
        [JsonProperty("checked")]
        public bool? Checked { get; set; }

        /// <summary>
        /// 设置节点是否隐藏 checkbox / radio
        /// </summary>
        [JsonProperty("nocheck")]
        public bool? Nocheck { get; set; }

        /// <summary>
        /// 设置点击节点后在何处打开 url 默认值：无
        /// </summary>
        [JsonProperty("target")]
        public string Target { get; set; }

        /// <summary>
        /// 节点链接的目标 URL 默认值：无
        /// </summary>
        [JsonProperty("url")]
        public string URL { get; set; }

        /// <summary>
        /// 父id
        /// </summary>
        [JsonProperty("pid")]
        public long? ParentID { get; set; }

        /// <summary>
        /// 节点标识
        /// </summary>
        [JsonProperty("id")]
        public long? ID { get; set; }

        /// <summary>
        /// 节点路径
        /// </summary>
        [JsonProperty("path")]
        public string Path { get; set; }

        /// <summary>
        /// 所属层级
        /// </summary>
        [JsonProperty("layer")]
        public int? Layer { get; set; }
    }
}
