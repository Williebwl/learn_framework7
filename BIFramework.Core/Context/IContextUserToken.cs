using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework
{
    public interface IContextUserToken : IIdentity
    {
        string[] Operations { get; }
        bool HasPermission(string operation);
    }
}
