using System;
using BIStudio.Framework;

using BIStudio.Framework.Tag;
using BIStudio.Framework.Utils;
using BIStudio.Framework.Domain;


namespace BIStudio.Framework.Permission
{
    public class SYSStrategyRepository : Repository<SYSStrategy>
    {
        internal SYSStrategy StrategyInject(SYSStrategyDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Code) || string.IsNullOrEmpty(dto.Name))
                throw CFException.Create(SYSStrategyResult.NameOrCodeNotFound);

            try
            {
                var tagService = TagService.GetInstance();
                tagService.DependOn(this.Context);
                SYSTag tagInfo = tagService.GetTag(dto.ResourceTagID);
                if (tagInfo.ID == null)
                    throw CFException.Create(SYSStrategyResult.ResourceTagInvalid);
                SYSTagClass tagClassInfo = tagService.GetTagClass(tagInfo.TagClassID ?? 0);
                if (tagClassInfo.ID == null)
                    throw CFException.Create(SYSStrategyResult.ResourceTagInvalid);

                var prevCertificate = this.Get(new EntitySpec<SYSStrategy>(query => { query.SystemID = dto.SystemId; query.StrategyCode = dto.Code; }));
                if (prevCertificate.ID.HasValue)
                    throw CFException.Create(SYSStrategyResult.CodeAlreadyExists);

                SYSStrategy entity = dto.Map<SYSStrategyDTO, SYSStrategy>();
                entity.SystemID = dto.SystemId;
                entity.InputTime = DateTime.Now;
                entity.Inputer = CFContext.User.UserName;
                entity.InputerID = CFContext.User.ID;
                this.Add(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw CFException.Create(SYSStrategyResult.Fail, ex);
            }
        }
    }
}
