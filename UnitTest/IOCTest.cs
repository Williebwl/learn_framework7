using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BIStudio.Framework;
using System.Linq;

namespace BIFramework.Test
{
    /// <summary>
    /// IOCTest 的摘要说明
    /// </summary>
    [TestClass]
    public class IOCTest
    {
        public IOCTest()
        {
            //
            //TODO:  在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

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

        [TestMethod]
        public void ObjectMapperTest()
        {
            //CFMapper.CreateMap<int[], List<int>>(d =>
            //{
            //    return d.ToList();
            //});
            var list = new[] { 1, 2, 3 }.Map<int[], List<int>>();
            Assert.AreNotEqual(list, null);
            Assert.AreEqual(list.Count, 3);
        }
        [TestMethod]
        public void ServiceLocatorTest()
        {
        }
        [TestMethod]
        public void MessageBusTest()
        {
        }
    }
}
