using System;
using System.Collections.Generic;
using System.Text;

namespace BIStudio.Framework.Security.Organization
{
    public abstract class AccessFactory
    {
        public static IModuleAccess Instance(int flag)
        {
            IModuleAccess access=null;
            switch (flag)
            {
                case 0:
                    access = new UserBO();
                    break;
                case 1:
                    access = new RoleBO();
                    break;
            }
            return access;
        }
    }
}
