using BIStudio.Framework;

using BIStudio.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.BestPractice.Impl
{
    public class DALService : ISampleService
    {
        IDBQuery dbQuery = CFAspect.Resolve<IDBQuery>();
        public long CreateUser(string name)
        {
            var entity = new { ID = CFID.NewID(), Name = "张三" };
            dbQuery.Execute(DBBuilder.Insert("TCTest", entity));
            Console.WriteLine("用户已创建，用户编号：" + entity.ID);
            return entity.ID;
        }

        public UserEntity FindUser(string name)
        {
            var query = DBBuilder.Select("TCTest").Where().Like(new { Name = "%张三%" });
            return dbQuery.ToList<UserEntity>(query).FirstOrDefault();
        }
    }
}
