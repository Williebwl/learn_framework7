using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIStudio.Framework.UI;
using BIStudio.Framework.Institution;
using BIStudio.Framework.Data;

namespace WebApi.Controllers.Institution
{
    public class DeptController : ApplicationService<DeptVM, PagedQuery, SYSDept>
    {
        public DeptController() : base("Name", "ShortName") { }
    }
}