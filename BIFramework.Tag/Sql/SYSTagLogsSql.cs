namespace BIStudio.Framework.Tag
{
    internal class SYSTagLogsSql
    {
#if DBTYPE_MYSQL
        #region 已作废

        internal static readonly string TagLogs表的所有数据 = @"
Select  * From SYSTagLogs where (SystemID=?SystemID or SystemID=0 or ?SystemID is null) ";

        internal static readonly string GetTagLogsByObjectTagClass = @"
select * from SYSTagLogs where TagClassID=?TagClassID and TargetObjectID=?TargetObjectID and TargetObject=?TargetObject and (SystemID=?SystemID or SystemID=0 or ?SystemID is null)";

        internal static readonly string GetTagLogsByObjectTag = @"
select * from SYSTagLogs where TagID=?TagID and TagName=?TagName and TargetObject=?TargetObject and TargetObjectID=?TargetObjectID and (SystemID=?SystemID or SystemID=0 or ?SystemID is null)";

        #endregion
        
        internal static readonly string DeleteByTagIDSql = @"delete from SYSTagLogs where TagID=?TagID and (SystemID=?SystemID or SystemID=0 or ?SystemID is null)";
        
#else
        #region 已作废

        internal static readonly string TagLogs表的所有数据 = "Select  * From SYSTagLogs where (SystemID=@SystemID or SystemID=0 or @SystemID is null)";

        internal static readonly string GetTagLogsByObjectTagClass = @"
select * from SYSTagLogs where TagClassID=@TagClassID and TargetObjectID=@TargetObjectID and TargetObject=@TargetObject and (SystemID=@SystemID or SystemID=0 or @SystemID is null)";

        internal static readonly string GetTagLogsByObjectTag = @"
select * from SYSTagLogs where TagID=@TagID and TagName=@TagName and TargetObject=@TargetObject and TargetObjectID=@TargetObjectID and (SystemID=@SystemID or SystemID=0 or @SystemID is null)";

        #endregion

        internal static readonly string DeleteByTagIDSql = @"delete from SYSTagLogs where TagID=@TagID and (SystemID=@SystemID or SystemID=0 or @SystemID is null)";
#endif
        
	}
}