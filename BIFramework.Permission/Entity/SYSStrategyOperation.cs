using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;


namespace BIStudio.Framework.Permission
{
    /// <summary>
    /// 策略关联操作
    /// </summary>
    [Table("SYSStrategyOperation")]
    public class SYSStrategyOperation : Entity
    {
        public long? StrategyID { get; set; }

        /// <summary>
        /// 操作id
        /// </summary>
        public long? OperationID { get; set; }

    }
}
