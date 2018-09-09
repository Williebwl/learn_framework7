using BIStudio.Framework.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Auth
{
    /// <summary>
    /// 注册或更新会员账号
    /// </summary>
    public struct SYSAccountRegistDTO
    {
        /// <summary>
        /// 系统代码
        /// </summary>
        public string SystemCode { get; set; }
        /// <summary>
        /// 系统内部用户标识
        /// </summary>
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
    }

    public enum SYSAccountRegistResult
    {
        /// <summary>
        /// 操作成功
        /// </summary>
        [Description("操作成功")]
        Success = 0,
        /// <summary>
        /// 操作失败
        /// </summary>
        [Description("{0}")]
        Fail,
        /// <summary>
        /// 系统代码无效
        /// </summary>
        [Description("系统代码无效")]
        SystemCodeInvalid,
        /// <summary>
        /// 用户标识无效
        /// </summary>
        [Description("用户标识无效")]
        UIDInvalid,
    }
}
