using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    public static class ModelValidatorExtension
    {
        /// <summary>
        /// 获取验证信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ICollection<ValidationResult> Validate(this IValidatableObject model)
        {
            return ModelValidator.Default.Validate(model);
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool IsValid(this IValidatableObject model)
        {
            return ModelValidator.Default.IsValid(model);
        }
    }
}
