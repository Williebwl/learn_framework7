using BIStudio.Framework;

using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 数据实体类
    /// </summary>
    public abstract class Entity : TransientDependency, IEntity
    {
        public Entity()
        {
            this.Property = DataEntityUtils.Entity(this.GetType()).Clone(this);
        }

        /// <summary>
        /// 数据实体定义
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        [Column(IsExtend = true)]
        public DataEntityDefinition Property { get; private set; }

        /// <summary>
        /// DataEntity主键
        /// </summary>
        public long? ID { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            IEntity ar = obj as IEntity;
            if (ar == null)
                return false;
            return this.ID == ar.ID;
        }
        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }

        /// <summary>
        /// 调用系统验证方法
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
