using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using BIStudio.Framework.UI;

namespace WebApi.Controllers.Tag
{
    public class TagQuery : PagedQuery
    {
        /// <summary>
        /// 标签项id
        /// </summary>
        public long? TagID { get; set; }

        /// <summary>
        /// 标签分类id
        /// </summary>
        public long? TagClassID { get; set; }

        /// <summary>
        /// 标签分组id
        /// </summary>
        public long? TagGroupID { get; set; }


    }
}