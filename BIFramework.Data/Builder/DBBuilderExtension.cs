using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BIStudio.Framework.Data
{
    public static class DBBuilderExtension
    {
        /// <summary>
        /// 获得请求的命令
        /// </summary>
        /// <param name="dbBuilder"></param>
        /// <returns></returns>
        public static CommandDefinition GetCommand(this DBBuilder dbBuilder, IDBAdapter dbAdapter)
        {
            return new CommandDefinition(dbBuilder.Compile(dbAdapter).Sql, dbBuilder.Parameters.Instance, dbBuilder.Transaction, null, dbBuilder.CommandType);
        }
    }
}
