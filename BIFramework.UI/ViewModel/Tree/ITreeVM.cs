using System;
using System.Collections.Generic;

namespace BIStudio.Framework.UI
{
    /// <summary>
    /// 树节点
    /// </summary>
    public interface ITreeVM : IViewModel
    {
        /// <summary>
        /// 节点标示
        /// </summary>
        long? ID { get; set; }

        /// <summary>
        /// 父节点标示
        /// </summary>
        long? ParentID { get; set; }

        /// <summary>
        /// 节点路径
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// 节点所属层级
        /// </summary>
        int? Layer { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        IList<ITreeVM> Children { get; set; }
    }
}
