using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Data
{
    /// <summary>
    /// 表示一次简单分页查询
    /// </summary>
    public class PagedQuery : Query
    {
        /// <summary>
        /// 分页索引
        /// </summary>
        public int? PageIndex { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int? PageSize { get; set; }
    }
}
