using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.BestPractice.Impl
{
    /// <summary>
    /// 按用户名称查询订单
    /// </summary>
    public class UserNameSpecification : SpecificationBase<UserEntity>
    {
        public UserNameSpecification(string userName)
        {
            this.Sql = DBBuilder.Define().Like(new { Name = "%" + userName + "%" });
        }
    }
}
