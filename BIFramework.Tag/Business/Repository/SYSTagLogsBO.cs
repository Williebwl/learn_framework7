using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BIStudio.Framework.Data;
using BIStudio.Framework.Tag.Internal;
using BIStudio.Framework.Domain;

namespace BIStudio.Framework.Tag
{
    public class SYSTagLogsBO : SYSTagBase<SYSTagLogs>
    {
        /// <summary>
        /// 删除指定标签的贴入日志
        /// </summary>
        /// <param name="tagID"></param>
        /// <returns></returns>
        internal int DeleteByTagID(long tagID)
        {
            return this.UnitOfWork.Execute(DBBuilder.Define(SYSTagLogsSql.DeleteByTagIDSql, new
                    {
                        TagID = tagID,
                        SystemID = systemID,
                    }));
        }

    }
}