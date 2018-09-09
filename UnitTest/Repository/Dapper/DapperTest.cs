using System;
using System.Linq;
using BIStudio.Framework;
using BIStudio.Framework.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Utils;
using System.Collections.Generic;
using BIStudio.Framework.Institution;
using BIStudio.Framework.Auth;
using System.Dynamic;
using System.ComponentModel;
using System.Data;


namespace BIFramework.Test
{
    /// <summary>
    ///     UnitTest1 的摘要说明
    /// </summary>
    [TestClass]
    public class EntityFrameworkTest
    {
        [TestMethod]
        public void DapperData()
        {
            IDBQuery dbQuery = CFAspect.Resolve<IDBQuery>();
            DBBuilder dbBuilder = DBBuilder.Define();
            //单表查询
            dbBuilder = DBBuilder.Select("User", new { LoginName = "system" });
            //多表查询
            dbBuilder = DBBuilder.Select("User")
                .InnerJoin("User", "ID", "RoleUser", "UserID")
                .InnerJoin("RoleUser", "RoleID", "Role", "ID")
                .Where("Role", new { ID = 1 })
                .And().Like("User", new { UserName = "%管理员%" });
            //条件查询
            var EntityModule = "模块";
            DateTime? BeginUserTime = DateTime.Now;
            DateTime? EndUserTime = DateTime.Now;
            int? UserID = 100;
            string EntityTitle = null;

            dbBuilder = DBBuilder.Define().Where(true)
                .And(d => d.Like(new { EntityModule = "%" + EntityModule + "%" }), !string.IsNullOrEmpty(EntityModule))
                .And(d => d.Ge(new { CreateTime = BeginUserTime }), BeginUserTime.HasValue)
                .And(d => d.Le(new { CreateTime = EndUserTime }), EndUserTime.HasValue)
                .And(d => d.Eq(new { UserID = UserID }), UserID.HasValue)
                .And(d => d.Like(new { EntityTitle = "%" + EntityTitle + "%" }), !string.IsNullOrEmpty(EntityTitle));

            //var infos = dbQuery.ToList<SYSUserInfo>(dbBuilder);
            //Assert.AreEqual(infos.Count, 1);
        }
        [TestMethod]
        public void DapperUnitofwork()
        {
            var dbQuery = CFAspect.Resolve<IDBQuery>();
            //使用Data.Core插入数据
            dbQuery.Execute(DBBuilder.Delete("TCTest"));
            dbQuery.Execute(DBBuilder.Insert("TCTest", new { ID = CFID.NewID(), Name = "张三" }));
            Assert.AreEqual(Convert.ToInt32(dbQuery.ToScalar(DBBuilder.Select().From("TCTest", "count(*)"))), 1);
            //使用Data.Core删除数据
            dbQuery.Execute(DBBuilder.Delete("TCTest").Where(new { Name = "张三" }));
            Assert.AreEqual(Convert.ToInt32(dbQuery.ToScalar<int>(DBBuilder.Select().From("TCTest", "count(*)"))), 0);

            using (var dbContext = BoundedContext.Create())
            {
                var testBo = dbContext.Resolve<TCTestDapperBO>();
                //使用Domain.Repository插入数据
                testBo.Add(new TCTest { Name = "张三" });
                Assert.AreEqual(testBo.GetAll(new EntitySpec<TCTest>(d => d.Name = "张三")).Count(), 1);
                //使用Domain.Repository查询数据
                var infos = testBo.GetAll(new EntitySpec<TCTest>(d => d.Name = "张三"));
                //var infos = testBo.GetAll(new TCTestInfo { Name = "张三" }.AsSpec());
                infos = testBo.GetAll(new Spec<TCTest>("Name=@Name", new { Name = "张三" }));
                var info = infos.FirstOrDefault() ?? new TCTest();
                Assert.AreEqual(info.Name, "张三");
                //使用Domain.Repository更新数据
                info.Name = "李四";
                testBo.Modify(info);
                var query = new EntitySpec<TCTest>(d => d.Name = "李四").And(
                    new EntitySpec<TCTest>(d => d.Property.Where("InputTime", "InputTime>'" + DateTime.Now.Date + "'")));
                Assert.AreEqual(testBo.GetAll(query).Count(), 1);

                dbContext.Rollback();
            }
            //测试事务
            Assert.AreEqual(new TCTestDapperBO().GetAll().Count(), 0);
        }
    }
}