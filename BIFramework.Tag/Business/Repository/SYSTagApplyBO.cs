using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BIStudio.Framework.Data;
using BIStudio.Framework.Tag.Internal;


using BIStudio.Framework.Utils;
using BIStudio.Framework.Domain;


namespace BIStudio.Framework.Tag
{
    internal class SYSTagApplyBO : SYSTagBase<SYSTagApply>
    {
        /// <summary>
        /// 根据贴入对象和标签值查找标签贴入数据
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="tagIDs"></param>
        /// <returns>查询语句</returns>
        internal string GetTargetObjectIDByTargetIDAndTagIDs(long targetID, long[] tagIDs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var tagID in tagIDs)
                sb.Append("'" + targetID + "_" + tagID + "',");
            if (sb.Length == 0)
                sb.Append("''");
            return string.Format(SYSTagApplySql.GetTargetObjectIDSql,
                sb.ToString().Trim(','),
                this.systemID.HasValue ? ("and SystemID=" + this.systemID) : "");
        }
        /// <summary>
        /// 根据贴入对象和标签值查找标签贴入数据和匹配程度(按匹配程度匹配)
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="tagIDs"></param>
        /// <returns>查询语句</returns>
        internal string GetTargetObjectIDWithMatchRateByTargetIDAndTagIDs(long targetID, long[] tagIDs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var tagID in tagIDs)
                sb.Append("'" + targetID + "_" + tagID + "',");
            if (sb.Length == 0)
                sb.Append("''");
            return string.Format(SYSTagApplySql.GetTargetObjectIDWithMatchRateSql,
                sb.ToString().Trim(','),
                this.systemID.HasValue ? ("and SystemID=" + this.systemID) : "");
        }

        /// <summary>
        /// 根据贴入对象和标签值查找标签贴入数据和匹配程度
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="tagIDs"></param>
        /// <returns>查询语句</returns>
        internal string GetTargetObjectIDWithGroupByTargetIDAndTagIDs(long targetID, long[] tagIDs, out int tagClassCount)
        {
            if (tagIDs == null || tagIDs.Length == 0)
                tagIDs = new long[] { 0 };
            //对输入标签按"标签组"分组
            StringBuilder sb = new StringBuilder();
            var kvs = this.UnitOfWork.ToDataTable(DBBuilder.Define("select TagClassID,ID from SYSTag where id in(" + ALConvert.ToString(tagIDs) + ")"))
                .AsEnumerable()
                .GroupBy(d => d.Field<int>("TagClassID"));
            tagClassCount = kvs.Count();

            foreach (var kv in kvs)
            {
                //获取当前"标签组"的标签
                var ids = ALConvert.ToString(kv.Select(d => d.Field<int>("ID")));
                sb.AppendFormat(@"max(case when TagID in ({0}) then 1 else 0 end)+", ids);
            }

            return string.Format(SYSTagApplySql.GetTargetObjectIDWithGroupSql,
                targetID,
                sb.ToString(),
                this.systemID.HasValue ? ("and SystemID=" + this.systemID) : "");
        }

        /// <summary>
        /// 查找指定对象已贴入的标签项编号
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="targetObjectID"></param>
        /// <returns>查询语句</returns>
        internal string GetTagIDsByTargetIDAndTargetObjectID(long targetID, long targetObjectID)
        {
            return string.Format(SYSTagApplySql.GetTagIDsByTargetIDAndTargetObjectIDSql,
                targetID,
                targetObjectID,
                this.systemID.HasValue ? ("and SystemID=" + this.systemID) : "");
        }
        /// <summary>
        /// 查找指定对象已贴入的标签项编号
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="targetObjectID"></param>
        /// <returns></returns>
        internal List<long> GetTagIDValuesByTargetIDAndTargetObjectID(long targetID, long targetObjectID)
        {
            return this.UnitOfWork.ToDataTable(DBBuilder.Define(this.GetTagIDsByTargetIDAndTargetObjectID(targetID, targetObjectID)))
                .AsEnumerable().Select(d => (long)d[0]).ToList();
        }

        /// <summary>
        /// 查找指定对象已贴入的标签项
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="targetObjectID"></param>
        /// <param name="tagClassID"></param>
        /// <param name="tagGroupID"></param>
        /// <param name="DisplayLevel"></param>
        /// <returns>查询语句</returns>
        internal DataTable GetTagsByTargetIDAndTargetObjectID(long targetID, long targetObjectID, long? tagClassID, long? tagGroupID, int? DisplayLevel)
        {
            return this.UnitOfWork.ToDataTable(DBBuilder.Define(SYSTagApplySql.GetTagsByTargetIDAndTargetObjectIDSql, new
            {
                TargetID = targetID,
                TargetObjectID = targetObjectID,
                TagClassID = tagClassID,
                TagGroupID = tagGroupID,
                DisplayLevel = DisplayLevel,
                SystemID = systemID,
            }));
        }

        /// <summary>
        /// 贴入新标签
        /// </summary>
        /// <param name="applyInfo"></param>
        /// <returns></returns>
        internal bool Save(SYSTagApply applyInfo)
        {
            return (applyInfo.ID.HasValue ? (Func<SYSTagApply, bool>)this.Modify : this.Add)(applyInfo);
        }

        /// <summary>
        /// 批量贴入新标签
        /// </summary>
        /// <param name="applyInfos"></param>
        internal void Save(List<SYSTagApply> applyInfos)
        {
            applyInfos.ForEach(applyInfo => (applyInfo.ID.HasValue ? (Func<SYSTagApply, bool>)this.Modify : this.Add)(applyInfo));
        }

        /// <summary>
        /// 删除全部已贴入标签
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="targetObjectID"></param>
        /// <returns></returns>
        internal int DeleteByCompanyID(long targetID, long targetObjectID)
        {
            return this.UnitOfWork.Execute(DBBuilder.Define(SYSTagApplySql.DeleteByCompanyIDSql, new
            {
                TargetID = targetID,
                TargetObjectID = targetObjectID,
                SystemID = systemID,
            }));
        }

        /// <summary>
        /// 删除指定标签的贴入数据
        /// </summary>
        /// <param name="tagID"></param>
        /// <returns></returns>
        internal int DeleteByTagID(long tagID)
        {
            return this.UnitOfWork.Execute(DBBuilder.Define(SYSTagApplySql.DeleteByTagIDSql, new
            {
                TagID = tagID,
                SystemID = systemID,
            }));
        }

        /// <summary>
        /// 删除已贴入标签项
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="targetObjectID"></param>
        /// <param name="tagID"></param>
        /// <returns></returns>
        internal int DeleteByCompanyIDAndTagID(long targetID, long targetObjectID, long tagID)
        {
            return this.UnitOfWork.Execute(DBBuilder.Define(SYSTagApplySql.DeleteByCompanyIDAndTagIDSql, new
            {
                TargetID = targetID,
                TargetObjectID = targetObjectID,
                TagID = tagID,
                SystemID = systemID,
            }));
        }

        /// <summary>
        /// 删除已贴入标签
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="targetObjectID"></param>
        /// <param name="tagClassID"></param>
        /// <returns></returns>
        internal int DeleteByCompanyIDAndTagClassID(long targetID, long targetObjectID, long tagClassID)
        {
            return this.UnitOfWork.Execute(DBBuilder.Define(SYSTagApplySql.DeleteByCompanyIDAndTagClassIDSql, new
            {
                TargetID = targetID,
                TargetObjectID = targetObjectID,
                TagClassID = tagClassID,
                SystemID = systemID,
            }));
        }

        /// <summary>
        /// 删除已贴入标签
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="targetObjectID"></param>
        /// <param name="tagGroupID"></param>
        /// <returns></returns>
        internal int DeleteByCompanyIDAndTagGroupID(long targetID, long targetObjectID, long tagGroupID)
        {
            return this.UnitOfWork.Execute(DBBuilder.Define(SYSTagApplySql.DeleteByCompanyIDAndTagGroupIDSql, new
            {
                TargetID = targetID,
                TargetObjectID = targetObjectID,
                TagGroupID = tagGroupID,
                SystemID = systemID,
            }));
        }

    }
}
