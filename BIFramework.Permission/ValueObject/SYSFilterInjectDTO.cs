using BIStudio.Framework.Utils;
using System.ComponentModel;

namespace BIStudio.Framework.Permission
{
    /// <summary>
    /// 将过滤器附加到资源点
    /// </summary>
    public struct SYSFilterInjectDTO
    {
        public SYSFilterInjectDTO(long systemId, long resourceTagID, string filterName, string filterCode, string entityName, string propertyName, string validatorDescription, string value)
            : this()
        {
            this.SystemId = systemId;
            this.ResourceTagID = resourceTagID;
            this.FilterName = filterName;
            this.FilterCode = filterCode;
            ValidatorDescription = validatorDescription;
            EntityName = entityName;
            PropertyName = propertyName;
            Value = value;
        }
        public long SystemId { get; set; }
        public long ResourceTagID { get; set; }
        public string FilterName { get; set; }
        public string FilterCode { get; set; }
        public int? Sequence { get; set; }
        public string Remarks { get; set; }
        public string ValidatorDescription { get; set; }
        public string EntityName { get; set; }
        public string PropertyName { get; set; }
        public string Value { get; set; }
    }
    /// <summary>
    /// 将过滤器附加到资源点
    /// </summary>
    [Description("将过滤器附加到资源点")]
    public enum SYSFilterInjectResult
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
        [Description("未指定过滤器名称或代码")]
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
