using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.File
{
    using BIStudio.Framework;

    public class ServiceRegistry : ApplicationModule
    {
        protected override void Init()
        {
            CFAspect.RegisterType<IBiAttach, STDAttachBO>();
        }
    }
}
