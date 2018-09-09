//===================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 实体验证
    /// </summary>
    public interface IModelValidator
    {
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool IsValid(IValidatableObject item);

        /// <summary>
        /// 获取验证信息
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        ICollection<ValidationResult> Validate(IValidatableObject item);
        
        /// <summary>
        /// 获取验证表达式
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        IDictionary<string, object> GetPropertyValidationJson(Type item);
    }
}
