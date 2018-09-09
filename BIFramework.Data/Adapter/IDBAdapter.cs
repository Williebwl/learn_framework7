using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Data
{
    /// <summary>
    /// 数据适配器，仅供开发人员使用
    /// </summary>
    public interface IDBAdapter
    {
        DbConnection CreateConnection();
        DbConnection CreateConnection(string connectionString);
        DbCommand CreateCommand();
        DbCommand CreateCommand(string commandText);
        DbParameter CreateParameter();
        DbParameter CreateParameter(string parmeterName, object parmValue);
        DbDataAdapter CreateDataAdapter();
        DbDataAdapter CreateDataAdapter(DbCommand cmd);
        string FormatTable(string tableName);
        string FormatField(string fieldName);
        string FormatParameter(string parameterName);
        string FormatValue(object objValue);
        string FormatFunction(string functionName);
        string FormatFunction(string functionName, params string[] parms);
    }
}
