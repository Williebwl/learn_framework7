using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;


namespace BIStudio.Framework.Institution
{
    /// <summary>
    /// 角色账户
    /// </summary>
    [Table("SYSGroupUser")]
    public class SYSGroupUser : Entity
    {
        /// <summary>
        /// 关联Group的角色ID
        /// </summary>
        public long? GroupID { get; set; }
        /// <summary>
        /// 角色用户ID
        /// </summary>
        public long? UserId { get; set; }
    }

}
