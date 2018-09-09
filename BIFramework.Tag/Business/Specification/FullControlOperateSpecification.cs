using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BIStudio.Framework.Tag.Internal;
using BIStudio.Framework.Domain;

namespace BIStudio.Framework.Tag
{
    /// <summary>
    /// 规约--完全控制数据
    /// </summary>
    public class FullControlOperateSpecification<TEntity> : OperateSpecification<TEntity>, ISpecification<TEntity>
        where TEntity : Entity
    {
        public FullControlOperateSpecification() : base() { }
        public FullControlOperateSpecification(int userID) : base(userID) { }

        protected override EnumSYSTagOperate OperateType
        {
            get { return EnumSYSTagOperate.FullControl; }
        }

    }
}
