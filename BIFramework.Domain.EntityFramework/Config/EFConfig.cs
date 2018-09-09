using BIStudio.Framework;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Domain.EntityFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Configuration
{
    /// <summary>
    /// EntityFramework数据实体配置
    /// </summary>
    public static class EFConfig
    {
        /// <summary>
        /// 注册EntityFramework实体配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static CFConfig RegisterEFRepository(this CFConfig config)
        {
            foreach (ConnectionStringSettings cnStr in ConfigurationManager.ConnectionStrings)
            {
                DynamicContext.GetCompiledModel(cnStr.Name);
            }
            return config;
        }
    }
}
