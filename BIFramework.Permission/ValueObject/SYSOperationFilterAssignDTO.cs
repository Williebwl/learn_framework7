using BIStudio.Framework.Utils;
using System.ComponentModel;

namespace BIStudio.Framework.Permission
{
    /// <summary>
    /// 分配过滤器到操作
    /// </summary>
    public struct SYSOperationFilterAssignDTO
    {
        public SYSOperationFilterAssignDTO(long systemId, string operationCode, string filterCode)
            : this()
        {
            this.SystemId = systemId;
            this.OperationCode = operationCode;
            this.FilterCode = filterCode;
        }
        public long SystemId { get; set; }
        public string OperationCode { get; set; }
        public string FilterCode { get; set; }
    }
    /// <summary>
    /// 将过滤器附加到操作
    /// </summary>
    [Description("将过滤器附加到操作")]
    public enum SYSOperationFilterAssignResult
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
        /// 过滤器未找到
        /// </summary>
        [Description("过滤器未找到")]
        FilterNotFound,
        /// <summary>
        /// 策略未找到
        /// </summary>
        [Description("操作未找到")]
        OperationNotFound,
        /// <summary>
        /// 策略未找到
        /// </summary>
        [Description("过滤器已经绑定到该操作")]
        OperationFilterAlreadyExist,
    }
}
