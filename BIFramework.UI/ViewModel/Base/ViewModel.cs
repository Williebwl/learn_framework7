using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BIStudio.Framework.Domain;

namespace BIStudio.Framework.UI
{
    /// <summary>
    /// 视图模型
    /// </summary>
    public class ViewModel : IViewModel
    {
        /// <summary>
        /// 调用系统验证方法
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }

        /// <summary>
        ///  创建当前对象的浅表副本。
        /// </summary>
        /// <typeparam name="T">当前对象类型</typeparam>
        /// <returns>当前对象的浅表副本。</returns>
        public virtual T Clone<T>() where T : ViewModel
        {
            return (T)this.MemberwiseClone();
        }
    }
}
