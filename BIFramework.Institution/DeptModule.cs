using BIStudio.Framework;
using BIStudio.Framework.Auth;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Institution
{
    public class DeptModule : ApplicationModule
    {
        protected override void Init()
        {
            CFAspect.RegisterType<IRepository<SYSDept>, Repository<SYSDept>>();
            CFAspect.RegisterType<IRepository<SYSDept>, Repository<SYSDept>>();
            CFAspect.RegisterType<IAppService, AppService>();
            CFAspect.RegisterType<IMenuService, MenuService>();
        }
    }
}
