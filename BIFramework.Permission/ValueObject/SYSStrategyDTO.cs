﻿using BIStudio.Framework.Utils;
using System.ComponentModel;

namespace BIStudio.Framework.Permission
{
    /// <summary>
    /// 分配用户角色
    /// </summary>
    public struct SYSStrategyDTO
    {
        public SYSStrategyDTO(long systemId, long resourceTagID, string name, string code)
            : this()
        {
            SystemId = systemId;
            ResourceTagID = resourceTagID;
            Name = name;
            Code = code;
        }
        public long SystemId { get; set; }
        public long ResourceTagID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? Sequence { get; set; }
        public string Remarks { get; set; }
    }

    /// <summary>
    /// 将操作附加到资源点
    /// </summary>
    [Description("将策略附加到资源点")]
    public enum SYSStrategyResult
    {
        /// <summary>
        /// 操作成功
        /// </summary>
        [Description("操作成功")]
        Success = 0,
        /// <summary>
        /// 操作失败
        /// </summary>
        [Description("操作失败")]
        Fail,
        /// <summary>
        /// 系统代码无效
        /// </summary>
        [Description("系统代码无效")]
        SystemCodeInvalid,
        /// <summary>
        /// 未指定操作名称或代码
        /// </summary>
        [Description("未指定操作名称或代码")]
        NameOrCodeNotFound,
        /// <summary>
        /// 资源点无效
        /// </summary>
        [Description("资源点无效")]
        ResourceTagInvalid,
        /// <summary>
        /// 操作代码已存在
        /// </summary>
        [Description("操作代码已存在")]
        CodeAlreadyExists,
    }
}
