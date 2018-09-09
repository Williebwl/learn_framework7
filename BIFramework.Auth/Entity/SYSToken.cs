
using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Auth
{
    /// <summary>
    /// 身份令牌，既是app也是用户的tocken，通过是否指定uid来区分
    /// </summary>
    [Table("SYSToken")]
    public class SYSToken : Entity
    {
        /// <summary>
        ///  通行证ID，冗余数据
        /// </summary>
        public long? PassportID { get; set; }
        /// <summary>
        ///  服务提供商ID
        /// </summary>
        public long? SystemID { get; set; }
        /// <summary>
        /// 账号，冗余数据
        /// </summary>
        public long? AccountID { get; set; }
        /// <summary>
        /// 外部系统用户编码
        /// </summary>
        public string UID { get; set; }
        /// <summary>
        /// 访问令牌
        /// </summary>
        [Column(IsExact=true)]
        public string AccessToken { get; set; }
        /// <summary>
        /// 更新令牌，由系统自动生成
        /// </summary>
        [Column(IsExact = true)]
        public string RefreshToken { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? ExpiresIn { get; set; }
        /// <summary>
        /// 授予权限
        /// </summary>
        public string Scope { get; set; }
        /// <summary>
        /// 请求时间
        /// </summary>
        public DateTime? RequestTime { get; set; }
        /// <summary>
        /// 请求IP
        /// </summary>
        public string RequestIP { get; set; }
    }

}
