using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Configuration
{
    /// <summary>
    /// AOP容器配置
    /// </summary>
    public static class CFAspectConfig
    {
        /// <summary>
        /// 注册AOP容器
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static CFConfig RegisterContainer(this CFConfig config)
        {
            ApplicationModule.RegisterIocContainer(config);
            CFAspect.RegisterAopContainer(config);
            return config;
        }
    }
}
