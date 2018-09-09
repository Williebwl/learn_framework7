using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;


namespace BIFramework.Test.System.Order
{
    [Table("TCOrderFollow")]
    public class TCOrderFollow : Entity
    {
        public long? OrderId { get; set; }

        public long? SaleId { get; set; }

    }
}
