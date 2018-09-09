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
    public class DapperTest
    {
        [TestMethod]
        public void EntityFrameworkData()
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
        public void EntityFrameworkRepository()
        {
            //var bo = new Repository<SYSUserInfo>();
            //bo.GetAll();
        }
        public class MyOrder : FieldSpec<TCTest, string>
        {
            public MyOrder(string inputUser) : base("Inputer", inputUser) { }
        }
        [TestMethod]
        public void AuditTest()
        {
            //CFAspect.Resolve<Repository<TCTest>>().Remove(466001131959028);

            var result = CFAspect.Resolve<Repository<TCTest>>().GetAll(new TrueSpec<TCTest>(), new SortExpression<TCTest>("Name"));
        }
        [TestMethod]
        public void EntityFrameworkUnitofwork()
        {
            var dbQuery = CFAspect.Resolve<IDBQuery>();
            //使用Data.Core插入数据
            dbQuery.Execute(DBBuilder.Delete("TCTest"));
            dbQuery.Execute(DBBuilder.Insert("TCTest", new { ID = CFID.NewID(), Name = "张三" }));
            Assert.AreEqual(Convert.ToInt32(dbQuery.ToScalar(DBBuilder.Select().From("TCTest", "count(*)"))), 1);
            //使用Data.Core删除数据
            dbQuery.Execute(DBBuilder.Delete("TCTest").Where(new { Name = "张三" }));
            Assert.AreEqual(Convert.ToInt32(dbQuery.ToScalar<int>(DBBuilder.Select().From("TCTest", "count(*)"))), 0);

            using (var uow = BoundedContext.Create())
            {
                var rp = uow.Repository<TCTest>();
                //Linq
                var queryByLinq = from test in rp.Entities
                                  where test.Inputer == CFContext.User.UserName
                            select test;
                //表达式
                var queryByLambda = rp.GetAll(new Spec<TCTest>(entity => entity.Inputer == CFContext.User.UserName));
                //规约
                var queryBySpec1 = rp.GetPaged(new MyOrder(CFContext.User.UserName));

                rp.Entities.Where(d => d.ID > 9);

                var spec21 = new Spec<TCTest>(entity => entity.ID == 3);
                var spec22 = new Spec<TCTest>(entity => entity.ID == 4);
                var queryBySpec2 = rp.Entities.Where(spec21 || spec22);

                var spec31 = new Spec<TCTest>("ID=3");
                var spec32 = new Spec<TCTest>("ID=4");
                var queryBySpec3 = rp.GetAll(spec31 || spec32);

                //使用Domain.Repository插入数据
                rp.Add(new TCTest { Name = "张三" });
                Assert.AreEqual(rp.GetAll(new EntitySpec<TCTest>(d => d.Name = "张三")).Count(), 1);
                //使用Domain.Repository查询数据
                var infos = rp.GetAll(new EntitySpec<TCTest>(d => d.Name = "张三"));
                //var infos = testBo.GetAll(new TCTestInfo { Name = "张三" }.AsSpec());
                infos = rp.GetAll(new Spec<TCTest>("Name=@Name", new { Name = "张三" }));
                var info = infos.FirstOrDefault() ?? new TCTest();
                Assert.AreEqual(info.Name, "张三");
                //使用Domain.Repository更新数据
                info.Name = "李四";
                rp.Modify(info);
                var query = new EntitySpec<TCTest>(d => d.Name = "李四").And(
                    new EntitySpec<TCTest>(d => d.Property.Where("InputTime", "InputTime>'" + DateTime.Now.Date + "'")));
                Assert.AreEqual(rp.GetAll(query).Count(), 1);

                uow.Rollback();
            }
            //测试事务
            Assert.AreEqual(new TCTestEntityFrameworkBO().GetAll().Count(), 0);
        }
    }
}