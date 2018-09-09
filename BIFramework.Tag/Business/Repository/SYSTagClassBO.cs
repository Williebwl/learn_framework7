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

    public class SYSTagClassBO : SYSTagBase<SYSTagClass>
	{
        /// <summary>
        /// ����TagClass���ƻ��ID
        /// </summary>
        /// <param name="TagClassCode"></param>
        /// <returns></returns>
        private long? getIDByTagClassName(string tagClassName)
        {
            if (string.IsNullOrEmpty(tagClassName))
                return null;

            SYSTagClass info = new SYSTagClass { ClassName = tagClassName };
            info.Property.ColumnAttributes["ClassName"].IsExact = true;
            info = this.Get(info.AsSpec());
            return info != null ? (info.ID) : null;
        }
        /// <summary>
        /// ����TagClass��Ż��ID
        /// </summary>
        /// <param name="TagClassCode"></param>
        /// <returns></returns>
        private long? getIDByTagClassCode(string tagClassCode)
        {
            if (string.IsNullOrEmpty(tagClassCode))
                return null;

            SYSTagClass info = new SYSTagClass { ClassCode = tagClassCode };
            info.Property.ColumnAttributes["ClassCode"].IsExact = true;
            info = this.Get(info.AsSpec());
            return info != null ? (info.ID) : null;
        }
        /// <summary>
        /// ����TagClass���ID
        /// </summary>
        /// <param name="tagClassInfo">��ָ��ID��ClassCode��ClassName����</param>
        /// <returns>��ǩID��NULL</returns>
        public long? GetIDByTagClass(SYSTagClass tagClassInfo)
        {
            return tagClassInfo.ID ??
                this.getIDByTagClassCode(tagClassInfo.ClassCode) ??
                this.getIDByTagClassName(tagClassInfo.ClassName);
        }
        /// <summary>
        /// �õ�ȫ����ǩ
        /// </summary>
        /// <returns></returns>
        public DataTable GetTagClass()
        {
            return this.GetTagClass(null, null);
        }

        /// <summary>
        /// �õ�ָ����ǩ�飬ָ����ǩ���͵ı�ǩ
        /// </summary>
        /// <param name="classCodePrefix">Ŀ¼ǰ׺</param>
        /// <param name="displayLevel">��ǩ����</param>
        /// <returns></returns>
        public DataTable GetTagClass(string classCodePrefix, int? displayLevel)
        {
            return this.UnitOfWork.ToDataTable(DBBuilder.Define(SYSTagClassSql.TagClassByCodePrefix, new
            {
                GroupCode = classCodePrefix,
                DisplayLevel = displayLevel,
                SystemID = systemID,
            }));
        }

        /// <summary>
        /// ���±�ǩ
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Save(SYSTagClass info)
        {
            bool flag = (info.ID.HasValue ? (Func<SYSTagClass, bool>)this.Modify : this.Add)(info);
            if (flag)
            {
                this.UnitOfWork.Execute(DBBuilder.Define(SYSTagClassSql.SaveSql, new
                {
                    TagClass = info.ClassName,
                    TagClassID = info.ID,
                    SystemID = systemID,
                }));
            }
            return flag;
            
        }
        
    }
}