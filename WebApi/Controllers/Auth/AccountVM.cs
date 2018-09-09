using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using BIStudio.Framework.UI;

namespace WebApi.Controllers.Auth
{
    /// <summary>
    /// 平台账号视图模型
    /// </summary>
    public class AccountVM : ViewModel
    {
        #region 视图模型属性

        /// <summary>
        /// ID
        /// </summary>
        [Required, Display(Name = "ID")]
        public long? ID { get; set; }

        /// <summary>
        /// 服务提供商ID
        /// </summary>
        [Required, Display(Name = "服务提供商ID")]
        public long? SystemID { get; set; }

        /// <summary>
        /// 服务提供商名称
        /// </summary>
        [StringLength(100), Display(Name = "服务提供商名称")]
        public string SystemName { get; set; }

        /// <summary>
        /// 外部系统用户编码
        /// </summary>
        [StringLength(500), RegularExpression(@"^[\t\r\n\u0020-\u007e]*$", ErrorMessage = "字段 {0} 必须是英文字母、数字或符号。"), Display(Name = "外部系统用户编码")]
        public string UID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [StringLength(1000), Display(Name = "用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [StringLength(1000), Display(Name = "真实姓名")]
        public string RealName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "性别")]
        public int? Gender { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        [StringLength(500), RegularExpression(@"^[\t\r\n\u0020-\u007e]*$", ErrorMessage = "字段 {0} 必须是英文字母、数字或符号。"), EmailAddress, Display(Name = "电子邮件")]
        public string Email { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        [StringLength(500), RegularExpression(@"^[\t\r\n\u0020-\u007e]*$", ErrorMessage = "字段 {0} 必须是英文字母、数字或符号。"), Phone, Display(Name = "电话号码")]
        public string Tel { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [StringLength(500), RegularExpression(@"^[\t\r\n\u0020-\u007e]*$", ErrorMessage = "字段 {0} 必须是英文字母、数字或符号。"), Phone, Display(Name = "手机号码")]
        public string Mobile { get; set; }

        /// <summary>
        /// 主页
        /// </summary>
        [StringLength(500), RegularExpression(@"^[\t\r\n\u0020-\u007e]*$", ErrorMessage = "字段 {0} 必须是英文字母、数字或符号。"), Display(Name = "主页")]
        public string Homepage { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        public string Remarks { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [StringLength(500), RegularExpression(@"^[\t\r\n\u0020-\u007e]*$", ErrorMessage = "字段 {0} 必须是英文字母、数字或符号。"), Display(Name = "头像")]
        public string Avatar { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Display(Name = "地址")]
        public string Address { get; set; }

        /// <summary>
        /// 地理位置纬度
        /// </summary>
        [Display(Name = "地理位置纬度")]
        public float? Latitude { get; set; }

        /// <summary>
        /// 地理位置经度
        /// </summary>
        [Display(Name = "地理位置经度")]
        public float? Longitude { get; set; }

        /// <summary>
        /// 地理位置精度
        /// </summary>
        [Display(Name = "地理位置精度")]
        public float? Precision { get; set; }

        /// <summary>
        /// 外部系统用户唯一编码
        /// </summary>
        [StringLength(500), RegularExpression(@"^[\t\r\n\u0020-\u007e]*$", ErrorMessage = "字段 {0} 必须是英文字母、数字或符号。"), Display(Name = "外部系统用户唯一编码")]
        public string OpenID { get; set; }

        /// <summary>
        /// 平台门户用户账号
        /// </summary>
        [Display(Name = "平台门户用户账号")]
        public long? PassportID { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        [Display(Name = "请求时间")]
        public DateTime? InputTime { get; set; }

        /// <summary>
        /// 请求IP
        /// </summary>
        [StringLength(50), RegularExpression(@"^[\t\r\n\u0020-\u007e]*$", ErrorMessage = "字段 {0} 必须是英文字母、数字或符号。"), Display(Name = "请求IP")]
        public string InputIP { get; set; }

        #endregion
        
        /// <summary>
        /// 登陆名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? IsValid { get; set; }
    }

}