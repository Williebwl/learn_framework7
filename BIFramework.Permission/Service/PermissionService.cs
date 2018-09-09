using System;
using System.Collections.Generic;
using System.Linq;
using BIStudio.Framework;
using BIStudio.Framework.Tag;
using BIStudio.Framework.Utils;
using BIStudio.Framework.Domain;


namespace BIStudio.Framework.Permission
{
    [Ioc(typeof(IPermissionService))]
    public class PermissionService : DomainService, IPermissionService
    {

        #region 初始化
        private SYSOperationRepository _operationRepository;
        private SYSFilterRepository _filterRepository;
        private ITag _tagService;
        private SYSStrategyRepository _strategyRepository;
        private SYSStrategyGroupRepository _strategyGroupRepository;
        private SYSOperationFilterRepository _operationFilterRepository;
        private SYSStrategyOperationRepository _strategyOperationRepository;

        private long CurrentSystemId;

        #endregion

        #region 资源点

        /// <summary>
        /// 根据id获取操作
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SYSOperationDTO GetOperation(long id)
        {
            var operation= _operationRepository.Get(id);
            var taginfo = _tagService.GetTagApplyInfos(operation).FirstOrDefault()??new SYSTag();
            return new SYSOperationDTO
            {
                Id = id,
                OperationCode = operation.OperationCode,
                Remarks = operation.Remarks,
                OperationName = taginfo.TagName
            };
        }

        /// <summary>
        /// 将操作附加到资源点
        /// </summary>
        /// <returns></returns>
        public SYSOperation OperationInject(SYSOperationDTO dto)
        {
            if (string.IsNullOrEmpty(dto.OperationCode) || string.IsNullOrEmpty(dto.OperationName))
                throw CFException.Create(SYSOperationInjectResult.NameOrCodeNotFound);

            try
            {
                //tag class 待确定
              

                var prevCertificate = _operationRepository.Get(new EntitySpec<SYSOperation>(query => { query.SystemID = CurrentSystemId; query.OperationCode = dto.OperationCode; }));
                if (prevCertificate.ID.HasValue)
                    throw CFException.Create(SYSOperationInjectResult.CodeAlreadyExists);

                SYSOperation entity = dto.Map<SYSOperationDTO, SYSOperation>();
                entity.SystemID = CurrentSystemId;
                entity.InputTime = DateTime.Now;
                entity.Inputer = CFContext.User.UserName;
                entity.InputerID = CFContext.User.ID;
                _operationRepository.Add(entity);
                //粘贴tag
                return entity;
            }
            catch (Exception ex)
            {
                throw CFException.Create(SYSOperationInjectResult.Fail, ex);
            }
        }
        /// <summary>
        /// 删除资源点
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public bool Remove(SYSOperation operation)
        {
            return _operationRepository.Remove(operation);
        }

        /// <summary>
        /// 将过滤器附加到资源点
        /// </summary>
        /// <returns></returns>
        public SYSFilter FilterInject(SYSFilterInjectDTO filter)
        {
            return _filterRepository.Add(filter);
        }
        /// <summary>
        /// 删除过滤器
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public bool Remove(SYSFilter filter)
        {
            return _filterRepository.Remove(filter);
        }

        /// <summary>
        /// 关联策略和过滤器
        /// </summary>
        /// <param name="operationFilter"></param>
        /// <returns></returns>
        public SYSOperationFilter OperationFilterAssign(SYSOperationFilterAssignDTO operationFilter)
        {
            return _operationFilterRepository.OperationAssign(operationFilter);
        }

        /// <summary>
        /// 撤销策略和过滤器的关联
        /// </summary>
        /// <param name="operationFilter"></param>
        public bool Remove(SYSOperationFilter operationFilter)
        {
            return _operationFilterRepository.Remove(operationFilter);
        }
        #endregion

        #region 策略
        /// <summary>
        /// 添加策略
        /// </summary>
        /// <param name="strategy"></param>
        /// <returns></returns>
        public SYSStrategy StrategyInject(SYSStrategyDTO strategy)
        {
            return _strategyRepository.StrategyInject(strategy);
        }

        /// <summary>
        /// 删除策略
        /// </summary>
        /// <param name="strategyGroup"></param>
        /// <returns></returns>
        public bool Remove(SYSStrategy strategyGroup)
        {
            return _strategyRepository.Remove(strategyGroup);
        }

        /// <summary>
        /// 关联策略和用户组
        /// </summary>
        /// <param name="strategyGroup"></param>
        /// <returns></returns>
        public SYSStrategyGroup StrategyGroupAssign(SYSStrategyGroupAssignDTO strategyGroup)
        {
            return _strategyGroupRepository.GroupAssign(strategyGroup);
        }

        /// <summary>
        /// 撤销策略和用户组的关联
        /// </summary>
        /// <param name="strategyGroup"></param>
        public bool Remove(SYSStrategyGroup strategyGroup)
        {
            return _strategyGroupRepository.Remove(strategyGroup);
        }

        /// <summary>
        /// 关联策略和操作
        /// </summary>
        /// <param name="strategyOperation"></param>
        /// <returns></returns>
        public SYSStrategyOperation StrategyOperationAssign(SYSStrategyOperationAssignDTO strategyOperation)
        {
            return _strategyOperationRepository.OperationAssign(strategyOperation);
        }

        /// <summary>
        /// 撤销策略和操作的关联
        /// </summary>
        /// <param name="strategyOperation"></param>
        public bool Remove(SYSStrategyOperation strategyOperation)
        {
            return _strategyOperationRepository.Remove(strategyOperation);
        }
        #endregion
    }
}
