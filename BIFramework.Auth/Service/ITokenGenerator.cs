using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIStudio.Framework.Utils;
using BIStudio.Framework.Domain;

namespace BIStudio.Framework.Auth
{
    /// <summary>
    /// 定义令牌生成器
    /// </summary>
    interface ITokenGenerator : IDomainService
    {
        /// <summary>
        /// 生成令牌
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        SYSAccessToken GeneralTocken(SYSAccessTokenDTO dto);
    }
}
