using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using BIStudio.Framework.Domain;



namespace BIStudio.Framework.Tag
{
    public abstract class TagService
    {
        /// <summary>
        /// 返回标签管理接口，并筛选返回值为List&lt;DataEntityBase&gt;的查询
        /// </summary>
        /// <param name="specification">查询筛选器</param>
        /// <returns></returns>
        public static ITag GetInstance(ISpecification<Entity> specification = null)
        {
            return new Tag(specification);
        }

        public static readonly ITag Default = new Tag();

    }
}
