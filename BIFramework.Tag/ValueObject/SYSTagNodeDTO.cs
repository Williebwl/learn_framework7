using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using BIStudio.Framework.Domain;


namespace BIStudio.Framework.Tag
{
    /// <summary>
    /// 标签节点信息，包含标签和标签项
    /// </summary>
    public class SYSTagNodeDTO
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long? ID { get; set;}
        /// <summary>
        /// 节点类型
        /// </summary>
        public EnumSYSTagNodeType NodeType { get; set; }
        /// <summary>
        /// 父级主键ID
        /// </summary>
        public long? ParentID { get; set; }
        /// <summary>
        /// 父级节点类型
        /// </summary>
        public EnumSYSTagNodeType ParentNodeType { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 标示符
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 是否内置标签
        /// </summary>
        public int? IsBuiltIn { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int? DisplayLevel { get; set; }
        /// <summary>
        /// 序列号
        /// </summary>
        public int? Sequence { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 数据源
        /// </summary>
        [IgnoreDataMember]
        public Entity DataSource { get; set; }

        public static SYSTagNodeDTO Parse(SYSTagGroup tagInfo)
        {
            return new SYSTagNodeDTO
            {
                ID = tagInfo.ID,
                NodeType = EnumSYSTagNodeType.TagGroup,
                ParentID = null,
                ParentNodeType = EnumSYSTagNodeType.TagGroup,
                Name = tagInfo.GroupName,
                Code = tagInfo.GroupCode,
                //Value = tagInfo.GroupValue,
                //IsBuiltIn = tagInfo.IsBuiltIn,
                //DisplayLevel = null,
                Sequence = tagInfo.Sequence,
                Remark = tagInfo.Remark,
                DataSource = tagInfo
            };
        }
        public static SYSTagNodeDTO Parse(SYSTagClass tagInfo)
        {
            return new SYSTagNodeDTO
            {
                ID = tagInfo.ID,
                NodeType = EnumSYSTagNodeType.TagClass,
                ParentID = tagInfo.AppID,
                ParentNodeType = EnumSYSTagNodeType.TagGroup,
                Name = tagInfo.ClassName,
                Code = tagInfo.ClassCode,
                Value = tagInfo.ClassValue,
                IsBuiltIn = tagInfo.IsBuiltIn,
                DisplayLevel = tagInfo.DisplayLevel,
                Sequence = tagInfo.Sequence,
                Remark = tagInfo.Remark,
                DataSource = tagInfo
            };
        }
        public static SYSTagNodeDTO Parse(SYSTag tagInfo)
        {
            SYSTagNodeDTO node = new SYSTagNodeDTO
            {
                ID = tagInfo.ID,
                NodeType = EnumSYSTagNodeType.Tag,
                Name = tagInfo.TagName,
                Code = tagInfo.TagCode,
                Value = tagInfo.TagValue,
                IsBuiltIn = tagInfo.IsBuiltIn,
                DisplayLevel = null,
                Sequence = tagInfo.Sequence,
                Remark = tagInfo.Remark,
                DataSource = tagInfo
            };
            if (tagInfo.ParentID == 0)
            {                
                node.ParentID = tagInfo.TagClassID;
                node.ParentNodeType = EnumSYSTagNodeType.TagClass;
            }
            else
            {
                node.ParentID = tagInfo.ParentID;
                node.ParentNodeType = EnumSYSTagNodeType.Tag;
            }
            return node;
        }
    }
}
