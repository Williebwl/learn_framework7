using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Tenant
{
    using BIStudio.Framework.Domain;

    /// <summary>
    /// app
    /// </summary>
    public interface IAppService : IDomainService
    {
        /// <summary>
        /// 获取系统信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        SYSSystem GetSystemInfo(string code);

        /// <summary>
        /// 获取系统信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        SYSSystem GetSystemInfo(long systemId);

        /// <summary>
        /// 系统注册
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        void SystemRegist(SYSSystemRegistDTO dto);
        /// <summary>
        /// 注销系统
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        void UnRegisterSystem(string code);
        /// <summary>
        /// 为指定系统颁发新证书
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        SYSSystemCertificate CertificateIssue(SYSSystemCertificateIssueDTO dto);
        /// <summary>
        /// 回收证书
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        void RecyleCertificate(string apiKey);
        
        /// <summary>
        /// 保存应用信息
        /// </summary>
        /// <param name="dto">应用信息</param>
        /// <returns>是否成功</returns>
        bool SaveApp(SYSAppRegistDTO dto);
    }
}
