using BIStudio.Framework;
using BIStudio.Framework.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Data.MySql
{
    public class DBAdapter : IDBAdapter
    {
        #region 数据库驱动

        #region 创建数据库对象

        /// <summary>
        /// 创建一个连接
        /// </summary>
        /// <returns></returns>
        public DbConnection CreateConnection()
        {
            return new MySqlConnection();
        }
        /// <summary>
        /// 使用指定的连接字符串创建连接
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public DbConnection CreateConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }
        /// <summary>
        /// 创建一个命令
        /// </summary>
        /// <returns></returns>
        public DbCommand CreateCommand()
        {
            return new MySqlCommand();
        }
        /// <summary>
        /// 使用指定的文本创建命令
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DbCommand CreateCommand(string commandText)
        {
            return new MySqlCommand(commandText);
        }
        /// <summary>
        /// 创建一个参数
        /// </summary>
        /// <returns></returns>
        public DbParameter CreateParameter()
        {
            return new MySqlParameter();
        }
        /// <summary>
        /// 使用指定的键值对创建参数
        /// </summary>
        /// <param name="parmeterName">参数名称,无需@或?</param>
        /// <param name="parmValue">参数值,如果为NULL则自动转换为DBNull.Value</param>
        /// <returns></returns>
        public DbParameter CreateParameter(string parmeterName, object parmValue)
        {
            return new MySqlParameter("?" + parmeterName.TrimStart('?'), parmValue ?? DBNull.Value);
        }
        /// <summary>
        /// 创建一个数据适配器
        /// </summary>
        /// <returns></returns>
        public DbDataAdapter CreateDataAdapter()
        {
            return new MySqlDataAdapter();
        }
        /// <summary>
        /// 使用指定的命令创建数据适配器
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public DbDataAdapter CreateDataAdapter(DbCommand cmd)
        {
            return new MySqlDataAdapter(cmd as MySqlCommand);
        }
        #endregion

        #region 格式化语法
        /// <summary>
        /// 格式化字段名
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string FormatTable(string tableName)
        {
            return "`" + tableName + "`";
        }
        /// <summary>
        /// 格式化字段名
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public string FormatField(string fieldName)
        {
            return "`" + fieldName + "`";
        }
        /// <summary>
        /// 格式化参数名
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public string FormatParameter(string parameterName)
        {
            return "?" + parameterName;
        }
        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="objValue"></param>
        /// <returns></returns>
        public string FormatValue(object objValue)
        {
            if (objValue == null || objValue == DBNull.Value)
                return "null";
            else if (objValue is bool)
                return (bool)objValue ? "1" : "0";
            else if (objValue is string)
                if ((string)objValue == string.Empty)
                    return "''";
                else
                    return "cast(0x" + BitConverter.ToString(Encoding.UTF8.GetBytes((string)objValue)).Replace("-", "") + " as nchar)";
            else if (objValue is int || objValue is decimal || objValue is double || objValue is long || objValue is short)
                return string.Format("{0}", objValue);
            else if (objValue is byte[])
                if (((byte[])objValue).Count() == 0)
                    return "''";
                else
                    return "0x" + BitConverter.ToString((byte[])objValue).Replace("-", "");
            else
                return string.Format("'{0}'", objValue);
        }
        /// <summary>
        /// 自定义函数
        /// </summary>
        private static Dictionary<string, string> sysFunctionMapper = new Dictionary<string, string>
        {
            {"getdate", "now()"},
            {"getyear", "year({0})"},
            {"getmonth", "month({0})"},
            {"getday", "day({0})"},
            {"datediff", "timestampdiff({0},{1},{2})"},
            
            {"charindex", "instr({1},{0})"},
            {"rowcount", "row_count()"},
            {"isnull", "ifnull({0},{1})"},
            
            {"gettables", "(select table_name from information_schema.tables where table_schema=database()) usertables"},
            {"getcolumns", "(select column_name FROM information_schema.columns where table_schema=database() and table_name={0}) usercolumns"},
            {"getmaxid", "call GetTablePKID({0},@id);select @id;"},
        };
        /// <summary>
        /// 格式化自定义函数
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public string FormatFunction(string functionName)
        {
            return sysFunctionMapper.ContainsKey(functionName) ? sysFunctionMapper[functionName] : (functionName + "()");
        }
        /// <summary>
        /// 格式化自定义函数
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public string FormatFunction(string functionName, params string[] parms)
        {
            if (parms == null || parms.Length == 0)
                return this.FormatFunction(functionName);

            //格式化函数文本
            int index = 0;
            string fnStr = sysFunctionMapper.ContainsKey(functionName) ? sysFunctionMapper[functionName] : (functionName + "(" + string.Join(",", parms.Select(d => "{" + index++ + "}").ToArray()) + ")");

            return string.Format(fnStr, parms.ToArray());
        }
        #endregion

        #endregion
    }
}
