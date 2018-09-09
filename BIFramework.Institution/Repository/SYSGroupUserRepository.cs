using System;
using BIStudio.Framework.Utils;
using BIStudio.Framework;
using BIStudio.Framework.Domain;


namespace BIStudio.Framework.Institution
{
    /// <summary>
    /// 角色账户
    /// </summary>
    [Ioc(typeof(IRepository<SYSGroupUser>))]
    public class SYSGroupUserRepository : Repository<SYSGroupUser>
    {        
        /// <summary>
        /// 分配账户角色
        /// </summary>
        public SYSGroupUser AccountAssign(SYSGroupUserAssignDTO dto)
        {
            if (string.IsNullOrEmpty(dto.GroupCode) )
                throw CFException.Create(SYSGroupUserAssignResult.SystemCodeInvalid);
            try
            {

                var group = this.Context.Resolve<SYSGroupRepository>().Get(new EntitySpec<SYSGroup>(query => { query.SystemID = dto.SystemId; query.GroupCode = dto.GroupCode; }));
                if (group.ID == null)
                    throw CFException.Create(SYSGroupUserAssignResult.GroupNotFound);

                var groupAccount = this.Get(new EntitySpec<SYSGroupUser>(query => { query.GroupID = group.ID; query.UserId = dto.AccountID; }));
                if(groupAccount.ID.HasValue)
                    throw CFException.Create(SYSGroupUserAssignResult.GroupAccountAlreadyExists);

                var entity = new SYSGroupUser
                {
                    UserId = dto.AccountID,
                    GroupID = group.ID,
                };
                this.Add(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw CFException.Create(SYSGroupUserAssignResult.Fail, ex);
            }
        }
    }
}
