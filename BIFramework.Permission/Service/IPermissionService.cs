using BIStudio.Framework.Domain;
using BIStudio.Framework.Utils;

namespace BIStudio.Framework.Permission
{
    public interface IPermissionService : IDomainService
    {
        #region 资源点

        /// <summary>
        /// 根据id获取操作
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SYSOperationDTO GetOperation(long id);

        /// <summary>
        /// 将操作附加到资源点
        /// </summary>
        /// <returns></returns>
        SYSOperation OperationInject(SYSOperationDTO operation);
        /// <summary>
        /// 删除资源点操作
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        bool Remove(SYSOperation operation);

        /// <summary>
        /// 将过滤器附加到资源点
        /// </summary>
        /// <returns></returns>
        SYSFilter FilterInject(SYSFilterInjectDTO filter);
        /// <summary>
        /// 删除资源点过滤器
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        bool Remove(SYSFilter filter);

        #endregion

        #region 策略

        /// <summary>
        /// 添加策略
        /// </summary>
        /// <param name="strategyGroup"></param>
        /// <returns></returns>
        SYSStrategy StrategyInject(SYSStrategyDTO strategyGroup);

        /// <summary>
        /// 删除策略
        /// </summary>
        /// <param name="strategyGroup"></param>
        bool Remove(SYSStrategy strategyGroup);

        /// <summary>
        /// 关联策略和用户组
        /// </summary>
        /// <param name="strategyGroup"></param>
        /// <returns></returns>
        SYSStrategyGroup StrategyGroupAssign(SYSStrategyGroupAssignDTO strategyGroup);

        /// <summary>
        /// 撤销策略和用户组的关联
        /// </summary>
        /// <param name="strategyGroup"></param>
        bool Remove(SYSStrategyGroup strategyGroup);

        /// <summary>
        /// 关联策略和操作
        /// </summary>
        /// <param name="strategyOperation"></param>
        /// <returns></returns>
        SYSStrategyOperation StrategyOperationAssign(SYSStrategyOperationAssignDTO strategyOperation);

        /// <summary>
        /// 撤销策略和操作的关联
        /// </summary>
        /// <param name="strategyOperation"></param>
        bool Remove(SYSStrategyOperation strategyOperation);
        
        /// <summary>
        /// 关联策略和过滤器
        /// </summary>
        /// <param name="strategyFilter"></param>
        /// <returns></returns>
        SYSOperationFilter OperationFilterAssign(SYSOperationFilterAssignDTO strategyFilter);

        /// <summary>
        /// 撤销策略和过滤器的关联
        /// </summary>
        /// <param name="strategyFilter"></param>
        bool Remove(SYSOperationFilter strategyFilter);
        #endregion
    }
}
