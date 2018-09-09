using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework
{
    public interface IContextUser : IPrincipal
    {
        long ID { get; }
        string IP { get; }
        string LoginName { get; }
        string UserName { get; }
        long SystemID { get; }
        string[] Groups { get; }
        IContextUserToken Identity { get; }
    }
}
