using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIStudio.Framework.Tag
{
    internal class SYSTagApplySql
    {
#if DBTYPE_MYSQL
        internal static readonly string GetTargetObjectIDSql = @"
select distinct TargetObjectID from SYSTagApply
where
    concat(TargetID,'_',TagID) in({0}) {1}";

        /// <summary>
        /// 按匹配程度匹配
        /// </summary>
        internal static readonly string GetTargetObjectIDWithMatchRateSql = @"
select TargetObjectID, count(*) MatchRate from SYSTagApply
where concat(TargetID,'_',TagID) in({0}) {1}
group by TargetObjectID";

        /// <summary>
        /// 按标签分组匹配
        /// </summary>
        internal static readonly string GetTargetObjectIDWithGroupSql = @"
select TargetObjectID, {1}0 MatchRate from SYSTagApply
where TargetID={0} {2}
group by TargetObjectID";
        
        internal static readonly string GetTagIDsByTargetIDAndTargetObjectIDSql = @"
select distinct TagID from SYSTagApply
where
    TargetID={0} and
    TargetObjectID={1} {2}";

        internal static readonly string GetTagsByTargetIDAndTargetObjectIDSql = @"
select distinct b.* from SYSTagApply a
    inner join SYSTag b on a.TagID=b.ID
    inner join SYSTagClass c on b.TagClassID=c.ID
where
    a.TargetID=?TargetID and
    a.TargetObjectID=?TargetObjectID and
    (b.TagClassID=?TagClassID or ?TagClassID is null) and
    (c.TagGroupID=?TagGroupID or ?TagGroupID is null) and
    (c.DisplayLevel=?DisplayLevel or ?DisplayLevel is null) and
    (a.SystemID=?SystemID or a.SystemID=0 or ?SystemID is null)";

        internal static readonly string DeleteByCompanyIDSql = @"
delete from SYSTagApply
where
    TargetID=?TargetID and
    TargetObjectID=?TargetObjectID and
    (SystemID=?SystemID or SystemID=0 or ?SystemID is null)";

        internal static readonly string DeleteByTagIDSql = @"
delete from SYSTagApply
where
    TagID=?TagID and
    (SystemID=?SystemID or SystemID=0 or ?SystemID is null)";

        internal static readonly string DeleteByCompanyIDAndTagIDSql = @"
delete from SYSTagApply
where
    TargetID=?TargetID and
    TargetObjectID=?TargetObjectID and
    TagID=?TagID and
    (SystemID=?SystemID or SystemID=0 or ?SystemID is null)";

        internal static readonly string DeleteByCompanyIDAndTagClassIDSql = @"
delete from SYSTagApply
where
    TargetID=?TargetID and
    TargetObjectID=?TargetObjectID and
    TagID in (select `ID` from SYSTag where TagClassID=?TagClassID) and
    (SystemID=?SystemID or SystemID=0 or ?SystemID is null)";

        internal static readonly string DeleteByCompanyIDAndTagGroupIDSql = @"
delete from SYSTagApply
where
    TargetID=?TargetID and
    TargetObjectID=?TargetObjectID and
    TagID in (select a.`ID` from SYSTag a inner join SYSTagClass b on a.TagClassID=b.ID where b.TagGroupID=?TagGroupID) and
    (SystemID=?SystemID or SystemID=0 or ?SystemID is null)";
#else
        internal static readonly string GetTargetObjectIDSql = @"
select distinct TargetObjectID from SYSTagApply
where
    cast(TargetID as varchar)+'_'+cast(TagID as varchar) in({0}) {1}";

        /// <summary>
        /// 按匹配程度匹配
        /// </summary>
        internal static readonly string GetTargetObjectIDWithMatchRateSql = @"
select top 100 percent TargetObjectID, count(*) MatchRate from SYSTagApply
where
    cast(TargetID as varchar)+'_'+cast(TagID as varchar) in({0}) {1}
group by
    TargetObjectID";

        /// <summary>
        /// 按标签分组匹配
        /// </summary>
        internal static readonly string GetTargetObjectIDWithGroupSql = @"
select top 100 percent TargetObjectID, {1}0 MatchRate from SYSTagApply
where TargetID={0} {2}
group by TargetObjectID";

        internal static readonly string GetTagIDsByTargetIDAndTargetObjectIDSql = @"
select distinct TagID from SYSTagApply
where
    TargetID={0} and
    TargetObjectID={1} {2}";

        internal static readonly string GetTagsByTargetIDAndTargetObjectIDSql = @"
select distinct b.* from SYSTagApply a
    inner join SYSTag b on a.TagID=b.ID
    inner join SYSTagClass c on b.TagClassID=c.ID
where
    a.TargetID=@TargetID and
    a.TargetObjectID=@TargetObjectID and
    (b.TagClassID=@TagClassID or @TagClassID is null) and
    (c.TagGroupID=@TagGroupID or @TagGroupID is null) and
    (c.DisplayLevel=@DisplayLevel or @DisplayLevel is null) and
    (a.SystemID=@SystemID or a.SystemID=0 or @SystemID is null)";

        internal static readonly string DeleteByCompanyIDSql = @"
delete from SYSTagApply
where
    TargetID=@TargetID and
    TargetObjectID=@TargetObjectID and
    (SystemID=@SystemID or SystemID=0 or @SystemID is null)";

        internal static readonly string DeleteByTagIDSql = @"
delete from SYSTagApply
where
    TagID=@TagID and
    (SystemID=@SystemID or SystemID=0 or @SystemID is null)";

        internal static readonly string DeleteByCompanyIDAndTagIDSql = @"
delete from SYSTagApply
where
    TargetID=@TargetID and
    TargetObjectID=@TargetObjectID and
    TagID=@TagID and
    (SystemID=@SystemID or SystemID=0 or @SystemID is null)";

        internal static readonly string DeleteByCompanyIDAndTagClassIDSql = @"
delete from SYSTagApply
where
    TargetID=@TargetID and
    TargetObjectID=@TargetObjectID and
    TagID in (select [ID] from SYSTag where TagClassID=@TagClassID) and
    (SystemID=@SystemID or SystemID=0 or @SystemID is null)";

        internal static readonly string DeleteByCompanyIDAndTagGroupIDSql = @"
delete from SYSTagApply
where
    TargetID=@TargetID and
    TargetObjectID=@TargetObjectID and
    TagID in (select a.[ID] from SYSTag a inner join SYSTagClass b on a.TagClassID=b.ID where b.TagGroupID=@TagGroupID) and
    (SystemID=@SystemID or SystemID=0 or @SystemID is null)";
#endif

    }
}
