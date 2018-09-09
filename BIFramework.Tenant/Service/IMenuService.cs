using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Data;

namespace BIStudio.Framework.Tenant
{
    public interface IMenuService : IDomainService
    {
        bool Delete(IList<long> ids);
        bool Delete(long id);
        bool Delete(string ids);
        IList<SYSMenu> GetChildren(IList<long> groupIDs, long pid);
        IList<SYSMenu> GetChildren(long groupID, long pid);
        IList<SYSMenu> GetChildrenAndSelf(IList<long> groupIDs, long pid);
        IList<SYSMenu> GetChildrenAndSelf(long groupID, long pid);
        IList<SYSMenu> GetChildrens(IList<long> groupIDs, long pid);
        IList<SYSMenu> GetChildrens(long groupID, long pid);
        IList<SYSMenu> GetChildrensAndSelf(IList<long> groupIDs, long pid);
        IList<SYSMenu> GetChildrensAndSelf(long groupID, long pid);
        SYSMenu GetInfo(long id);
        IList<SYSMenu> GetInfos(IList<long> ids);
        IList<SYSMenu> GetInfos(string ids);
        IList<SYSMenu> GetInfosByGroup(IList<long> groupIDs);
        IList<SYSMenu> GetInfosByGroup(long groupID);
        IList<SYSMenu> GetRoot(IList<long> groupIDs);
        IList<SYSMenu> GetRoot(long groupID);
        IList<SYSMenu> GetRoute(IList<long> groupIDs);
        IList<SYSMenu> GetRoute(long groupID);
        long Save(SYSMenu info);
        bool Save(params SYSMenu[] infos);

        IList<SYSMenu> GetInfoByAppId(long appID);
    }
}
