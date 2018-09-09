
using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Tenant
{
    /// <summary>
    /// 客户端访问证书，用于客户端访问平台的证书
    /// </summary>
    [Table("SYSSystemCertificate")]
    public class SYSSystemCertificate : Entity
    {
        /// <summary>
        ///  服务提供商ID
        /// </summary>
        public long? SystemID { get; set; }
        /// <summary>
        /// 证书名称
        /// </summary>
        public string CertificateName { get; set; }
        /// <summary>
        /// 客户端识别码，由客户端提供
        /// </summary>
        [Column(IsExact = true)]
        public string ApiKey { get; set; }
        /// <summary>
        /// 客户端密钥，由系统自生成
        /// </summary>
        [Column(IsExact = true)]
        public string Secret { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool? IsValid { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Inputer { get; set; }
        /// <summary>
        /// 创建人的ID
        /// </summary>
        public long? InputerID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? InputTime { get; set; }
    }

}
