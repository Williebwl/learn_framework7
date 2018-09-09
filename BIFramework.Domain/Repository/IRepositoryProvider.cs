using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    public interface IRepositoryProvider
    {
        IRepository<TEntity> Instance<TEntity>() where TEntity : class ,IEntity, new();
    }
}
