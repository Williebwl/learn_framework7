using BIStudio.Framework;
using BIStudio.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Data.MySql
{
    public class ServiceRegistry : ApplicationModule
    {
        protected override void Init()
        {
            DBUtils.RegisterDBAdapter("MySql.Data.MySqlClient", typeof(DBAdapter));
            DBUtils.RegisterDBAnalyzer("MySql.Data.MySqlClient", typeof(DBAnalyzer));
        }

    }
}
