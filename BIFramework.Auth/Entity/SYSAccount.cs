
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
    /// 平台账号
    /// </summary>
    [Table("SYSAccount")]
    public class SYSAccount : Entity
    {
        /// <summary>
        ///  服务提供商ID
        /// </summary>
        public long? SystemID { get; set; }
        /// <summary>
        ///  服务提供商名称
        /// </summary>
        public string SystemName { get; set; }
        /// <summary>
        /// 外部系统用户编码
        /// </summary>
        [Column(IsExact = true)]
        public string UID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int? Gender { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 主页
        /// </summary>
        public string Homepage { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public float? Latitude { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public float? Longitude { get; set; }
        /// <summary>
        /// 地理位置精度
        /// </summary>
        public float? Precision { get; set; }
        /// <summary>
        /// 外部系统用户唯一编码
        /// </summary>
        public string OpenID { get; set; }
        /// <summary>
        /// 平台门户用户账号
        /// </summary>
        public long? PassportID { get; set; }
        /// <summary>
        /// 请求时间
        /// </summary>
        public DateTime? InputTime { get; set; }
        /// <summary>
        /// 请求IP
        /// </summary>
        public string InputIP { get; set; }
    }

}
