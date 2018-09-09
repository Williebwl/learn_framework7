using System;

using BIStudio.Framework.Utils;
using BIStudio.Framework;
using BIStudio.Framework.Domain;


namespace BIStudio.Framework.Permission
{
    public class SYSStrategyOperationRepository : Repository<SYSStrategyOperation>
    {
        public SYSStrategyOperation OperationAssign(SYSStrategyOperationAssignDTO dto)
        {
            if (string.IsNullOrEmpty(dto.StrategyCode) || string.IsNullOrEmpty(dto.OperationCode))
                throw CFException.Create(SYSStrategyOperationAssignResult.SystemCodeInvalid);
            try
            {

                var operation = this.Context.Resolve<SYSOperationRepository>().Get(new EntitySpec<SYSOperation>(query => { query.SystemID = dto.SystemId; query.OperationCode = dto.OperationCode; }));
                if (!operation.ID.HasValue)
                    throw CFException.Create(SYSStrategyOperationAssignResult.OperationNotFound);

                var strategy = this.Context.Resolve<SYSStrategyRepository>().Get(new EntitySpec<SYSStrategy>(query => { query.SystemID = dto.SystemId; query.StrategyCode = dto.StrategyCode; }));
                if (!strategy.ID.HasValue)
                    throw CFException.Create(SYSStrategyOperationAssignResult.StrategyNotFound);

                var strategyGroup = this.Get(new EntitySpec<SYSStrategyOperation>(query => { query.OperationID = operation.ID; query.StrategyID = strategy.ID; }));
                if (strategyGroup.ID.HasValue)
                    throw CFException.Create(SYSStrategyOperationAssignResult.StrategyOperationAlreadyExist);

                var entity = new SYSStrategyOperation
                {
                    StrategyID = strategy.ID,
                    OperationID = operation.ID,
                };
                this.Add(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw CFException.Create(SYSStrategyOperationAssignResult.Fail, ex);
            }
        }
    }
}
