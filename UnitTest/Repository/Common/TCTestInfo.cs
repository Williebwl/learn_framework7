using System;

using BIStudio.Framework.Domain;
using BIStudio.Framework.Data;


namespace BIFramework.Test
{
    [Table("TCTest")]
    public class TCTest : Entity, IInputAudited, ISoftDelete
    {
        public string Name { get; set; }
        public string Inputer { get; set; }
        public long? InputerID { get; set; }
        public DateTime? InputTime { get; set; }
        public bool? IsDelete { get; set; }
    }
}