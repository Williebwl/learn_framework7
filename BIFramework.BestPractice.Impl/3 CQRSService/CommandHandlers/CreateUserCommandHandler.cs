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
    /// 准备创建用户
    /// </summary>
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
    {
        public void Handle(CreateUserCommand evt)
        {
            using (var unitOfWork = BoundedContext.Create())
            {
                //创建订单
                var orderBO = unitOfWork.Resolve<UserRepository>();
                var entity = new UserEntity { Name = evt.UserName };
                orderBO.Add(entity);
                evt.UserID = entity.ID;
                unitOfWork.Commit();
                //向第三方系统发送通知，更新缓存的用户名
                entity.ApplyEvent(new UserCreatedEvent());
            }
        }

    }
}
