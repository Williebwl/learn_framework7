using System;
using BIStudio.Framework.Utils;
using BIStudio.Framework;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Institution;


namespace BIStudio.Framework.Permission
{
    public class SYSStrategyGroupRepository : Repository<SYSStrategyGroup>
    {
        public SYSStrategyGroup GroupAssign(SYSStrategyGroupAssignDTO dto)
        {
            if (string.IsNullOrEmpty(dto.GroupCode) || string.IsNullOrEmpty(dto.StrategyCode))
                throw CFException.Create(SYSStrategyGroupAssignResult.SystemCodeInvalid);
            try
            {
                var group = this.Context.Resolve<SYSGroupRepository>().Get(new EntitySpec<SYSGroup>(query => { query.SystemID = dto.SystemId; query.GroupCode = dto.GroupCode; }));
                if (!group.ID.HasValue)
                    throw CFException.Create(SYSStrategyGroupAssignResult.GroupNotFound);

                var strategy = this.Context.Resolve<SYSStrategyRepository>().Get(new EntitySpec<SYSStrategy>(query => { query.SystemID = dto.SystemId; query.StrategyCode = dto.StrategyCode; }));
                if (!strategy.ID.HasValue)
                    throw CFException.Create(SYSStrategyGroupAssignResult.StrategyNotFound);

                var strategyGroup = this.Get(new EntitySpec<SYSStrategyGroup>(query => { query.GroupID = group.ID; query.StrategyID = strategy.ID; }));
                if (strategyGroup.ID.HasValue)
                    throw CFException.Create(SYSStrategyGroupAssignResult.StrategyGroupAlreadyExist);

                var entity = new SYSStrategyGroup
                {
                    StrategyID = strategy.ID,
                    GroupID = group.ID,
                };
                this.Add(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw CFException.Create(SYSStrategyGroupAssignResult.Fail, ex);
            }
        }
    }
}
