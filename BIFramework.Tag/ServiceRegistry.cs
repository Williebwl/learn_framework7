using BIStudio.Framework;
using BIStudio.Framework.Domain;

namespace BIStudio.Framework.Tag
{
    public class ServiceRegistry : ApplicationModule
    {
        protected override void Init()
        {
            CFAspect.RegisterType<IRepository<SYSTagGroup>, Repository<SYSTagGroup>>();
            CFAspect.RegisterType<IRepository<SYSTagClass>, Repository<SYSTagClass>>();
            CFAspect.RegisterType<IRepository<SYSTag>, Repository<SYSTag>>();
        }
    }
}
