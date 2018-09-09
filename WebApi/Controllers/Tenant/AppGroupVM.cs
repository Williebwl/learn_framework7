using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIStudio.Framework.UI;
using WebApi.Controllers.Institution;

namespace WebApi.Controllers.Tenant
{
    public class AppGroupVM : ViewModel
    {
        public long? AppID { get; set; }

        public IList<GroupVM> AppGroups { get; set; }

        public IList<GroupVM> AllGroups { get; set; }
    }
}