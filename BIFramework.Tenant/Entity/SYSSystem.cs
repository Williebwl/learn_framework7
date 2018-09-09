
using BIStudio.Framework.Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIStudio.Framework.Utils;
using BIStudio.Framework.Data;

namespace BIStudio.Framework.Tenant
{
    /// <summary>
    /// 平台应用
    /// </summary>
    [Table("SYSSystem")]
    public class SYSSystem : Entity
    {
        public SYSSystem()
        {
        }
        public SYSSystem(string code)
        {
            SystemCode = code;
            Status = (int)EnumSYSSystemStatus.Running;
            InputTime = DateTime.Now;
            Inputer = CFContext.User.UserName;
            InputerID = CFContext.User.ID;
        }

        /// <summary>
        ///  服务提供商ID
        /// </summary>
        [Column(IsExact = true)]
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
        /// 程序运行状态(1=未安装，2=停用，3=启用）
        /// </summary>
        public int? Status { get; set; }
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

}
