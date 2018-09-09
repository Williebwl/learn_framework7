using BIStudio.Framework.Utils;

namespace BIStudio.Framework.Permission
{
    /// <summary>
    /// 过滤器操作
    /// </summary>
    public enum EnumFilterOperation
    {
        None,
        [ALEnumDescription(">")]
        GreaterThan,
        [ALEnumDescription(">=")]
        GreaterThanOrEqual,
        [ALEnumDescription("<")]
        LessThan,
        [ALEnumDescription("<=")]
        LessThanOrEqual,
        [ALEnumDescription("=")]
        Equal,
        [ALEnumDescription("!=")]
        NotEqual,
        [ALEnumDescription("like")]
        Like,
        [ALEnumDescription("not like")]
        NotLike
    }
}