using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Tenant
{
    public class AppMenuNameSpec : SpecificationBase<SYSMenu>
    {
        public AppMenuNameSpec(string menuName)
        {
            Sql = DBBuilder.Define("ISNULL(MenuName,'')+ISNULL(SampleName,'') LIKE @Key", new { Key = "%" + menuName + "%" });
        }
    }
}
