using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BIStudio.Framework.Tag.Internal;
using BIStudio.Framework.Domain;

namespace BIStudio.Framework.Tag
{
    /// <summary>
    /// 规约--前台读取数据
    /// </summary>
    public class FontReadOperateSpecification<TEntity> : OperateSpecification<TEntity>, ISpecification<TEntity>
        where TEntity : Entity
    {
        public FontReadOperateSpecification() : base() { }
        public FontReadOperateSpecification(int userID) : base(userID) { }

        protected override EnumSYSTagOperate OperateType
        {
            get { return EnumSYSTagOperate.FontRead; }
        }

    }
}
