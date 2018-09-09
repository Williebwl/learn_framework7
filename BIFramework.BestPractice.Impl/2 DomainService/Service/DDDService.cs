using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.BestPractice.Impl
{
    public class DDDService : ISampleService
    {
        UserRepository userBO = new UserRepository();
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public long CreateUser(string name)
        {
            var entity = new UserEntity { Name = name };
            userBO.Add(entity);
            Console.WriteLine("用户已创建，用户编号：" + entity.ID);
            return entity.ID.Value;
        }
        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public UserEntity FindUser(string name)
        {
            return userBO.Get(new EntitySpec<UserEntity>(item => item.Name = name));
        }
    }
}
