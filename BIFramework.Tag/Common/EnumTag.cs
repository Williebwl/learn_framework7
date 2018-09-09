using System;
using System.Collections.Generic;
using System.Text;
using BIStudio.Framework.Utils;

namespace BIStudio.Framework.Tag
{

    /// <summary>
    /// 标签操作
    /// </summary>
    [ALEnumDescription("标签操作")]
    public enum OperateFlagEnum
    {
        /// <summary>
        /// 添加
        /// </summary>
        [ALEnumDescription("添加")]
        TagAdd = 1,

        /// <summary>
        /// 修改
        /// </summary>
        [ALEnumDescription("修改")]
        TagEdit = 2,

        /// <summary>
        /// 删除
        /// </summary>
        [ALEnumDescription("删除")]
        TagDelete = 3
    }

    /// <summary>
    /// 标签库版本
    /// </summary>
    internal enum EnumSYSTagAPIVersion
    {
        /// <summary>
        /// 经典版
        /// </summary>
        Classic = 1,
        /// <summary>
        /// 标准版
        /// </summary>
        Standard = 2
    }
    /// <summary>
    /// 标签类型
    /// </summary>
    [ALEnumDescription("标签类型")]
    public enum EnumSYSTagDisplayLevel
    {
        /// <summary>
        /// 贴入标签
        /// </summary>
        [ALEnumDescription("贴入标签")]
        System = 1,
        /// <summary>
        /// 分类标签
        /// </summary>
        [ALEnumDescription("分类标签")]
        Custom = 2,
        /// <summary>
        /// 字典
        /// </summary>
        [ALEnumDescription("字典")]
        Dict = 3
    }
    /// <summary>
    /// 标签搜索方式
    /// </summary>
    public enum EnumSYSTagSearch
    {
        /// <summary>
        /// 模糊匹配，搜索结果至少需要包含一个标签
        /// </summary>
        Fuzzy = 1,
        /// <summary>
        /// 智能匹配，搜索结果包含相关度(tApply.MatchRate)字段
        /// </summary>
        Smart = 2,
        /// <summary>
        /// 精确匹配，搜索结果必需包含全部标签
        /// </summary>
        Exact = 3
    }
    /// <summary>
    /// 标签操作权限
    /// </summary>
    [ALEnumDescription("标签操作权限")]
    public enum EnumSYSTagOperate
    {
        /// <summary>
        /// 创建
        /// </summary>
        [ALEnumDescription("创建")]
        Create = 1,
        /// <summary>
        /// 更新
        /// </summary>
        [ALEnumDescription("更新")]
        Update = 2,
        /// <summary>
        /// 读取
        /// </summary>
        [ALEnumDescription("读取")]
        Read = 4,
        /// <summary>
        /// 删除
        /// </summary>
        [ALEnumDescription("删除")]
        Delete = 8,
        /// <summary>
        /// 完全控制
        /// </summary>
        [ALEnumDescription("完全控制")]
        FullControl = 16,
        /// <summary>
        /// 前台浏览
        /// </summary>
        [ALEnumDescription("前台浏览")]
        FontRead = 32
    }
    /// <summary>
    /// 标签权限范围
    /// </summary>
    [ALEnumDescription("标签权限范围")]
    public enum EnumSYSTagRange
    {
        /// <summary>
        /// 父级标签节点
        /// </summary>
        [ALEnumDescription("父级标签节点")]
        Parents = 1,
        /// <summary>
        /// 当前标签节点
        /// </summary>
        [ALEnumDescription("当前标签节点")]
        Current = 2,
        /// <summary>
        /// 子级标签节点
        /// </summary>
        [ALEnumDescription("子级标签节点")]
        Children = 4
    }
    /// <summary>
    /// 标签权限类型
    /// </summary>
    [ALEnumDescription("标签权限类型")]
    public enum EnumSYSTagAuthorityType
    {
        /// <summary>
        /// 用户权限
        /// </summary>
        [ALEnumDescription("用户权限")]
        User = 1,
        /// <summary>
        /// 角色权限
        /// </summary>
        [ALEnumDescription("角色权限")]
        Role = 2,
        /// <summary>
        /// 部门权限
        /// </summary>
        [ALEnumDescription("部门权限")]
        Dept = 4
    }
    /// <summary>
    /// 标签节点类型
    /// </summary>
    public enum EnumSYSTagNodeType
    {
        /// <summary>
        /// 标签组
        /// </summary>
        TagGroup = 1,
        /// <summary>
        /// 标签
        /// </summary>
        TagClass = 2,
        /// <summary>
        /// 标签项
        /// </summary>
        Tag = 4
    }

}
