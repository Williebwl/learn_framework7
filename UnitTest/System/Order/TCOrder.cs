using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;


namespace BIFramework.Test.System.Order
{
    [Table("TCOrder")]
    public  class TCOrder : Entity
    {
        public long? CustomId { get; set; }

        public string Name { get; set; }

        public long? SaleId { get; set; }
    }
}
