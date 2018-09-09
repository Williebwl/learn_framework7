using System.Collections.Generic;
using System.Data;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 工作单元，请使用BoundedContext.Create管理工作单元
    /// </summary>
    public interface IUnitOfWork : IDBQuery
    {
    }
}
