using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Standard.Contact
{
    /// <summary>
    /// 联系信息
    /// </summary>
    [Table("STDContactDetail")]
    public class STDContactDetail : Entity
    {
        /// <summary>
        /// 联系人编号
        /// </summary>
        public long? ContactID { get; set; }

        /// <summary>
        /// 沟通方式
        /// </summary>
        public string ContactMethod { get; set; }

        /// <summary>
        /// 沟通方式
        /// </summary>
        public long? ContactMethodID { get; set; }

        /// <summary>
        /// 通讯方式名称（TagName）
        /// </summary>
        public string ContactItemName { get; set; }

        /// <summary>
        /// 通讯方式ID（TagID）
        /// </summary>
        public long? ContactItemID { get; set; }

        /// <summary>
        /// 通讯方式内容
        /// </summary>
        public string ContactNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Inputer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? InputerID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? InputTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? UnitID { get; set; }
    }

}
