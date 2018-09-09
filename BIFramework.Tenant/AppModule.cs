using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Tenant
{
    public class AppModule : ApplicationModule
    {
        public struct Operations
        {
        }
        public struct Groups
        {
        }
        protected override void Init()
        {
            CFAspect.RegisterType<IRepository<SYSSystem>, Repository<SYSSystem>>();
            CFAspect.RegisterType<IRepository<SYSSystemCertificate>, Repository<SYSSystemCertificate>>();
            CFAspect.RegisterType<IRepository<SYSApp>, Repository<SYSApp>>();
            CFAspect.RegisterType<IRepository<SYSAppAccess>, Repository<SYSAppAccess>>();
            CFAspect.RegisterType<IRepository<SYSMenu>, Repository<SYSMenu>>();
        }
    }
}
