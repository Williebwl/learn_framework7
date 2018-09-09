using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIStudio.Framework;

namespace BIStudio.Framework.BestPractice.Impl
{
    /// <summary>
    /// 表示应用模块服务层，由Web API调用
    /// </summary>
    [Log]
    public interface ISampleService
    {
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        long CreateUser(string name);
        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        UserEntity FindUser(string name);
    }
}
