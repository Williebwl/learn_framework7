using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Institution
{
    [Ioc(typeof(IGroupService))]
    public class GroupService : DomainService, IGroupService
    {
        #region 初始化

        private SYSGroupRepository _groupRepository;
        private SYSGroupUserRepository _groupAccountRepository;

        #endregion

        #region 角色

        /// <summary>
        /// 分配账户角色
        /// </summary>
        public SYSGroupUser GroupAccountAssign(SYSGroupUserAssignDTO groupAccount)
        {
            return _groupAccountRepository.AccountAssign(groupAccount);
        }

        /// <summary>
        /// 撤销账户角色
        /// </summary>
        /// <param name="groupAccount"></param>
        public bool Remove(SYSGroupUser groupAccount)
        {
            return _groupAccountRepository.Remove(groupAccount);
        }

        /// <summary>
        /// 将角色附加到资源点
        /// </summary>
        /// <returns></returns>
        public SYSGroup GroupInject(SYSGroupInjectDTO group)
        {
            return _groupRepository.Add(group);
        }
        /// <summary>
        /// 删除资源点角色
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public bool Remove(SYSGroup group)
        {
            return _groupRepository.Remove(group);
        }

        #endregion

        /// <summary>
        /// 获得用户角色
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IList<SYSGroup> GetUserGroups(long userID)
        {
            var query = from g in _groupRepository.Entities
                        join gu in _groupAccountRepository.Entities on g.ID equals gu.GroupID
                        where gu.UserId == userID
                        select g;
            return query.ToList();
        }
    }
}
