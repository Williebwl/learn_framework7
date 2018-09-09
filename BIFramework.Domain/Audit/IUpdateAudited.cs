using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 表示更新审计
    /// </summary>
    public interface IUpdateAudited
    {
        /// <summary>
        /// 更新人姓名
        /// </summary>
        string Updater { get; set; }
        /// <summary>
        /// 更新人编号
        /// </summary>
        long? UpdaterID { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        DateTime? UpdateTime { get; set; }
    }
}
