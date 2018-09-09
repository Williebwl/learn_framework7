using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BIStudio.Framework.MQ
{
    /// <summary>
    /// 表示消息
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// 消息标识
        /// </summary>
        long ID { get; set; }
        /// <summary>
        /// 消息类别
        /// </summary>
        string TypeName { get; set; }
        /// <summary>
        /// 消息时间
        /// </summary>
        DateTime Timestamp { get; set; }
    }
}
