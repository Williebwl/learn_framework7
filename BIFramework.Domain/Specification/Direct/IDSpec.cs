using BIStudio.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 按主键值查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IDSpec<T> : FieldSpec<T, long>
        where T : class, IEntity
    {
        public IDSpec(string ids)
            : base("ID", ALConvert.ToList<long>(ids))
        {
        }
        public IDSpec(IEnumerable<long> ids)
            : base("ID", ids)
        {
        }
        public IDSpec(params long[] ids)
            : base("ID", ids)
        {
        }
    }
}
