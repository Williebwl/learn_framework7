using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 表示上下文依赖解析器
    /// </summary>
    public interface IContextResolver : IAspectResolver<ITransientDependency>
    {
    }
}
