using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIStudio.Framework.Tag
{
    internal class SYSTagAuthoritySql
    {
#if DBTYPE_MYSQL
        internal static readonly string GetAuthorityByCurrentUserSql = @"
select ObjectType,ObjectValue from SYSTagAuthority
where
    (
     (AuthorityType='User' and AuthorityValue=?CurrentUserID) or
     (AuthorityType='Role' and AuthorityValue in (select RoleID from 'RoleUser' where UserID=?CurrentUserID)) or
     (AuthorityType='Dept' and AuthorityValue in (select DeptID from 'User' where ID=?CurrentUserID))
    ) and
    AcceptOperate|?AcceptOperate=AcceptOperate and
    (SystemID=?SystemID or SystemID=0 or ?SystemID is null)
";
#else
        internal static readonly string GetAuthorityByCurrentUserSql = @"
select ObjectType,ObjectValue from SYSTagAuthority
where
    (
     (AuthorityType='User' and AuthorityValue=@CurrentUserID) or
     (AuthorityType='Role' and AuthorityValue in (select RoleID from [RoleUser] where UserID=@CurrentUserID)) or
     (AuthorityType='Dept' and AuthorityValue in (select DeptID from [User] where ID=@CurrentUserID))
    ) and
    AcceptOperate|@AcceptOperate=AcceptOperate and
    (SystemID=@SystemID or SystemID=0 or @SystemID is null)
";
#endif
    }
}
