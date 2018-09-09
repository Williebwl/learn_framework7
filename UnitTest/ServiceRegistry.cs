using BIStudio.Framework;
using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIFramework.Test
{
    public class ServiceRegistry : ApplicationModule
    {
        protected override void Init()
        {
            CFAspect.RegisterType<IRepository<TCTest>, TCTestEntityFrameworkBO>();
        }
    }
}
