using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 表示逻辑删除
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        bool? IsDelete { get; set; }
    }
}
