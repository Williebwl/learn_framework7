using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;


namespace BIStudio.Framework.Permission
{
    /// <summary>
    /// 策略管理用户组
    /// </summary>
    [Table("SYSStrategyGroup")]
    public class SYSStrategyGroup : Entity
    {
        public long? StrategyID { get; set; }

        /// <summary>
        /// 用户组id
        /// </summary>
        public long? GroupID { get; set; }

    }
}
