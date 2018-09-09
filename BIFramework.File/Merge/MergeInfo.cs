using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.File
{
    /// <summary>
    /// 文件合并信息
    /// </summary>
    internal class MergeInfo
    {
        /// <summary>
        /// 过期时间默认为6小时
        /// </summary>
        internal DateTime Expires { get; set; }

        /// <summary>
        /// 已经合并的文件数量
        /// </summary>
        internal int Merged { get; set; }
    }
}
