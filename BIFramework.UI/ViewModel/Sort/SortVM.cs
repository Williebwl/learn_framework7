using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.UI
{
    public class SortVM : ViewModel
    {
        /// <summary>
        /// 唯一标示
        /// </summary>
        public long? ID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sequence { get; set; }
    }
}
