
using BIStudio.Framework.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Tenant
{
    /// <summary>
    /// 为指定系统创建新证书
    /// </summary>
    public struct SYSSystemCertificateIssueDTO
    {
        /// <summary>
        /// 系统代码
        /// </summary>
        public string SystemCode { get; set; }
        /// <summary>
        /// 证书别名
        /// </summary>
        public string CertificateName { get; set; }
        /// <summary>
        /// 证书代码
        /// </summary>
        public string ApiKey { get; set; }
    }

    /// <summary>
    /// 证书颁发结果
    /// </summary>
    public enum STDCertificateIssueResult
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
        /// 未指定证书名称或代码
        /// </summary>
        [Description("未指定证书名称或代码")]
        NameOrCodeNotFound,
        /// <summary>
        /// 证书代码已存在
        /// </summary>
        [Description("证书代码已存在")]
        CodeAlreadyExists,
    }
}
