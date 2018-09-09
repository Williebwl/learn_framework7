using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIStudio.Framework.Domain;

namespace BIStudio.Framework.Tag.Internal
{
    public abstract class SYSTagBase<T> : Repository<T>
        where T : Entity, new()
    {
        public SYSTagBase()
        {
            this.OnGet += SYSTagBase_OnGet;
        }

        void SYSTagBase_OnGet(ref ISpecification<T> specification)
        {
            specification = new Spec<T>("SystemID=0" + (this.systemID.HasValue ? " or SystemID=" + this.systemID : "")).And(specification);
        }

        internal long? systemID = null;
        public override bool Add(T entity)
        {
            if (entity != null && entity.Property.GetValue("SystemID") == null && this.systemID != null)
                entity.Property.SetValue("SystemID", this.systemID);
            return base.Add(entity);
        }
        
    }
}
