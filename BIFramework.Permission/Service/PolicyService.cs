using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIStudio.Framework;
using BIStudio.Framework.Permission;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Institution;
using BIStudio.Framework.Utils;


namespace BIStudio.Framework.Permission
{

    [Ioc(typeof(IPolicyService))]
    public class PolicyService : DomainService, IPolicyService
    {
        private SYSOperationRepository _operationRepository;
        private SYSGroupRepository _groupRepository;
        private SYSFilterRepository _filterRepository;
        private SYSGroupUserRepository _groupAccountRepository;
        private SYSStrategyRepository _strategyRepository;
        private SYSStrategyGroupRepository _strategyGroupRepository;
        private SYSOperationFilterRepository _operationFilterRepository;
        private SYSStrategyOperationRepository _strategyOperationRepository;

        #region  策略服务
        
        /// <summary>
        /// 获得用户可用操作
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="time"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public IList<SYSOperation> GetOperations(long accountId, DateTime? time, string ip)
        {
            time = time ?? DateTime.Now;
            ip = ip ?? CFContext.User.IP;
            //字符串比较，需要补零
            ip = ALValidator.IsIP(ip) ? string.Join(".", ip.Split('.').Select(d => d.PadLeft(3, '0'))) : "127.000.000.001";

            var query = from o in _operationRepository.Entities
                        join so in _strategyOperationRepository.Entities on o.ID equals so.OperationID
                        join s in _strategyRepository.Entities on so.StrategyID equals s.ID
                        join sg in _strategyGroupRepository.Entities on s.ID equals sg.StrategyID
                        join g in _groupRepository.Entities on sg.GroupID equals g.ID
                        join gu in _groupAccountRepository.Entities on g.ID equals gu.GroupID
                        where
                            gu.UserId == accountId &&
                            (s.StartTime == null || s.StartTime <= time) &&
                            (s.EndTime == null || s.EndTime >= time) &&
                            (s.StartIP == null || string.Compare(s.StartIP, ip) <= 0) &&
                            (s.EndIP == null || string.Compare(s.EndIP, ip) >= 0)
                        select o;
            return query.ToList();
        }

        /// <summary>
        /// 根据操作代码获取过滤器
        /// </summary>
        /// <param name="operationCode"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        private IList<SYSFilter> GetFiltersByOperation(string operationCode, string entityType)
        {
            var query = from f in _filterRepository.Entities
                        join of in _operationFilterRepository.Entities on f.ID equals of.FilterID
                        join o in _operationRepository.Entities on of.OperationID equals o.ID
                        where o.OperationCode == operationCode && f.EntityType == entityType
                        select f;
            return query.ToList();
        }

        /// <summary>
        /// 获得操作过滤器
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="operationCode"></param>
        /// <returns></returns>
        public ISpecification<TEntity> GetFilter<TEntity>(string operationCode) where TEntity : class
        {
            Type entityType = typeof(TEntity);
            ISpecification<TEntity> result = null;
            GetFiltersByOperation(operationCode, entityType.FullName).ForEach(filter =>
            {
                var validator = FilterProviderService.Default.GetFilter(filter.FilterOperation);
                validator.Init(filter);
                ISpecification<TEntity> spec = (ISpecification<TEntity>)validator.GetType().GetMethod("Compile").MakeGenericMethod(entityType).Invoke(validator, null);
                result = (result != null ? result.And(spec) : spec);
            });
            return result ?? new TrueSpec<TEntity>();
        }

        /// <summary>
        /// 对数据源应用过滤器
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <param name="operationCode"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> ApplyFilter<TEntity>(IEnumerable<TEntity> source, string operationCode) where TEntity : class
        {
            return source.Where(this.GetFilter<TEntity>(operationCode).Lambda.Compile());
        }
        
        #endregion
    }
}
