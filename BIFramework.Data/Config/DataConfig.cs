using BIStudio.Framework;
using BIStudio.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Configuration
{
    /// <summary>
    /// 数据实体配置
    /// </summary>
    public static class DataConfig
    {
        /// <summary>
        /// 注册实体配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static CFConfig RegisterDataMapping(this CFConfig config)
        {
            config.ScanAttributes<TableAttribute>((type, attribute) =>
            {
                DataEntityUtils.Entity(type);
            });
            return config;
        }
    }
}
