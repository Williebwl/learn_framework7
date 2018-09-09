using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIStudio.Framework.Tag
{
    internal class SYSTagSearchSql
    {
#if DBTYPE_MYSQL
        internal static readonly string TableNameSql = @"`{0}` t{1}";
        internal static readonly string InSql = @"t{0}.`ID` in ({1})";
        internal static readonly string InnerJoinSql = @" inner join ({1}) tApply on t{0}.`ID`=tApply.`TargetObjectID`";
        internal static readonly string JoinSql = @"join `{0}` t{1} on t1.`{3}`=t{1}.`{2}`";
        internal static readonly string MatchRateEqualsSql = @"tApply.`MatchRate`={0}";
#else
        internal static readonly string TableNameSql = "[{0}] t{1}";
        internal static readonly string InSql = @"t{0}.[ID] in ({1})";
        internal static readonly string InnerJoinSql = @" inner join ({1}) tApply on t{0}.[ID]=tApply.[TargetObjectID]";
        internal static readonly string JoinSql = @"join [{0}] t{1} on t1.[{4}]=t{1}.[{2}]";
        internal static readonly string MatchRateEqualsSql = @"tApply.[MatchRate]={0}";
#endif
    }
}
