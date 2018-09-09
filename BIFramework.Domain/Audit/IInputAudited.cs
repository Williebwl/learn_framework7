using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 表示录入审计
    /// </summary>
    public interface IInputAudited
    {
        /// <summary>
        /// 录入人姓名
        /// </summary>
        string Inputer { get; set; }
        /// <summary>
        /// 录入人编号
        /// </summary>
        long? InputerID { get; set; }
        /// <summary>
        /// 录入时间
        /// </summary>
        DateTime? InputTime { get; set; }
    }
}
