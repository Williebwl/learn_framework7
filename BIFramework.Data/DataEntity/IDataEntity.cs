using System;

namespace BIStudio.Framework.Data
{
    /// <summary>
    /// 表示一个数据实体
    /// </summary>
    public interface IDataEntity
    {
        /// <summary>
        /// 数据实体定义
        /// </summary>
        DataEntityDefinition Property { get; }
    }
}
