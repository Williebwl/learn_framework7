using System;
using System.Collections.Generic;
using System.Linq;
    using BIStudio.Framework;
    using BIStudio.Framework.Data;
    using BIStudio.Framework.Domain;
    using BIStudio.Framework.UI;
using BIStudio.Framework.Utils;

namespace WebApi.Controllers.Auth
{

    public partial class AccountController : ApplicationService<AccountVM, AccountPageQuery>
    {
        public virtual EditableKeyValuePair<string, int>[] GetAllStatus() => GetStatus();

        public override PagedList<AccountVM> Get(AccountPageQuery info) => GetAccountForPage(info);

        public override AccountVM Post(AccountVM vm)
        {
            this.Validate(vm);

            return Save(vm);
        }
        public override AccountVM Put(long id, AccountVM vm)
        {
            this.Validate(vm);

            return Save(id, vm);
        }
        public override bool Delete(string ids) => accountBO.Remove(ids);
    }
}