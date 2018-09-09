using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BIStudio.Framework.Data;
using BIStudio.Framework.Tag.Internal;
using BIStudio.Framework.Utils;
using BIStudio.Framework.Domain;

namespace BIStudio.Framework.Tag
{
    public partial class SYSTagBO : SYSTagBase<SYSTag>
    {
        /// <summary>
        /// ���ݱ�ǩ���ͻ�ñ�ǩ
        /// </summary>
        /// <param name="TagClass"></param>
        /// <returns></returns>
        [Obsolete("��ʹ��TagProvider.GetInstance����ʵ��")]
        public DataTable GetTagByTagClass(string TagClass)
        {
            return this.UnitOfWork.ToDataTable(DBBuilder.Define(SYSTagSql.GetTagByTagClass, new
            {
                ClassName = TagClass,
                SystemID = systemID,
            }));
        }
        /// <summary>
        /// ���ݱ�ǩ���ͻ�ñ�ǩ
        /// </summary>
        /// <param name="TagClassID"></param>
        /// <returns></returns>
        [Obsolete("��ʹ��GetTagByTagClassID(long TagClassID,int? ParentTagID)����")]
        public DataTable GetTagByTagClassID(string TagClassID)
        {
            return this.GetTagByTagClassID(ALConvert.ToInt0(TagClassID), (long?)null);
        }

        /// <summary>
        /// ���ݱ�ǩ���ͻ�ñ�ǩ
        /// </summary>
        /// <param name="TagClassID"></param>
        /// <param name="ParentTagID"></param>
        /// <returns></returns>
        public DataTable GetTagByTagClassID(long TagClassID, long? ParentTagID)
        {
            return this.UnitOfWork.ToDataTable(DBBuilder.Define(SYSTagSql.GetTagByTagClassID, new
            {
                TagClassID = TagClassID,
                ParentID = ParentTagID,
                SystemID = systemID,
            }));
        }

        /// <summary>
        /// ���ݱ�ǩ���ͻ�ñ�ǩ
        /// </summary>
        /// <param name="TagClassID"></param>
        /// <param name="ParentTagName"></param>
        /// <returns></returns>
        public DataTable GetTagByTagClassID(long TagClassID, string ParentTagName)
        {
            long? parentTagID = this.getIDByTagName(ParentTagName, TagClassID);
            return this.GetTagByTagClassID(TagClassID, parentTagID);
        }

        /// <summary>
        /// ��Tag�����½���༭һ������
        /// </summary>
        /// <returns></returns>
        public void Save(SYSTag tagInfo)
        {
            SYSTag tagParentInfo = this.Get(tagInfo.ParentID.Value);
            (tagInfo.ID.HasValue ? (Func<SYSTag, bool>)this.Modify : this.Add)(tagInfo);
            if (tagParentInfo != null && tagParentInfo.Path != null)
            {
                tagParentInfo.IsLeaf = 0;
                (tagInfo.ID.HasValue ? (Func<SYSTag, bool>)this.Modify : this.Add)(tagParentInfo);
                tagInfo.Path = tagParentInfo.Path.Trim(',') + "," + tagInfo.ID.ToString() + ",";
                tagInfo.Layer = tagParentInfo.Path.Trim(',').Split(',').Length;
                (tagInfo.ID.HasValue ? (Func<SYSTag, bool>)this.Modify : this.Add)(tagInfo);
            }
            else
            {
                tagInfo.Path = "0," + tagInfo.ID.ToString() + ",";
                tagInfo.Layer = tagInfo.Path.Trim(',').Split(',').Length - 1;
                (tagInfo.ID.HasValue ? (Func<SYSTag, bool>)this.Modify : this.Add)(tagInfo);
            }

            UpdateChildTagByParant(tagInfo.TagClassID.Value);
        }

        /// <summary>
        /// ����TagClassɾ��Tag���е�����
        /// </summary>
        /// <param name="tagClassIDs"></param>
        public void DeleteTagByTagClass(string tagClassIDs)
        {
            string sqlStr = "delete from SYSTag  where TagClassID in (" + tagClassIDs + ")";
            this.UnitOfWork.Execute(DBBuilder.Define(sqlStr));
        }

        /// <summary>
        /// ����Tag���ƻ��ID
        /// </summary>
        /// <param name="TagCode"></param>
        /// <returns></returns>
        private long? getIDByTagName(string tagName, long? tagClassID)
        {
            if (string.IsNullOrEmpty(tagName) || !tagClassID.HasValue)
                return null;

            SYSTag info = new SYSTag { TagName = tagName, TagClassID = tagClassID };
            info.Property.ColumnAttributes["TagName"].IsExact = true;
            info = this.Get(info.AsSpec());
            return info != null ? (info.ID) : null;
        }
        /// <summary>
        /// ����Tag��Ż��ID
        /// </summary>
        /// <param name="TagCode"></param>
        /// <returns></returns>
        private long? getIDByTagCode(string tagCode, long? tagClassID)
        {
            if (string.IsNullOrEmpty(tagCode) || !tagClassID.HasValue)
                return null;

            SYSTag info = new SYSTag { TagCode = tagCode, TagClassID = tagClassID };
            info.Property.ColumnAttributes["TagCode"].IsExact = true;
            info = this.Get(info.AsSpec());
            return info != null ? (info.ID) : null;
        }
        /// <summary>
        /// ����Tag���ID
        /// </summary>
        /// <param name="tagInfo">��ѯ��������Ҫָ��ID��TagCode��TagName����</param>
        /// <returns>��ǩ��ID��NULL</returns>
        public long? GetIDByTag(SYSTag tagInfo)
        {
            return tagInfo.ID ??
                this.getIDByTagCode(tagInfo.TagCode, tagInfo.TagClassID) ??
                this.getIDByTagName(tagInfo.TagName, tagInfo.TagClassID);
        }


