using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIStudio.Framework.Tag
{
    /// <summary>
    /// 被授权的目标
    /// </summary>
    public struct SYSTagObjectDTO
    {
        /// <summary>
        /// 被授权的目标(TagGroup,TagClass,Tag)
        /// </summary>
        public string ObjectType { get; set; }
        /// <summary>
        /// 被授权的目标编号
        /// </summary>
        public long? ObjectValue { get; set; }

    }
}
