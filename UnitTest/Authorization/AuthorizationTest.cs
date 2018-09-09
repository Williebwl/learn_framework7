using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using BIStudio.Framework.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BIStudio.Framework;

using BIStudio.Framework.Tag;
using BIStudio.Framework.Auth;
using BIStudio.Framework.Tenant;

namespace BIFramework.Test
{
    /// <summary>
    /// AuthorizationTest 的摘要说明
    /// </summary>
    [TestClass]
    public class AuthorizationTest
    {
        public AuthorizationTest()
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
        public void SystemTest()
        {
            var auth = CFAspect.Resolve<IAppService>();

            auth.SystemRegist(new SYSSystemRegistDTO { SystemCode = "test", SystemName = "网站运营管理" });

            var certificate = auth.CertificateIssue(new SYSSystemCertificateIssueDTO { ApiKey = "test1", SystemCode = "test", CertificateName = "系统管理" });
            Assert.AreEqual(certificate.ApiKey, "test1");
            auth.RecyleCertificate(certificate.ApiKey);
            auth.UnRegisterSystem("test");
        }

        [TestMethod]
        public void OAuth2Test()
        {
            var auth = CFAspect.Resolve<IAuthorizationService>();

            var passport = auth.PassportRegist(new SYSPassportRegistDTO("test1", "123456", "123456", "michael78@126.com"));

            var account = auth.AccountRegist(new SYSAccountRegistDTO { SystemCode = "BICMS", UID = "测试账号5" });
            var linkDto = new SYSPassportLinkDTO { LoginName = "test1", SystemCode = "BICMS", UID = "测试账号5" };
            //auth.PassportLink(linkDto);

            //授权码模式
            var authorize = auth.Authorize(new SYSAuthorizeDTO("BICMS_Master"));

            auth.AuthorizeLogin(new SYSAuthorizeLoginDTO(authorize.code, passport.LoginName, "123456"));

            var accessTokenByCode = auth.AccessToken(new SYSAccessTokenDTO("BICMS_Master", "44678314ba0efa0c", authorize.code));
            //客户端模式
            var accessTokenByCredentials = auth.AccessToken(new SYSAccessTokenDTO("BICMS_Master", "44678314ba0efa0c"));

            //更新令牌
            var accessTokenByRefreshToken = auth.AccessToken(new SYSAccessTokenDTO(accessTokenByCredentials.refresh_token));

            auth.DestroyToken(accessTokenByCredentials.access_token);
            //auth.PassportUnlink(linkDto);
            //auth.UnRegisterAccount(account.SystemID.Value,account.UID);
            //auth.PassportUnRegister(passport.LoginName);
        }


        [TestMethod]
        public void PassportTest()
        {
            var auth = CFAspect.Resolve<IAuthorizationService>();
            var passport = auth.PassportRegist(new SYSPassportRegistDTO("test1", "123456", "123456", "michael78@126.com"));

            var forgetDto = auth.PassportForgot(passport.Email);

            Assert.IsTrue(auth.VerifyCode(passport.LoginName,forgetDto.VerificationCode));

            auth.PassportRetrievePassword(new SYSPassportRetrievePasswordDTO(passport.LoginName, "123456", "123456"));

            auth.PassportChangePassword(new SYSPassportChangePasswordDTO(passport.LoginName, "123456", "1234567", "1234567"));

            auth.PassportValid(passport.LoginName, false);

            auth.PassportValid(passport.LoginName, true);

            auth.PassportLock(passport.LoginName, true);

            auth.PassportLock(passport.LoginName, false);

            auth.PassportUnRegister(passport.LoginName);
        }

    }
}
