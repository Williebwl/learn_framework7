using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIStudio.Framework.Utils;
using BIStudio.Framework;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Tag.Internal;

namespace BIStudio.Framework.Tag
{
    /// <summary>
    /// 标签节点管理
    /// </summary>
    internal class SYSTagNodeBO : SYSTagBase<SYSTag>
    {
        /// <summary>
        /// 修复断开的节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        internal List<SYSTagNodeDTO> FixBrokenNodes(List<SYSTagNodeDTO> nodes)
        {
            SYSTagGroupBO tagGroupBO = (this.systemID.HasValue ? this.Context.Resolve<SYSTagGroupBO>().With(d => d.systemID = this.systemID.Value) : this.Context.Resolve<SYSTagGroupBO>());
            SYSTagClassBO tagClassBO = (this.systemID.HasValue ? this.Context.Resolve<SYSTagClassBO>().With(d => d.systemID = this.systemID.Value) : this.Context.Resolve<SYSTagClassBO>());
            SYSTagBO tagBO = (this.systemID.HasValue ? this.Context.Resolve<SYSTagBO>().With(d => d.systemID = this.systemID.Value) : this.Context.Resolve<SYSTagBO>());
            //从叶子节点开始遍历
            nodes.Sort(this.TagNodeComparer);
            nodes.Reverse();
            int index = -1;
            while (index++ < nodes.Count - 1)
            {
                //已达到根节点,退出
                long? parentNodeID = nodes[index].ParentID;
                EnumSYSTagNodeType parentNodeType = nodes[index].ParentNodeType;
                if (!parentNodeID.HasValue)
                    continue;
                //将当前节点的父节点加入队列
                if (!nodes.Exists(d => parentNodeID == d.ID && parentNodeType == d.NodeType))
                {
                    switch (parentNodeType)
                    {
                        case EnumSYSTagNodeType.TagGroup:
                            nodes.Add(SYSTagNodeDTO.Parse(tagGroupBO.Get(parentNodeID ?? 0)));
                            break;
                        case EnumSYSTagNodeType.TagClass:
                            nodes.Add(SYSTagNodeDTO.Parse(tagClassBO.Get(parentNodeID ?? 0)));
                            break;
                        case EnumSYSTagNodeType.Tag:
                            nodes.Add(SYSTagNodeDTO.Parse(tagBO.Get(parentNodeID ?? 0)));
                            break;
                    }
                }
            }
            nodes.Sort(this.TagNodeComparer);
            return nodes;
        }
        /// <summary>
        /// 标签节点比较器
        /// </summary>
        private Comparison<SYSTagNodeDTO> TagNodeComparer
        {
            get
            {
                return (node1, node2) =>
                {
                    //比较标签类型
                    int value = ((int)node1.NodeType).CompareTo((int)node2.NodeType);
                    if (value != 0)
                        return value;
                    //比较标签排序
                    if (node1.NodeType == EnumSYSTagNodeType.TagGroup || node1.NodeType == EnumSYSTagNodeType.TagClass)
                        return (node1.Sequence ?? 0).CompareTo(node2.Sequence ?? 0);
                    //比较标签深度
                    SYSTag tag1 = (SYSTag)node1.DataSource;
                    SYSTag tag2 = (SYSTag)node2.DataSource;
                    if (tag1.Layer == tag2.Layer)
                        return (tag1.Sequence ?? 0).CompareTo(tag2.Sequence);
                    else
                        return (tag1.Layer ?? 0).CompareTo(tag2.Layer);
                };
            }
        }

    }
}
