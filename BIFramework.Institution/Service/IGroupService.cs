using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Institution
{
    public interface IGroupService : IDomainService
    {
        #region 角色

        /// <summary>
        /// 分配账户角色
        /// </summary>
        SYSGroupUser GroupAccountAssign(SYSGroupUserAssignDTO groupAccount);

        /// <summary>
        /// 撤销账户角色
        /// </summary>
        /// <param name="groupAccount"></param>
        bool Remove(SYSGroupUser groupAccount);

        /// <summary>
        /// 将角色附加到资源点
        /// </summary>
        /// <returns></returns>
        SYSGroup GroupInject(SYSGroupInjectDTO group);
        /// <summary>
        /// 删除资源点角色
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        bool Remove(SYSGroup group);

        #endregion

        /// <summary>
        /// 获得用户角色
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        IList<SYSGroup> GetUserGroups(long userID);
    }
}
