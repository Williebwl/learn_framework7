using BIStudio.Framework;
using BIStudio.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Data.Sql2005
{
    public class ServiceRegistry : ApplicationModule
    {
        protected override void Init()
        {
            DBUtils.RegisterDBAdapter("System.Data.SqlClient", typeof(DBAdapter));
            DBUtils.RegisterDBAnalyzer("System.Data.SqlClient", typeof(DBAnalyzer));
        }

    }
}
