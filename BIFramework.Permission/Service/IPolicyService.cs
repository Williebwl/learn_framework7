using BIStudio.Framework.Domain;
using BIStudio.Framework.Permission;
using System;
using System.Collections.Generic;

namespace BIStudio.Framework.Permission
{
    public interface IPolicyService : IDomainService
    {
        #region 判断权限相关服务

        /// <summary>
        /// 获得用户可用操作
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="time"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        IList<SYSOperation> GetOperations(long accountId, DateTime? time, string ip);
        
        /// <summary>
        /// 获得操作过滤器
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="operationCode"></param>
        /// <returns></returns>
        ISpecification<TEntity> GetFilter<TEntity>(string operationCode) where TEntity : class;
        
        /// <summary>
        /// 对数据源应用过滤器
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <param name="operationCode"></param>
        /// <returns></returns>
        IEnumerable<TEntity> ApplyFilter<TEntity>(IEnumerable<TEntity> source, string operationCode) where TEntity : class;

        #endregion
    }
}
