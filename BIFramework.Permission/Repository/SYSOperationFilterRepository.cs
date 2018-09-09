using System;

using BIStudio.Framework.Utils;
using BIStudio.Framework;
using BIStudio.Framework.Domain;


namespace BIStudio.Framework.Permission
{
    public class SYSOperationFilterRepository : Repository<SYSOperationFilter>
    {
        public SYSOperationFilter OperationAssign(SYSOperationFilterAssignDTO dto)
        {
            if (string.IsNullOrEmpty(dto.OperationCode) || string.IsNullOrEmpty(dto.OperationCode))
                throw CFException.Create(SYSOperationFilterAssignResult.SystemCodeInvalid);
            try
            {
                var filter = this.Context.Resolve<SYSFilterRepository>().Get(new EntitySpec<SYSFilter>(query => { query.SystemID = dto.SystemId; query.FilterCode = dto.FilterCode; }));
                if (!filter.ID.HasValue)
                    throw CFException.Create(SYSOperationFilterAssignResult.FilterNotFound);

                var operation = this.Context.Resolve<SYSOperationRepository>().Get(new EntitySpec<SYSOperation>(query => { query.SystemID = dto.SystemId; query.OperationCode = dto.OperationCode; }));
                if (!operation.ID.HasValue)
                    throw CFException.Create(SYSOperationFilterAssignResult.OperationNotFound);

                var operationFilter = this.Get(new EntitySpec<SYSOperationFilter>(query => { query.FilterID = filter.ID; query.OperationID = operation.ID; }));
                if (operationFilter.ID.HasValue)
                    throw CFException.Create(SYSOperationFilterAssignResult.OperationFilterAlreadyExist);

                var entity = new SYSOperationFilter
                {
                    OperationID = operation.ID,
                    FilterID = filter.ID,
                };
                this.Add(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw CFException.Create(SYSOperationFilterAssignResult.Fail, ex);
            }
        }
    }
}
