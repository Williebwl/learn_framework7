using BIStudio.Framework.Utils;
using System.ComponentModel;

namespace BIStudio.Framework.Institution
{
    /// <summary>
    /// 分配用户角色
    /// </summary>
    public struct SYSGroupUserAssignDTO
    {
        public SYSGroupUserAssignDTO(long systemId, string groupCode, long accountId)
            : this()
        {
            this.SystemId = systemId;
            this.GroupCode = groupCode;
            this.AccountID = accountId;
        }
        public long SystemId { get; set; }
        public string GroupCode { get; set; }
        public long AccountID { get; set; }
    }
    public enum SYSGroupUserAssignResult
    {
        /// <summary>
        /// 操作成功
        /// </summary>
        [Description("操作成功")]
        Success = 0,
        /// <summary>
        /// 操作失败
        /// </summary>
        [Description("操作成功")]
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
        [Description("账户未找到")]
        AccountNotFound,
        /// <summary>
        /// 用户角色已分配
        /// </summary>
        [Description("用户角色已分配")]
        GroupAccountAlreadyExists,
    }
}
