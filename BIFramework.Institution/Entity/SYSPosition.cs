using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Institution
{
    /// <summary>
    /// 职位
    /// </summary>
    [Table("SYSPosition")]
    public class SYSPosition : Entity
    {
        /// <summary>
        ///  服务提供商ID
        /// </summary>
        public long? SystemID { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string PositionName { get; set; }

        /// <summary>
        /// 账号ID
        /// </summary>
        public long? UserID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public long? DeptID { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 上级领导ID
        /// </summary>
        public long? LeaderID { get; set; }

        /// <summary>
        /// 上级领导名称
        /// </summary>
        public string LeaderName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sequence { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool? IsValid { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool? IsDelete { get; set; }
    }

}
