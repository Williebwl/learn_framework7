namespace BIStudio.Framework.Tag
{
    internal class SYSTagSql
    {
#if DBTYPE_MYSQL
        #region 已作废

        internal static readonly string Tag表的所有数据 = @"
Select  * From SYSTag
where
    (SystemID=?SystemID or SystemID=0 or ?SystemID is null)
order by
    TagClassID,ComputedSequence";

        internal static readonly string DeleteTagByTagParentID = @"
delete from SYSTag where ParentID in ({0}) where (SystemID=?SystemID or SystemID=0 or ?SystemID is null)
";

        internal static readonly string GetChildTagByParentID = @"
select * from SYSTag  where `Path` like ?Path and (SystemID=?SystemID or SystemID=0 or ?SystemID is null)
";
        #endregion

        internal static readonly string TagByCodePrefix = @"
Select  * From SYSTag
Where
    `TagClassID` in (Select a.ID From `SYSTagClass` a left join `vw_SYSTagGroup` b on a.TagGroupID=b.ID Where b.GroupCode=?ClassCode and (a.DisplayLevel=?DisplayLevel or ?DisplayLevel is null)) and
    (SystemID=?SystemID or SystemID=0 or ?SystemID is null)
order by
    TagClassID,ComputedSequence";

        internal static readonly string UpdateChildTagByParant = @"
update SYSTag,vw_SYSTag
set
    `SYSTag`.`Path`=vw_SYSTag.NewPath,
    `SYSTag`.`Layer`=vw_SYSTag.NewLayer,
    `SYSTag`.`IsLeaf`=vw_SYSTag.NewIsLeaf,
    `SYSTag`.`ComputedSequence`=vw_SYSTag.NewSequence
where
	`SYSTag`.`ID`=vw_SYSTag.ID and
    (vw_SYSTag.TagClassID=?TagClassID or ?TagClassID is null) and
    (vw_SYSTag.SystemID=?SystemID or vw_SYSTag.SystemID=0 or ?SystemID is null)";

        internal static readonly string GetTagByTagClass = @"
select * from SYSTag
where
    TagClassID in (select ID from SYSTagClass where ClassName=?ClassName) and
    (SystemID=?SystemID or SystemID=0 or ?SystemID is null)
order by
    ComputedSequence";

        internal static readonly string GetTagByTagClassID = @"
select * from SYSTag
where
    TagClassID=?TagClassID and
    (ParentID=?ParentID or ?ParentID is null) and
    (SystemID=?SystemID or SystemID=0 or ?SystemID is null)
order by
    ComputedSequence";

        internal static readonly string GetTagByTagClassCode = @"
select * from SYSTag
where
    TagClassID in (select ID from SYSTagClass where ClassCode=?ClassCode) and
    (SystemID=?SystemID or SystemID=0 or ?SystemID is null)
order by
    ComputedSequence";

        internal static readonly string GetTagByTagClassCode_TagName = @"
select * from SYSTag
where 
    TagClassID in (select ID from SYSTagClass where ClassCode=?ClassCode) and
    tagName=?tagName and
    (SystemID=?SystemID or SystemID=0 or ?SystemID is null)
order by
    ComputedSequence";

        internal static readonly string GetTagByTagClassCode_TagCode = @"
select * from SYSTag
where
    TagClassID in (select ID from SYSTagClass where ClassCode=?ClassCode) and
    tagCode=?tagCode and
    (SystemID=?SystemID or SystemID=0 or ?SystemID is null)
order by
    ComputedSequence";

        internal static readonly string GetTagByTagClassCode_TagValue = @"
select * from SYSTag 
where 
    TagClassID in (select ID from SYSTagClass where ClassCode=?ClassCode) and
    tagCode=?tagValue and
    (SystemID=?SystemID or SystemID=0 or ?SystemID is null)
order by
    ComputedSequence";

        internal static readonly string GetTagsByParentID = @"
select * from SYSTag 
where 
    TagClassID in (select ID from SYSTagClass where ClassCode=?ClassCode) and 
    (ParentID=?ParentID or ?ParentID is null) and 
    (SystemID=?SystemID or SystemID=0 or ?SystemID is null)
order by 
    ComputedSequence";

        internal static readonly string GetTagsByParentName = @"
select a.* from SYStag a
    left join SYStag b on a.ParentID=b.ID
    inner join SYStagclass c on a.TagClassID=c.ID
where
    (c.ClassCode=?ClassCode or ?ClassCode is null) and
    (b.TagName=?ParentName or ?ParentName is null) and
    (a.SystemID=?SystemID or a.SystemID=0 or ?SystemID is null)
order by
    a.ComputedSequence";

        
        internal static readonly string GetDBName = @"SELECT DATABASE()";
        
        internal static readonly string UpdateHashID = @"update {0} set ID=ifnull((select max(ID)+1 from {0} where ID between @HashIDMin and @MashIDMax),@HashIDMin) where ID=@ID";
#else
        #region 已作废

        internal static readonly string Tag表的所有数据 = @"Select  * From [SYSTag]
where
    (SystemID=@SystemID or SystemID=0 or @SystemID is null)
order by
    TagClassID,ComputedSequence";

        internal static readonly string DeleteTagByTagParentID = @"
delete from SYSTag where ParentID in ({0}) where (SystemID=@SystemID or SystemID=0 or @SystemID is null)
";

        internal static readonly string GetChildTagByParentID = @"
select * from SYSTag  where [Path] like @Path and (SystemID=@SystemID or SystemID=0 or @SystemID is null)
";
        #endregion

        internal static readonly string TagByCodePrefix = @"
Select  * From [SYSTag]
Where
    [TagClassID] in (Select a.ID From [SYSTagClass] a left join [vw_SYSTagGroup] b on a.TagGroupID=b.ID Where b.GroupCode=@ClassCode and (a.DisplayLevel=@DisplayLevel or @DisplayLevel is null)) and
    (SystemID=@SystemID or SystemID=0 or @SystemID is null)
order by
    TagClassID,ComputedSequence";

        internal static readonly string UpdateChildTagByParant = @"
update vw_SYSTag
set
    [Path]=NewPath,
    [Layer]=NewLayer,
    [IsLeaf]=NewIsLeaf,
    [ComputedSequence]=NewSequence
where
    (TagClassID=@TagClassID or @TagClassID is null) and
    (SystemID=@SystemID or SystemID=0 or @SystemID is null)";

        internal static readonly string GetTagByTagClass = @"
select * from SYSTag
where
    TagClassID in (select ID from SYSTagClass where ClassName=@ClassName) and
    (SystemID=@SystemID or SystemID=0 or @SystemID is null)
order by
    ComputedSequence";

        internal static readonly string GetTagByTagClassID = @"
select * from SYSTag
where
    TagClassID=@TagClassID and
    (ParentID=@ParentID or @ParentID is null) and
    (SystemID=@SystemID or SystemID=0 or @SystemID is null)
order by
    ComputedSequence";

        internal static readonly string GetTagByTagClassCode = @"
select * from SYSTag
where
    TagClassID in (select ID from SYSTagClass where ClassCode=@ClassCode) and
    (SystemID=@SystemID or SystemID=0 or @SystemID is null)
order by
    ComputedSequence";

        internal static readonly string GetTagByTagClassCode_TagName = @"
select * from SYSTag
where 
    TagClassID in (select ID from SYSTagClass where ClassCode=@ClassCode) and
    tagName=@tagName and
    (SystemID=@SystemID or SystemID=0 or @SystemID is null)
order by
    ComputedSequence";

        internal static readonly string GetTagByTagClassCode_TagCode = @"
select * from SYSTag
where
    TagClassID in (select ID from SYSTagClass where ClassCode=@ClassCode) and
    tagCode=@tagCode and
    (SystemID=@SystemID or SystemID=0 or @SystemID is null)
order by
    ComputedSequence";

        internal static readonly string GetTagByTagClassCode_TagValue = @"
select * from SYSTag 
where 
    TagClassID in (select ID from SYSTagClass where ClassCode=@ClassCode) and
    tagCode=@tagValue and
    (SystemID=@SystemID or SystemID=0 or @SystemID is null)
order by
    ComputedSequence";

        internal static readonly string GetTagsByParentID = @"
select * from SYSTag 
where 
    TagClassID in (select ID from SYSTagClass where ClassCode=@ClassCode) and 
    (ParentID=@ParentID or @ParentID is null) and 
    (SystemID=@SystemID or SystemID=0 or @SystemID is null)
order by 
    ComputedSequence";

        internal static readonly string GetTagsByParentName = @"
select a.* from SYStag a
    left join SYStag b on a.ParentID=b.ID
    inner join SYStagclass c on a.TagClassID=c.ID
where
    (c.ClassCode=@ClassCode or @ClassCode is null) and
    (b.TagName=@ParentName or @ParentName is null) and
    (a.SystemID=@SystemID or a.SystemID=0 or @SystemID is null)
order by
    a.ComputedSequence";

        internal static readonly string GetDBName = @"select db_name()";
        internal static readonly string UpdateHashID = @"update {0} set ID=isnull((select max(ID)+1 from {0} where ID between @HashIDMin and @MashIDMax),@HashIDMin) where ID=@ID";
#endif
	}
}