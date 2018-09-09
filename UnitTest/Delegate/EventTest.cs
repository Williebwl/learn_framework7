using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BIStudio.Framework.Test.UnitTest.Delegate
{
    [TestClass]
    public class EventTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            GreetToUsers gtu = new GreetToUsers();
            DelegateGreet dg = new DelegateGreet();
            //3.事件注册方法
            dg.eventGreet += gtu.ChinaPeople;
            dg.eventGreet += gtu.EnglishPeople;
            //4.调用事件
            dg.GreetUser("小王");
        }
    }
    /// <summary>
    /// 事件发布
    /// </summary>
    public class DelegateGreet
    {
        //1.声明委托
        public delegate string DelegateGetGreeting(string userName);
        //2.声明事件：虽然Event是public，但还是私有变量，只能通过+=,-=访问
        public event DelegateGetGreeting eventGreet;
        public void GreetUser(string userName)
        {
            eventGreet?.Invoke(userName);
        }

    }
    //定义基本问候类和方法
    public class GreetToUsers
    {
        /// <summary>
        /// 中文问候
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string ChinaPeople(string userName)
        {
            return "您好， " + userName;
        }
        /// <summary>
        /// 英文问候
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string EnglishPeople(string userName)
        {
            return "Hello," + userName;
        }
        /// <summary>
        /// 非英非汉问候
        /// </summary>
        /// <param name="UserName"></param>
        public void OtherPeople(string UserName)
        {
            Console.WriteLine("Sorrry,当前系统只支持汉语与英语");
        }
    }
}
