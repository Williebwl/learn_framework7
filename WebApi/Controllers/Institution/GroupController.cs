using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using BIStudio.Framework.Data;
using BIStudio.Framework.Institution;
using BIStudio.Framework.UI;
using WebApi.Controllers.Tenant;

namespace WebApi.Controllers.Institution
{
    /// <summary>
    /// 用户组
    /// </summary>
    public partial class GroupController : ApplicationService<GroupVM, Query>
    {
        public override IEnumerable<GroupVM> GetAll(Query info)
        {
            var q = from d in _group.Entities
                    orderby d.GroupTypeID, d.Sequence
                    select new GroupVM
                    {
                        ID = d.ID,
                        SystemID = d.SystemID,
                        AppID = d.AppID,
                        GroupCode = d.GroupCode,
                        GroupName = d.GroupName,
                        GroupType = d.GroupType,
                        GroupTypeID = d.GroupTypeID,
                        GroupFlag = d.GroupFlag,
                        GroupFlagID = d.GroupFlagID,
                        UserCount = (from b in _groupUser.Entities where b.GroupID == d.ID select b.GroupID).Count()
                    };

            return q.ToArray();
        }

        //public virtual IList<GroupVM> GetNav() => GetInfoForNav();


        public virtual AppGroupVM GetAppGroups(long id) => GetAppGroupInfos(id);

    }
}