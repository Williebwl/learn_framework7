using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain.EntityFramework
{
    [Ioc(typeof(IRepositoryProvider))]
    public class EFRepositoryProvider : IRepositoryProvider
    {
        public IRepository<TEntity> Instance<TEntity>() where TEntity : class ,IEntity, new()
        {
            return new EFRepository<TEntity>();
        }
    }
}
