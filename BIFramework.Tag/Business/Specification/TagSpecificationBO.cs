using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIStudio.Framework.Tag.Internal;
using System.Reflection;
using BIStudio.Framework.Utils;

using BIStudio.Framework.Domain;


namespace BIStudio.Framework.Tag
{
    public class TagSpecificationBO : TransientDependency
    {
        private ISpecification<TEntity> GetSpecification<TEntity>(EnumSYSTagOperate operates, int userID)
            where TEntity : Entity
        {
            switch (operates)
            {
                case EnumSYSTagOperate.Create:
                    return new CreateOperateSpecification<TEntity>(userID);
                case EnumSYSTagOperate.Update:
                    return new UpdateOperateSpecification<TEntity>(userID);
                case EnumSYSTagOperate.Read:
                    return new ReadOperateSpecification<TEntity>(userID);
                case EnumSYSTagOperate.Delete:
                    return new DeleteOperateSpecification<TEntity>(userID);
                case EnumSYSTagOperate.FullControl:
                    return new FullControlOperateSpecification<TEntity>(userID);
                case EnumSYSTagOperate.FontRead:
                    return new FontReadOperateSpecification<TEntity>(userID);
            }
            return new ReadOperateSpecification<TEntity>(userID);
        }
        /// <summary>
        /// 获得指定类型的契约，如果目标包含多个类型，则使用AND连接
        /// </summary>
        /// <param name="operates"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public ISpecification<TEntity> GetSpecification<TEntity>(EnumSYSTagOperate? operates, long? userID) 
            where TEntity : Entity 
        {
            if (operates == null)
                return null;

            ISpecification<TEntity> specification = null;
            foreach (EnumSYSTagOperate operate in Enum.GetValues(typeof(EnumSYSTagOperate)))
            {
                if ((operates | operate) == operates)
                {
                    var temp = GetSpecification<TEntity>(operate, (userID ?? CFContext.User.ID));
                    specification = (specification == null ? temp :new  AndSpecification<TEntity>(specification, temp)); ;
                }
            }
            return specification;
        }
    }
}
