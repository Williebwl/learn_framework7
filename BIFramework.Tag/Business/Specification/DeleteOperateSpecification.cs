using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BIStudio.Framework.Tag.Internal;
using BIStudio.Framework.Domain;

namespace BIStudio.Framework.Tag
{
    /// <summary>
    /// 规约--删除数据
    /// </summary>
    public class DeleteOperateSpecification<TEntity> : OperateSpecification<TEntity>
        where TEntity : Entity
    {
        public DeleteOperateSpecification() : base() { }
        public DeleteOperateSpecification(int userID) : base(userID) { }

        protected override EnumSYSTagOperate OperateType
        {
            get { return EnumSYSTagOperate.Delete; }
        }

    }
}
