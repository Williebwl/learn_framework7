using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BIStudio.Framework;
using System.Web.Script.Serialization;
using System.Collections.ObjectModel;
using BIStudio.Framework.Utils;
using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 基于 Data Annotations的验证 
    /// 使用IValidatableObject 
    /// 和使用ValidationAttribute来进行验证
    /// </summary>
    public class ModelValidator : IModelValidator
    {
        /// <summary>
        /// 获得默认的验证器
        /// </summary>
        public static readonly IModelValidator Default = new ModelValidator();

        #region IEntityValidator Members
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsValid(IValidatableObject item)
        {
            if (item == null)
                return false;
            var errors = new Collection<ValidationResult>();
            var context = new ValidationContext(item);
            Validator.TryValidateObject(context.ObjectInstance, context, errors, true);
            return !errors.Any();
        }

        /// <summary>
        /// 获取错误消息
        /// </summary>
        public ICollection<ValidationResult> Validate(IValidatableObject item)
        {
            if (item == null)
                return null;
            var errors = new Collection<ValidationResult>();
            var context = new ValidationContext(item);
            Validator.TryValidateObject(context.ObjectInstance, context, errors, true);
            return errors;
        }
        
        /// <summary>
        /// 获取验证表达式
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IDictionary<string, object> GetPropertyValidationJson(Type item)
        {
            var q = from d in TypeDescriptor.GetProperties(item).AsParallel().Cast<PropertyDescriptor>()
                    let attrs = d.Attributes.OfType<ValidationAttribute>()
                    where attrs.Any()
                    let display = d.Attributes.OfType<DisplayAttribute>()
                    select new { d.Name, display = display.Any() ? display.First().Name : d.Name, Attr = attrs.ToArray() };

            return q.ToDictionary(d => d.Name, d => GetPropertyValidationObject(d.display, d.Attr) as object);
        }

        private IDictionary<string, object> GetPropertyValidationObject(string display, ValidationAttribute[] attrs)
        {
            return attrs.Aggregate(new Dictionary<string, object> { { "display", display } }, (atrs, attr) => GetPropertyValidationObject(atrs, display, attr));
        }

        private readonly static string[] NotAttr = { "MatchTimeoutInMilliseconds", "OperandType" };

        private Dictionary<string, object> GetPropertyValidationObject(Dictionary<string, object> atrs, string display, ValidationAttribute attr)
        {
            var msg = attr.FormatErrorMessage(display);

            if (attr is RequiredAttribute)
            {
                var atr = attr as RequiredAttribute;

                if (!atr.AllowEmptyStrings) atrs.Add("required", string.IsNullOrEmpty(msg) ? (object)true : msg);

                goto End;
            }

            var attrs = (attr.TypeId as Type).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                                             .Where(d => !NotAttr.Contains(d.Name))
                                             .Select(atr => new { atr.Name, val = atr.GetValue(attr) })
                                             .Where(d => d.val != null)
                                             .ToDictionary(d => d.Name.ToLower(), d => d.val);

            if (!string.IsNullOrEmpty(msg)) attrs["msg"] = msg;

            //if (attr is DataTypeAttribute)
            //{
            //    var regex = (attr.TypeId as Type).GetField("_regex", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

            //    if (regex != null) attrs["pattern"] = regex.GetValue(attr).ToString();
            //}

            var key = (attr.TypeId as Type).Name.Replace("Attribute", string.Empty).ToLower();

            atrs[key] = attrs.Any() ? (object)attrs : true;

            End: return atrs;
        }
        
        #endregion

    }
}
