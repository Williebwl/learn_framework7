namespace BIStudio.Framework.Tag
{
    internal class SYSTagClassSql
    {
#if DBTYPE_MYSQL
        internal static readonly string TagClass表的所有数据 = @"
Select  * From `SYSTagClass`
where
    (SystemID=?SystemID or SystemID=0 or ?SystemID is null)";
        internal static readonly string TagClassByCodePrefix = @"
Select a.* From `SYSTagClass` a
    left join `vw_SYSTagGroup` b on a.TagGroupID=b.ID
Where 
    (b.GroupCode=?GroupCode or ?GroupCode is null) and
    (a.DisplayLevel=?DisplayLevel or ?DisplayLevel is null) and
    (a.SystemID=?SystemID or a.SystemID=0 or ?SystemID is null)
order by
    b.Sequence,a.Sequence";

        internal static readonly string SaveSql = @"
update SYSTag set TagClass=?TagClass where TagClassID=?TagClassID and (SystemID=?SystemID or SystemID=0 or ?SystemID is null)";
#else
        internal static readonly string TagClass表的所有数据 = @"Select  * From SYSTagClass
where
    (SystemID=@SystemID or SystemID=0 or @SystemID is null)";
        internal static readonly string TagClassByCodePrefix = @"
Select a.* From SYSTagClass a
    left join vw_SYSTagGroup b on a.TagGroupID=b.ID
Where 
    (b.GroupCode=@GroupCode or @GroupCode is null) and
    (a.DisplayLevel=@DisplayLevel or @DisplayLevel is null) and
    (a.SystemID=@SystemID or a.SystemID=0 or @SystemID is null)
order by
    b.Sequence,a.Sequence";

        internal static readonly string SaveSql = @"
update SYSTag set TagClass=@TagClass where TagClassID=@TagClassID and (SystemID=@SystemID or SystemID=0 or @SystemID is null)";
#endif
	}
}