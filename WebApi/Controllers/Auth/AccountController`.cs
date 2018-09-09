using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
    using BIStudio.Framework;
    using BIStudio.Framework.Auth;
    using BIStudio.Framework.Data;
    using BIStudio.Framework.Domain;
    using BIStudio.Framework.Utils;


namespace WebApi.Controllers.Auth
{
    public partial class AccountController
    {
        protected IRepository<SYSAccount> accountBO;
        protected IRepository<SYSPassport> passportBO;

        protected virtual PagedList<AccountVM> GetAccountForPage(AccountPageQuery info)
        {
            var key = info != null ? info.Key : null;
            var query = from a in accountBO.Entities
                        join p in passportBO.Entities on a.PassportID equals p.ID
                        where a.SystemID == CFContext.User.SystemID && (key == null || a.UserName.Contains(key) || p.LoginName.Contains(key))
                        orderby a.ID descending
                        select new AccountVM
                        {
                            RealName = a.RealName,
                            LoginName = p.LoginName,
                            Email = p.Email,
                            LastLoginTime = p.LastLoginTime,
                            IsValid = p.IsValid,
                        };
            return query.ToPagedList(info);
        }
        
        protected virtual EditableKeyValuePair<string, int>[] GetStatus()
        {
            return ALEnumDescription.GetFieldTexts(typeof(AccountStatusEnum)).Select(d => new EditableKeyValuePair<string, int>(d.EnumDisplayText, d.EnumValue)).ToArray();
        }

        protected virtual AccountVM Save(AccountVM vm)
        {
            return vm;
        }

        protected virtual AccountVM Save(long id, AccountVM vm)
        {
            return vm;
        }
    }
}