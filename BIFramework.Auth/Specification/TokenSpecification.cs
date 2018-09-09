using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Auth
{
    public class TokenSpecification : SpecificationBase<SYSToken>
    {
        /// <summary>
        /// 查找令牌
        /// </summary>
        /// <param name="systemID">系统编号</param>
        /// <param name="accessToken">令牌代码，传NULL则查找系统令牌</param>
        /// <param name="checkExpires">是否包含超时的令牌</param>
        public TokenSpecification(long systemID, string accessToken = null, bool checkExpires = true)
        {
            Sql = DBBuilder.Define()
                .Eq(new { SystemID = systemID })
                .And(d => d.Gt(new { ExpiresIn = DateTime.Now }), checkExpires);

            if (string.IsNullOrEmpty(accessToken))
                Sql.IsNull("AccessToken");
            else
                Sql.Eq(new { AccessToken = accessToken });
        }
        /// <summary>
        /// 查找令牌
        /// </summary>
        /// <param name="systemID">系统编号</param>
        /// <param name="passportID">用户代码，传NULL则查找系统令牌</param>
        /// <param name="checkExpires">是否包含超时的令牌</param>
        public TokenSpecification(long systemID, long? passportID = null, bool checkExpires = true)
        {
            Sql = DBBuilder.Define()
                .Eq(new { SystemID = systemID })
                .And(d => d.Gt(new { ExpiresIn = DateTime.Now }), checkExpires)
                .And(d =>
                {
                    if (passportID == null)
                        d.IsNull("PassportID");
                    else
                        d.Eq(new { PassportID = passportID });
                });
        }
    }
}
