using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIStudio.Framework.Tag.Internal;
using BIStudio.Framework.Utils;
using BIStudio.Framework.Domain;

namespace BIStudio.Framework.Tag
{
    internal class SYSTagGroupBO : SYSTagBase<SYSTagGroup>
    {
        /// <summary>
        /// 根据TagGroup获得ID
        /// </summary>
        /// <param name="tagGroupInfo">需指定ID或GroupCode或GroupName属性</param>
        /// <returns>标签ID或NULL</returns>
        internal long? GetIDByTagGroup(SYSTagGroup tagGroupInfo)
        {
            return tagGroupInfo.ID ??
                this.getIDByTagGroupCode(tagGroupInfo.GroupCode) ??
                this.getIDByTagGroupName(tagGroupInfo.GroupName);
        }

        /// <summary>
        /// 根据TagGroup名称获得ID
        /// </summary>
        /// <param name="tagGroupName"></param>
        /// <returns></returns>
        private long? getIDByTagGroupName(string tagGroupName)
        {
            if (string.IsNullOrEmpty(tagGroupName))
                return null;

            SYSTagGroup info = new SYSTagGroup { GroupName = tagGroupName };
            info.Property.ColumnAttributes["GroupName"].IsExact = true;
            info = this.Get(info.AsSpec());
            return info != null ? (info.ID) : null;
        }
        /// <summary>
        /// 根据TagGroup编号获得ID
        /// </summary>
        /// <param name="tagGroupCode"></param>
        /// <returns></returns>
        private long? getIDByTagGroupCode(string tagGroupCode)
        {
            if (string.IsNullOrEmpty(tagGroupCode))
                return null;

            SYSTagGroup info = new SYSTagGroup { GroupCode = tagGroupCode };
            info.Property.ColumnAttributes["GroupCode"].IsExact = true;
            info = this.Get(info.AsSpec());
            return info != null ? (info.ID) : null;
        }

    }
}
