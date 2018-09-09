using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.UI.Models
{
    using BIStudio.Framework.Domain;
    using Newtonsoft.Json;

    /// <summary>
    /// 树模型接口
    /// </summary>
    public class SmartTreeVM : ViewModel, ITreeVM
    {
        /// <summary>
        /// 显示文本
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// 节点值
        /// </summary>
        [JsonProperty("value")]
        public object Value { get; set; }

        /// <summary>
        /// id
        /// </summary>
        [JsonProperty("id")]
        public long? ID { get; set; }

        /// <summary>
        /// 父节点id
        /// </summary>
        [JsonProperty("pid")]
        public long? ParentID { get; set; }

        /// <summary>
        /// 节点路径
        /// </summary>
        [JsonProperty("path")]
        public string Path { get; set; }

        /// <summary>
        /// 是否显示选择框
        /// </summary>
        [JsonProperty("showcheck")]
        public bool? ShowCheck { get; set; }

        /// <summary>
        /// 是否展开节点
        /// </summary>
        [JsonProperty("isexpand")]
        public bool? IsExpand { get; set; }

        /// <summary>
        /// 是否有子节点
        /// </summary>
        [JsonProperty("hasChildren")]
        public bool? HasChildren { get; set; }

        /// <summary>
        /// 节点选中状态
        /// </summary>
        [JsonProperty("checkstate")]
        public int? CheckState { get; set; }

        /// <summary>
        /// 子节点集合
        /// </summary>
        [JsonProperty("children")]
        public IList<ITreeVM> Children { get; set; }

        /// <summary>
        /// 所属层级
        /// </summary>
        [JsonProperty("layer")]
        public int? Layer { get; set; }
    }
}
