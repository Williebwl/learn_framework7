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
        /// 根据TagClass名称获得ID
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
        /// 根据TagClass编号获得ID
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
        /// 根据TagClass获得ID
        /// </summary>
        /// <param name="tagClassInfo">需指定ID或ClassCode或ClassName属性</param>
        /// <returns>标签ID或NULL</returns>
        public long? GetIDByTagClass(SYSTagClass tagClassInfo)
        {
            return tagClassInfo.ID ??
                this.getIDByTagClassCode(tagClassInfo.ClassCode) ??
                this.getIDByTagClassName(tagClassInfo.ClassName);
        }
        /// <summary>
        /// 得到全部标签
        /// </summary>
        /// <returns></returns>
        public DataTable GetTagClass()
        {
            return this.GetTagClass(null, null);
        }

        /// <summary>
        /// 得到指定标签组，指定标签类型的标签
        /// </summary>
        /// <param name="classCodePrefix">目录前缀</param>
        /// <param name="displayLevel">标签类型</param>
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
        /// 更新标签
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