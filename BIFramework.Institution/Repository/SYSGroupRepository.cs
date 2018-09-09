using System;
using BIStudio.Framework;
using BIStudio.Framework.Tag;
using BIStudio.Framework.Utils;
using BIStudio.Framework.Domain;


namespace BIStudio.Framework.Institution
{
    /// <summary>
    /// 角色
    /// </summary>
    [Ioc(typeof(IRepository<SYSGroup>))]
    public class SYSGroupRepository : Repository<SYSGroup>
    {
        /// <summary>
        /// 将角色附加到资源点
        /// </summary>
        /// <returns></returns>
        public SYSGroup Add(SYSGroupInjectDTO dto)
        {
            if (string.IsNullOrEmpty(dto.GroupCode) || string.IsNullOrEmpty(dto.GroupName))
                throw CFException.Create(SYSGroupInjectResult.NameOrCodeNotFound);

            try
            {
                var prevCertificate = this.Get(new EntitySpec<SYSGroup>(query => { query.SystemID = dto.SystemId;  query.GroupCode = dto.GroupCode; }));
                if (prevCertificate.ID.HasValue)
                    throw CFException.Create(SYSGroupInjectResult.CodeAlreadyExists);

                SYSGroup entity = dto.Map<SYSGroupInjectDTO, SYSGroup>();
                entity.SystemID = dto.SystemId;
                entity.InputTime = DateTime.Now;
                entity.Inputer = CFContext.User.UserName;
                entity.InputerID = CFContext.User.ID;
                this.Add(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw CFException.Create(SYSGroupInjectResult.Fail, ex);
            }
        }
    }
}
