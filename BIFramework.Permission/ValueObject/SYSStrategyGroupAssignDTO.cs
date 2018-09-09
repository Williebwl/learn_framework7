using BIStudio.Framework.Utils;
using System.ComponentModel;

namespace BIStudio.Framework.Permission
{
    /// <summary>
    /// 分配用户角色
    /// </summary>
    public struct SYSStrategyGroupAssignDTO
    {
        public SYSStrategyGroupAssignDTO(long systemId, string groupCode, string strategyCode)
            : this()
        {
            this.SystemId = systemId;
            this.GroupCode = groupCode;
            this.StrategyCode = strategyCode;
        }
        public long SystemId { get; set; }
        public string GroupCode { get; set; }
        public string StrategyCode { get; set; }
    }
    /// <summary>
    /// 将用户组附加到策略
    /// </summary>
    [Description("将用户组附加到策略")]
    public enum SYSStrategyGroupAssignResult
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
        /// 角色未找到
        /// </summary>
        [Description("角色未找到")]
        GroupNotFound,
        /// <summary>
        /// 账户未找到
        /// </summary>
        [Description("策略未找到")]
        StrategyNotFound,
        /// <summary>
        /// 策略和用户组已存在
        /// </summary>
        [Description("策略和用户组已存在")]
        StrategyGroupAlreadyExist,
    }
}
