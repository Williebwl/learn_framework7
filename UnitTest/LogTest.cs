using System.Collections.Generic;
using BIStudio.Framework.Log;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BIFramework.Test
{
    /// <summary>
    ///     日志测试
    ///     先运行write方法，在运行相应的read方法
    ///     相应的配置请查看Configs文件下的BIFramework.LocalLog 和BIFramework.DBLog 文件中的Log节点下的内容
    /// </summary>
    [TestClass]
    public class LogTest
    {
        [TestMethod]
        public void LocalLogTest()
        {
            LogService.Default.WriteInfo("测试操作内容1");
            List<STDLog> items = LogService.Default.Read(new LogCondtiton { EntityModule = "BIFramework.Local", EntityID = 0 });
            Assert.IsNotNull(items);
        }

        [TestMethod]
        public void DBLogTest()
        {
            LogService.Default.WriteInfo("测试操作内容2");
            List<STDLog> items = LogService.Default.Read(new LogCondtiton { EntityModule = "BIFramework.Remoting", EntityID = 0 });
            Assert.IsNotNull(items);
        }
    }
}