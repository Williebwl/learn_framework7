using BIStudio.Framework.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 表示领域实体
    /// </summary>
    public interface IEntity : IDataEntity, ITransientDependency, IValidatableObject
    {
        /// <summary>
        /// 主键
        /// </summary>
        long? ID { get; set; }
    }
}