using BIStudio.Framework.MQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.BestPractice.Impl
{
    public class UserCreatedEventHandler : IEventHandler<UserCreatedEvent>
    {
        /// <summary>
        /// 更新缓存字段
        /// </summary>
        /// <param name="evt"></param>
        public void Handle(UserCreatedEvent evt)
        {
            Console.WriteLine("用户已创建，用户编号：" + evt.AggregateRootID);
        }
    }
}
