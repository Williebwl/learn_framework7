using System;
using BIStudio.Framework;

using BIStudio.Framework.Tag;
using BIStudio.Framework.Utils;
using BIStudio.Framework.Domain;


namespace BIStudio.Framework.Permission
{
    public class SYSFilterRepository : Repository<SYSFilter>
    {
        /// <summary>
        /// 将操作附加到资源点
        /// </summary>
        /// <returns></returns>
        internal SYSFilter Add(SYSFilterInjectDTO dto)
        {
            if (string.IsNullOrEmpty(dto.FilterCode) || string.IsNullOrEmpty(dto.FilterName))
                throw CFException.Create(SYSFilterInjectResult.NameOrCodeNotFound);

            try
            {
                var prevCertificate = this.Get(new EntitySpec<SYSFilter>(query => { query.SystemID = dto.SystemId;  query.FilterCode = dto.FilterCode; }));
                if (prevCertificate.ID.HasValue)
                    throw CFException.Create(SYSFilterInjectResult.CodeAlreadyExists);



                SYSFilter entity = dto.Map<SYSFilterInjectDTO, SYSFilter>();
                entity.SystemID = dto.SystemId;
                entity.InputTime = DateTime.Now;
                entity.Inputer = CFContext.User.UserName;
                entity.InputerID = CFContext.User.ID;
                this.Add(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw CFException.Create(SYSFilterInjectResult.Fail, ex);
            }
        }
    }
}
