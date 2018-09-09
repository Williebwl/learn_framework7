using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BIFramework.Test.System.Order;
using BIStudio.Framework;

using BIStudio.Framework.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BIStudio.Framework.Permission;
using BIStudio.Framework.Permission;
using BIStudio.Framework.Auth;
using BIStudio.Framework.Institution;

namespace BIFramework.Test.System
{
    [TestClass]
    public class PrivilegeTest
    {
        private TestContext testContextInstance;

        private long systemId = 1;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        /// <summary>
        /// 资源的测试
        /// </summary>
        [TestMethod]
        public void ResourseTest()
        {
            var privilege = CFAspect.Resolve<IPermissionService>();
            var org = CFAspect.Resolve<IGroupService>();
            long userId = 1;
            var operation = privilege.OperationInject(new SYSOperationDTO
            {
                OperationCode = "Order_SalesManager_CreateOrder",
                OperationName = "客服创建订单",
            });
            Assert.AreEqual(operation.OperationCode, "Order_SalesManager_CreateOrder");

            var role = org.GroupInject(new SYSGroupInjectDTO(systemId, 742, "客服代表", "Order_SalesManager"));
            Assert.AreEqual(role.GroupCode, "Order_SalesManager");

            var roleAccount = org.GroupAccountAssign(new SYSGroupUserAssignDTO(systemId, role.GroupCode, userId));

            var filter = privilege.FilterInject(new SYSFilterInjectDTO(systemId, 742, "客服代表订单查询", "Order_SalesManager_MyOrder", "Order", "Name", "=", "1"));
            Assert.AreEqual(filter.FilterCode, "Order_SalesManager_MyOrder");

            org.Remove(roleAccount);
            org.Remove(role);
            privilege.Remove(operation);
            privilege.Remove(filter);
        }

        /// <summary>
        /// 策略的添加删除测试
        /// </summary>
        [TestMethod]
        public void StrategyTest()
        {
            var privilege = CFAspect.Resolve<IPermissionService>();
            var org = CFAspect.Resolve<IGroupService>();
            long userId = 1;

            var operation = privilege.OperationInject(new SYSOperationDTO
            {
                OperationCode = "Order_SalesManager_CreateOrder",
                OperationName = "客服创建订单",
            });
            Assert.AreEqual(operation.OperationCode, "Order_SalesManager_CreateOrder");

            var group = org.GroupInject(new SYSGroupInjectDTO(systemId, 742, "客服代表", "Order_SalesManager"));
            Assert.AreEqual(group.GroupCode, "Order_SalesManager");

            var groupAccount = org.GroupAccountAssign(new SYSGroupUserAssignDTO(systemId, group.GroupCode, userId));

            var filter = privilege.FilterInject(new SYSFilterInjectDTO(systemId, 742, "客服代表订单查询", "Order_SalesManager_MyOrder", "Order", "Name", "=", "1"));
            Assert.AreEqual(filter.FilterCode, "Order_SalesManager_MyOrder");

            var operationFilter = privilege.OperationFilterAssign(
                new SYSOperationFilterAssignDTO(systemId, "Order_SalesManager_CreateOrder", "Order_SalesManager_MyOrder"));

            var strategy = privilege.StrategyInject(new SYSStrategyDTO(systemId, 742, "订单策略", "Order_SalesManager_Order_Strategy"));
            Assert.AreEqual(strategy.StrategyCode, "Order_SalesManager_Order_Strategy");

            var strategyOperation = privilege.StrategyOperationAssign(new SYSStrategyOperationAssignDTO(systemId, "Order_SalesManager_Order_Strategy", "Order_SalesManager_CreateOrder"));

            var strategyGroup = privilege.StrategyGroupAssign(new SYSStrategyGroupAssignDTO(systemId, "Order_SalesManager", "Order_SalesManager_Order_Strategy"));

            org.Remove(groupAccount);
            privilege.Remove(strategyGroup);
            org.Remove(group);

            privilege.Remove(operationFilter);
            privilege.Remove(filter);

            privilege.Remove(strategyOperation);
            privilege.Remove(operation);

            privilege.Remove(strategy);


        }

