using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 表示删除审计
    /// </summary>
    public interface IDeleteAudited : ISoftDelete
    {
        /// <summary>
        /// 删除人姓名
        /// </summary>
        string Deleter { get; set; }
        /// <summary>
        /// 删除人编号
        /// </summary>
        long? DeleterID { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        DateTime? DeleteTime { get; set; }
    }
}
