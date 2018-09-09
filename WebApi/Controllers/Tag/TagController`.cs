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
    public partial class TagController : ApplicationService<TagVM, TagQuery, SYSTag>
    {
        protected IRepository<SYSTag> _tagRepository;
        protected IRepository<SYSTagClass> _tagClassRepository;

        public TagController() : base("tag.TagName") { }

        protected override ISpecification<SYSTag> GetQueryParams<T>(T info)
        {
            var sql = @"SELECT  tag.* ,
                                tg.TagName AS ParentName
                        FROM    SYSTag tag
                                LEFT JOIN SYSTag tg ON tag.ParentID > 0
                                                        AND tg.ID =tag.ParentID
                        WHERE   1 = 1";

            var dbBuilder = DBBuilder.Define(sql);

            if (info != null)
            {
                if (info.TagID.HasValue) dbBuilder.Append("AND (tag.ID=@ID OR tag.ParentID=@ID)", new { ID = info.TagID });
                else if (info.TagClassID.HasValue) dbBuilder.Append("AND tag.TagClassID=@TagClassID", new { TagClassID = info.TagClassID });
                else if (info.TagGroupID.HasValue) dbBuilder.Append("AND EXISTS(SELECT 1 FROM SYSTagClass tc WHERE tc.ID=tag.TagClassID AND tc.TagGroupID=@GroupID)", new { GroupID = info.TagGroupID });
            }

            return new Spec<SYSTag>(dbBuilder);
        }

      
    }
}