        [TestMethod]
        public void OrderTestCase()
        {
            var privilege = CFAspect.Resolve<IPermissionService>();
            var auth = CFAspect.Resolve<IAuthorizationService>();
            var org = CFAspect.Resolve<IGroupService>();
            var operation = privilege.OperationInject(new SYSOperationDTO
            {
                OperationCode = "Order_SalesManager_CreateOrder",
                OperationName = "客服创建订单",
            });
            Assert.AreEqual(operation.OperationCode, "Order_SalesManager_MyOrder");

            var group = org.GroupInject(new SYSGroupInjectDTO(systemId, 742, "客服代表", "Order_SalesManager"));
            Assert.AreEqual(group.GroupCode, "Order_SalesManager");

            var custom1 = auth.AccountRegist(new SYSAccountRegistDTO { SystemCode = "BICMS", UID = "custom1" });
            Assert.AreEqual(custom1.UID, "custom1");

            SYSGroupUser groupAccount = null;
            var filters = new List<SYSFilter>();
            var operationFilters = new List<SYSOperationFilter>();

            long saleId = 0;

            var orderRepository = new TCOrderRepository();
            var sales = new List<SYSAccount>();
            var orders = new List<TCOrder>();
            for (int i = 0; i < 10; i++)
            {
                //新增客服
                var sale = auth.AccountRegist(new SYSAccountRegistDTO { SystemCode = "BICMS", UID = "sale" + i });
                Assert.AreEqual(sale.UID, "sale" + i);
                sales.Add(sale);

                //新增订单
                var order = new TCOrder { CustomId = custom1.ID, Name = "order" + i, SaleId = sale.ID.Value };
                orderRepository.Add(order);
                orders.Add(order);

                if (i != 1) continue;
                groupAccount = org.GroupAccountAssign(new SYSGroupUserAssignDTO(systemId, group.GroupCode, sale.ID.Value));
                saleId = sale.ID.Value;
                //设置当前连接id
                //ALCurrentUser.OperationIdentify = new ThreadLocal<string>(() => saleId.ToString()); ;

                var idFilter = privilege.FilterInject(new SYSFilterInjectDTO(systemId, 742, "客服代表订单查询", "Order_SalesManager_MyOrder", typeof(TCOrder).FullName, "SaleId", "=", saleId.ToString()));
                Assert.AreEqual(idFilter.FilterCode, "Order_SalesManager_MyOrder");

                filters.Add(idFilter);

                var operationFilter1 = privilege.OperationFilterAssign(new SYSOperationFilterAssignDTO(systemId, operation.OperationCode, idFilter.FilterCode));
                operationFilters.Add(operationFilter1);

                var nameFilter = privilege.FilterInject(new SYSFilterInjectDTO(systemId, 742, "客服代表订单按照名字查询", "Order_SalesManager_OrderNameLikeSale", typeof(TCOrder).FullName, "Name", "like", "order%"));
                Assert.AreEqual(nameFilter.FilterCode, "Order_SalesManager_OrderNameLikeSale");
                filters.Add(nameFilter);

                var operationFilter2 = privilege.OperationFilterAssign(new SYSOperationFilterAssignDTO(systemId, operation.OperationCode, nameFilter.FilterCode));
                operationFilters.Add(operationFilter2);
            }
            var strategy = privilege.StrategyInject(new SYSStrategyDTO(systemId, 742, "订单策略", "Order_SalesManager_Order_Strategy"));
            Assert.AreEqual(strategy.StrategyCode, "Order_SalesManager_Order_Strategy");

            var strategyOperation = privilege.StrategyOperationAssign(new SYSStrategyOperationAssignDTO(systemId, strategy.StrategyCode, operation.OperationCode));

            var strategyGroup = privilege.StrategyGroupAssign(new SYSStrategyGroupAssignDTO(systemId, group.GroupCode, strategy.StrategyCode));

            //CFContext.User.UserID = saleId;

            //核心
            var service = CFAspect.Resolve<IOrderService>();
            var infos = service.TestMethod();


            Assert.IsTrue(infos.Any());
            Assert.IsTrue(infos.All(item => item.SaleId == saleId));
            //end

            sales.ForEach(item => auth.UnRegisterAccount(item.SystemID.Value, item.UID));
            orders.ForEach(item => orderRepository.Remove(item));



            if (groupAccount != null) org.Remove(groupAccount);
            privilege.Remove(strategyGroup);
            org.Remove(group);
            operationFilters.ForEach(item => privilege.Remove(item));

            filters.ForEach(item => privilege.Remove(item));

            privilege.Remove(strategyOperation);
            privilege.Remove(operation);

            privilege.Remove(strategy);


            auth.UnRegisterAccount(custom1.SystemID.Value, custom1.UID);
        }

        [TestMethod]
        public void GetValidateOperationCodesTest()
        {
            var privilege = CFAspect.Resolve<IPermissionService>();

            var orderRepository = new TCOrderRepository();

            var orders = new List<TCOrder>();
            for (int i = 0; i < 10; i++)
            {
                var order = new TCOrder { CustomId = i, Name = "testorder", SaleId = i };
                orderRepository.Add(order);
                orders.Add(order);
            }

            var filters = new List<SYSFilter>();
            var operationFilters = new List<SYSOperationFilter>();

            var operations = new List<SYSOperation>();
            var filter =
                   privilege.FilterInject(new SYSFilterInjectDTO(systemId, 742, "客服代表订单查询", "Order_SalesManager_MyOrder",
                       typeof(TCOrder).FullName, "Name", "not like", "123"));
            filters.Add(filter);
            for (int i = 2; i < 5; i++)
            {
                var operation = privilege.OperationInject(new SYSOperationDTO
                {
                    OperationCode = "Order_SalesManager_MyOrder"+i,
                    OperationName = "客服创建订单",
                });
                operations.Add(operation);

                operationFilters.Add(privilege.OperationFilterAssign(new SYSOperationFilterAssignDTO(systemId, operation.OperationCode, filter.FilterCode)));
            }
            //var items = privilege.GetValidateOperationCodes(typeof(TCOrder).FullName, orders.FirstOrDefault(item => item.SaleId == 0));
            //Assert.IsNotNull(items);

            operations.ForEach(item => privilege.Remove(item));
            filters.ForEach(item => privilege.Remove(item));
            operationFilters.ForEach(item => privilege.Remove(item));
            orders.ForEach(item => orderRepository.Remove(item));
        }

        [TestMethod]
        public void IocOrderTest()
        {
            var service = CFAspect.Resolve<IOrderService>();

            //try
            //{
                service.TestMethod();
            //}
            //catch (NotHavePermissionEception)
            //{
            //    Assert.IsTrue(true);
            //}
            //catch (Exception)
            //{
            //    Assert.Fail();
            //}
        }
    }
}
