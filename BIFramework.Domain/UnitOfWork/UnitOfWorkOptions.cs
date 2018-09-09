using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 工作单元初始配置
    /// </summary>
    public class UnitOfWorkOptions
    {
        public UnitOfWorkOptions()
        {
            ConnectionName = CFConfig.DefaultConnectionName;
            IsTransactional = true;
        }

        public static UnitOfWorkOptions Default = new UnitOfWorkOptions();

        /// <summary>
        /// 连接字符串名称
        /// </summary>
        public string ConnectionName { get; set; }

        /// <summary>
        /// 是否开启事务
        /// </summary>
        public bool IsTransactional { get; set; }
    }
}
