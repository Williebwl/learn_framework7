using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.MQ
{
    /// <summary>
    /// 命令
    /// </summary>
    public abstract class Command : Message, ICommand
    {
    }
}
