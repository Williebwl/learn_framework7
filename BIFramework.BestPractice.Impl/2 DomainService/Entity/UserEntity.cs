
using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.BestPractice.Impl
{
    /// <summary>
    /// 用户表，已实现输入审计，逻辑删除
    /// </summary>
    [Table("TCTest")]
    public class UserEntity : AggregateRoot, IInputAudited, ISoftDelete
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [MinLength(2)]
        [Display(Name = "姓名")]
        public string Name { get; set; }
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
        public bool? IsDelete { get; set; }
    }
}
