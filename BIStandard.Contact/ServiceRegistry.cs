using BIStudio.Framework;
using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Standard.Contact
{
    public class ServiceRegistry : ApplicationModule
    {
        protected override void Init()
        {
            CFAspect.RegisterType<IRepository<STDContact>, Repository<STDContact>>();
            CFAspect.RegisterType<IRepository<STDContactDetail>, Repository<STDContactDetail>>();
            //11111
        }
    }
}
