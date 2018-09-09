using BIStudio.Framework.Utils;
using System.ComponentModel;

namespace BIStudio.Framework.Permission
{
    /// <summary>
    /// 分配用户角色
    /// </summary>
    public struct SYSStrategyOperationAssignDTO
    {
        public SYSStrategyOperationAssignDTO(long systemId, string strategyCode, string operationCode)
            : this()
        {
            this.SystemId = systemId;
            this.StrategyCode = strategyCode;
            this.OperationCode = operationCode;
        }
        public long SystemId { get; set; }
        public string StrategyCode { get; set; }
        public string OperationCode { get; set; }
    }
    /// <summary>
    /// 将用户组附加到策略
    /// </summary>
    [Description("将操作附加到策略")]
    public enum SYSStrategyOperationAssignResult
    {
        /// <summary>
        /// 操作成功
        /// </summary>
        [Description("操作成功")]
        Success = 0,
        /// <summary>
        /// 操作失败
        /// </summary>
        [Description("{0}")]
        Fail,
        /// <summary>
        /// 系统代码无效
        /// </summary>
        [Description("系统代码无效")]
        SystemCodeInvalid,
        /// <summary>
        /// 角色未找到
        /// </summary>
        [Description("操作点未找到")]
        OperationNotFound,
        /// <summary>
        /// 账户未找到
        /// </summary>
        [Description("策略未找到")]
        StrategyNotFound,
        /// <summary>
        /// 操作策略已经存在
        /// </summary>
        [Description("操作策略已经存在")]
        StrategyOperationAlreadyExist,
    }
}
