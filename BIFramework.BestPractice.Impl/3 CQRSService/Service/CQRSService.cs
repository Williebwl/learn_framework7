using BIStudio.Framework;
using BIStudio.Framework.Domain;
using BIStudio.Framework.MQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.BestPractice.Impl
{
    /// <summary>
    /// 应用模块服务层，由Web API调用
    /// </summary>
    [Ioc(RegisterType = typeof(ISampleService), AopType = AopType.InterfaceInterceptor,LifetimeManagerType = LifetimeManagerType.Transient)]
    public class CQRSService : ISampleService
    {
        Lazy<UserRepository> testBO = new Lazy<UserRepository>(() => new UserRepository());
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public long CreateUser(string name)
        {
            var cmd = new CreateUserCommand(name);
            MessageService.Default.Dispatch(cmd);
            //异步操作，无法立即获取返回值
            //return cmd.UserID.Value;
            return 0;
        }
        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public UserEntity FindUser(string name)
        {
            return testBO.Value.Get(new UserNameSpecification(name));
        }
    }
}
