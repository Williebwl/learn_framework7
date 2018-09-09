using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using BIStudio.Framework.Tag.Internal;
using BIStudio.Framework.Domain;

namespace BIStudio.Framework.Tag
{
    /// <summary>
    /// 规约--创建数据
    /// </summary>
    public class CreateOperateSpecification<TEntity> : OperateSpecification<TEntity>
        where TEntity : Entity
    {
        public CreateOperateSpecification() : base() { }
        public CreateOperateSpecification(int userID) : base(userID) { }

        protected override EnumSYSTagOperate OperateType
        {
            get { return EnumSYSTagOperate.Create; }
        }

    }
}
