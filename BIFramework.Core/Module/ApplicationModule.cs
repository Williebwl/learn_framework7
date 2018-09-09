using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework
{
    /// <summary>
    /// 模块入口
    /// </summary>
    public abstract class ApplicationModule
    {
        /// <summary>
        /// 扫描并注册Ioc容器
        /// </summary>
        /// <param name="config"></param>
        internal static void RegisterIocContainer(CFConfig config)
        {
            var currentType = typeof(ApplicationModule);
            var containers = new List<ApplicationModule>();
            //查找程序集中CFAspect的子类
            config.ScanTypes(type =>
            {
                if (type.IsSubclassOf(currentType))
                {
                    dynamic ct = Activator.CreateInstance(type);
                    containers.Add(ct);
                }
            });
            //模块初始化
            foreach (var ct in containers)
            {
                try
                {
                    ct.Init();
                }
                catch
                {
                    continue;
                }
            }
            //模块启动
            foreach (var ct in containers)
            {
                try
                {
                    ct.Load();
                }
                catch
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// 在当前模块中注册接口的实现
        /// </summary>
        protected virtual void Init()
        {
        }
        /// <summary>
        /// 在当前模块中注册应用程序启动事件
        /// </summary>
        protected virtual void Load()
        {
        }
    }
}
