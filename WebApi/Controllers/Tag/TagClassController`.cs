using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Tag;
using BIStudio.Framework.UI;

namespace WebApi.Controllers.Tag
{
    public partial class TagClassController : ApplicationService<TagClassVM, TagQuery, SYSTagClass>
    {
        public TagClassController() : base("ClassName") { }

        protected override ISpecification<SYSTagClass> GetQueryParams<T>(T info)
        {
            var spec = base.GetQueryParams(info);

            if (info != null && info.TagGroupID.HasValue) spec.Sql.And(d => d.Eq(new { TagGroupID = info.TagGroupID }));

            return spec;
        }
    }
}