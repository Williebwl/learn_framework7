using System;
using BIStudio.Framework.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BIFramework.Test
{
    [TestClass]
    public class UtilsTest
    {
        [TestMethod]
        public void MailTest()
        {

            var mail = new ALMail();
            var result = mail.Send("123", "1231231", "skyven", "549515547@qq.com");

            Assert.IsTrue(result);
        }
    }
}
