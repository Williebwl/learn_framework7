using BIStudio.Framework;
using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BIStudio.Framework.Domain.EntityFramework
{
    /// <summary>
    /// EF仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public sealed class EFRepository<TEntity> : RepositoryBase<TEntity>, IRepository<TEntity>
        where TEntity : class ,IEntity, new()
    {
        public override IQueryable<TEntity> Entities
        {
            get
            {
                return this.UnitOfWork.GetDBContext().Set<TEntity>().AsNoTracking();
            }
        }
    }
}
