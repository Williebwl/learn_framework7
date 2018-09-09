using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 领域服务
    /// </summary>
    public abstract class DomainService : TransientDependency, IDomainService
    {
    }
}