        /// <summary>
        /// ���ݱ�ǩ�����ñ�ǩ��
        /// </summary>
        /// <param name="TagClassCode"></param>
        /// <returns></returns>
        internal DataTable GetTagByTagClassCode(string TagClassCode)
        {
            return this.UnitOfWork.ToDataTable(DBBuilder.Define(SYSTagSql.GetTagByTagClassCode, new
            {
                ClassCode = TagClassCode,
                SystemID = systemID,
            }));
        }

        /// <summary>
        /// ���ݱ�ǩ�����ñ�ǩ��
        /// </summary>
        /// <param name="TagClassCode"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        internal DataTable GetTagByTagClassCode(string TagClassCode, string tagName)
        {
            return this.UnitOfWork.ToDataTable(DBBuilder.Define(SYSTagSql.GetTagByTagClassCode_TagName, new
                {
                    ClassCode = TagClassCode,
                    tagName = tagName,
                    SystemID = systemID,
                }));
        }
        /// <summary>
        /// ���ݱ�ǩ�����ñ�ǩ��
        /// </summary>
        /// <param name="TagClassCode"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        internal DataTable GetTagByTagClassCode(string TagClassCode, string tagCode, string tagValue)
        {
            if (!string.IsNullOrEmpty(tagCode))
            {
                return this.UnitOfWork.ToDataTable(DBBuilder.Define(SYSTagSql.GetTagByTagClassCode_TagCode, new
                {
                    ClassCode = TagClassCode,
                    tagCode = tagCode,
                    SystemID = systemID,
                }));
            }
            else if (!string.IsNullOrEmpty(tagValue))
            {
                return this.UnitOfWork.ToDataTable(DBBuilder.Define(SYSTagSql.GetTagByTagClassCode_TagValue, new
                {
                    ClassCode = TagClassCode,
                    tagValue = tagValue,
                    SystemID = systemID,
                }));
            }
            else
                return null;
        }
        /// <summary>
        /// ȡ��Tag���е�һ������
        /// </summary>
        /// <param name="classCodePrefix">Ŀ¼ǰ׺</param>
        /// <returns></returns>
        internal DataTable GetTagClass(string classCodePrefix, long? displayLevel)
        {
            return this.UnitOfWork.ToDataTable(DBBuilder.Define(SYSTagSql.TagByCodePrefix, new
                {
                    ClassCode = classCodePrefix ?? string.Empty,
                    DisplayLevel = displayLevel,
                    SystemID = systemID,
                }));
        }

        /// <summary>
        /// ���ݸ��ڵ���ҡ���һ�����ӽڵ�
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        internal DataTable GetTagsByParentID(long parentID)
        {
            if (this.systemID==null)
                return this.UnitOfWork.ToDataTable(DBBuilder.Select("Tag").Where(new { ParentID = parentID }));

            return this.UnitOfWork.ToDataTable(DBBuilder.Select("Tag").Where(new { ParentID = parentID }).And().Eq(new { SystemID = this.systemID }));
        }
        /// <summary>
        /// ���ݸ��ڵ���ҡ���һ�����ӽڵ�
        /// </summary>
        /// <param name="TagClassCode"></param>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        internal DataTable GetTagsByParentID(string TagClassCode, long ParentID)
        {
            return this.UnitOfWork.ToDataTable(DBBuilder.Define(SYSTagSql.GetTagsByParentID, new
                {
                    ClassCode = TagClassCode,
                    ParentID = ParentID,
                    SystemID = systemID,
                }));
        }
        /// <summary>
        /// ���ݸ��ڵ���ҡ���һ�����ӽڵ�
        /// </summary>
        /// <param name="TagClassCode"></param>
        /// <param name="ParentName"></param>
        /// <returns></returns>
        internal DataTable GetTagsByParentID(string TagClassCode, string ParentName)
        {
            return this.UnitOfWork.ToDataTable(DBBuilder.Define(SYSTagSql.GetTagsByParentName, new
                {
                    ClassCode = TagClassCode,
                    ParentName = ParentName,
                    SystemID = systemID,
                }));
        }

        /// <summary>
        /// ����ָ����ǩ�������
        /// </summary>
        /// <param name="tagClassID"></param>
        private void UpdateChildTagByParant(long? tagClassID)
        {
            this.UnitOfWork.Execute(DBBuilder.Define(SYSTagSql.UpdateChildTagByParant, new
                {
                    TagClassID = tagClassID,
                    SystemID = systemID,
                }));
        }
        /// <summary>
        /// ����ָ����ǩ�������
        /// </summary>
        /// <param name="idAndSequence"></param>
        public void UpdateSequence(Dictionary<long, int> idAndSequence)
        {
            foreach (var kv in idAndSequence)
                this.Modify(new SYSTag { ID = kv.Key, Sequence = kv.Value });
            this.UpdateChildTagByParant(null);
        }

    }
}