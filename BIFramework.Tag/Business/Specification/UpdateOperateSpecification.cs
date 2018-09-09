using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BIStudio.Framework.Tag.Internal;
using BIStudio.Framework.Domain;

namespace BIStudio.Framework.Tag
{
    /// <summary>
    /// 规约--更新数据
    /// </summary>
    public class UpdateOperateSpecification<TEntity> : OperateSpecification<TEntity>
        where TEntity : Entity
    {
        public UpdateOperateSpecification() : base() { }
        public UpdateOperateSpecification(long userID) : base(userID) { }

        protected override EnumSYSTagOperate OperateType
        {
            get { return EnumSYSTagOperate.Update; }
        }

    }
}
