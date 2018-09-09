using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain.EntityFramework
{
    public static class EFUnitOfWorkExtensions
    {
        /// <summary>
        /// 获得数据上下文
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
        public static DynamicContext GetDBContext(this IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork");

            if (!(unitOfWork is EFUnitOfWork))
                throw new ArgumentException("unitOfWork is not type of " + typeof(EFUnitOfWork).FullName, "unitOfWork");

            return (unitOfWork as EFUnitOfWork).dbContext;
        }
    }
}
