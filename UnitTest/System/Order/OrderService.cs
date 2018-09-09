using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIStudio.Framework;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using BIStudio.Framework.Permission;
using BIStudio.Framework.Domain;

namespace BIFramework.Test.System.Order
{
    //[Privilege]
    public interface IOrderService
    {
        List<TCOrder> TestMethod();
    }

    [Ioc(RegisterType = typeof(IOrderService), AopType = AopType.InterfaceInterceptor)]
    public class OrderService : IOrderService
    {
        public virtual List<TCOrder> TestMethod()
        {
            //核心
            var repository = new TCOrderRepository();
            //使用Domain.Repository查询数据
            return repository.GetAll(new TrueSpec<TCOrder>()).ToList();
        }
    }
}
