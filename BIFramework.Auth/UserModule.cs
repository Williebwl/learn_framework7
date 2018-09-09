using BIStudio.Framework;
using BIStudio.Framework.Domain;


namespace BIStudio.Framework.Auth
{
    public class UserModule : ApplicationModule
    {
        public struct Operations
        {
            public const string USER_DEFAULT = "USER_DEFAULT";
        }
        public struct Groups
        {
            public const string USER_DEFAULT = "USER_DEFAULT";
        }
        public struct Users
        {
            public const string USER_DEFAULT = "USER_DEFAULT";
        }
        protected override void Init()
        {
            CFAspect.RegisterType<IRepository<SYSPassport>, Repository<SYSPassport>>();
            CFAspect.RegisterType<IRepository<SYSAccount>, Repository<SYSAccount>>();
            CFAspect.RegisterType<IRepository<SYSToken>, Repository<SYSToken>>();
        }
    }
}
