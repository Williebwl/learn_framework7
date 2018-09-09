using System.Collections.Generic;
using System.Linq;
using BIStudio.Framework;
using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Institution;
using BIStudio.Framework.Tenant;
using WebApi.Controllers.Tenant;

namespace WebApi.Controllers.Institution
{
    public partial class GroupController
    {
        protected IRepository<SYSGroup> _group;
        protected IRepository<SYSGroupUser> _groupUser;
        protected IRepository<SYSAppAccess> _appAccess;

        //protected virtual IList<GroupVM> GetAll(Query info)
        //{
        //    var q = from d in _group.Entities
        //            orderby d.GroupTypeID, d.Sequence
        //            select new GroupVM
        //            {
        //                ID = d.ID,
        //                SystemID = d.SystemID,
        //                AppID = d.AppID,
        //                GroupCode = d.GroupCode,
        //                GroupName = d.GroupName,
        //                GroupType = d.GroupType,
        //                GroupTypeID = d.GroupTypeID,
        //                GroupFlag = d.GroupFlag,
        //                GroupFlagID = d.GroupFlagID,
        //                UserCount = (from b in _groupUser.Entities where b.GroupID == d.ID select b.GroupID).Count()
        //            };

        //    return q.ToArray();
        //}

        protected virtual AppGroupVM GetAppGroupInfos(long appID)
        {
            var q1 = from d in _group.Entities
                     where !_appAccess.Entities.Any(b => d.ID == b.GroupID && b.AppID == appID)
                     select d;

            var q2 = from d in _group.Entities
                     where _appAccess.Entities.Any(b => d.ID == b.GroupID && b.AppID == appID)
                     select d;


            return new AppGroupVM
            {
                AppID = appID,
                AllGroups = q1.Map<SYSGroup, GroupVM>().ToArray(),
                AppGroups = q2.Map<SYSGroup, GroupVM>().ToArray()
            };
        }
    }
}