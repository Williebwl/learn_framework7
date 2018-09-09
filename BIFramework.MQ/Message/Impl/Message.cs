using BIStudio.Framework;
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
    /// 消息
    /// </summary>
    public abstract class Message : IMessage
    {
        public Message()
        {
            this.ID = CFID.NewID();
            this.TypeName = this.GetType().FullName;
            this.Timestamp = DateTime.Now;
        }
        /// <summary>
        /// 消息标识
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        public long ID { get; set; }
        /// <summary>
        /// 消息类别
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        public string TypeName { get; set; }
        /// <summary>
        /// 消息时间
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        public DateTime Timestamp { get; set; }
    }
}
