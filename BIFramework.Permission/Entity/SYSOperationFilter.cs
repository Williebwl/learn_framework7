using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;


namespace BIStudio.Framework.Permission
{
    [Table("SYSOperationFilter")]
    public class SYSOperationFilter : Entity
    {
        public long? OperationID { get; set; }

        /// <summary>
        /// 过滤器id
        /// </summary>
        public long? FilterID { get; set; }

    }
}
