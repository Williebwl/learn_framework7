using BIStudio.Framework;
using BIStudio.Framework.MQ;
using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BIStudio.Framework.BestPractice.Impl;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using BIStudio.Framework.Configuration;

namespace BIStudio.Framework.BestPractice
{
    class Program
    {
        static void Main(string[] args)
        {
            CFConfig.Default
              .RegisterContainer()
              .RegisterDataMapping()
              .RegisterEFRepository();
            
            Console.WriteLine("1. 运行Data示例");
            Console.WriteLine("2. 运行Domain示例");
            Console.WriteLine("3. 运行CQRS示例");
            Console.WriteLine("4. 运行Actor示例");
            Console.WriteLine("请输入序号：");

            ISampleService service = null;
            char key = Console.ReadKey(true).KeyChar;
            if (key == '1')
            {
                service = new DALService();
            }
            else if (key == '2')
            {
                service = new DDDService();
            }
            else if (key == '3')
            {
                CFConfig.Default
                    .RegisterMessageDispatcher();
                service = CFAspect.Resolve<ISampleService>();
            }
            else if (key == '4')
            {
                CFConfig.Default
                    .RegisterMessageBroker("127.0.0.1")
                    .RegisterMessageDispatcher("127.0.0.1");
                service = CFAspect.Resolve<ISampleService>();
            }
            if (service != null)
            {
                try
                {
                    Console.WriteLine(service.GetType().Name + " Running...");
                    long orderID1 = service.CreateUser("张三");
                    var order1 = service.FindUser("张三");

                    long orderID2 = service.CreateUser("李四");
                    var order2 = service.FindUser("李四");

                    long orderID3 = service.CreateUser("王五");
                    var order3 = service.FindUser("王五");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("异常："+ex.Message);
                }
            }
            Console.ReadKey();
        }
    }
}
