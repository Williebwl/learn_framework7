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
    /// 系统注册
    /// </summary>
    public struct SYSSystemRegistDTO
    {
        /// <summary>
        ///  服务提供商ID
        /// </summary>
        public string SystemCode { get; set; }
        /// <summary>
        ///  服务提供商名称
        /// </summary>
        public string SystemName { get; set; }
        /// <summary>
        /// 程序版本
        /// </summary>
        public string SystemVersion { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string ProviderName { get; set; }
        /// <summary>
        /// 供应商主页
        /// </summary>
        public string ProviderUrl { get; set; }
        /// <summary>
        /// 图标路径
        /// </summary>
        public string IconPath { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? Sequence { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 尝试获得版本信息
        /// </summary>
        /// <returns></returns>
        public Version GetVersion()
        {
            if (string.IsNullOrEmpty(this.SystemVersion))
                return new Version(1, 0);

            Version ver;
            Version.TryParse(this.SystemVersion, out ver);
            return ver ?? new Version(1, 0);
        }
    }
    /// <summary>
    /// 系统注册结果
    /// </summary>
    [Description("系统注册结果")]
    public enum SYSSystemRegistResult
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
        /// 未指定系统名称或代码
        /// </summary>
        [Description("未指定系统名称或代码")]
        NameOrCodeNotFound,
        /// <summary>
        /// 系统代码已存在
        /// </summary>
        [Description("系统代码已存在")]
        CodeAlreadyExists,
    }
}
