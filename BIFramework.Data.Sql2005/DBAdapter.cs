using BIStudio.Framework;

using BIStudio.Framework.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Data.Sql2005
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
            return new SqlConnection();
        }
        /// <summary>
        /// 使用指定的连接字符串创建连接
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public DbConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
        /// <summary>
        /// 创建一个命令
        /// </summary>
        /// <returns></returns>
        public DbCommand CreateCommand()
        {
            return new SqlCommand();
        }
        /// <summary>
        /// 使用指定的文本创建命令
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DbCommand CreateCommand(string commandText)
        {
            return new SqlCommand(commandText);
        }
        /// <summary>
        /// 创建一个参数
        /// </summary>
        /// <returns></returns>
        public DbParameter CreateParameter()
        {
            return new SqlParameter();
        }
        /// <summary>
        /// 使用指定的键值对创建参数
        /// </summary>
        /// <param name="parmeterName">参数名称,无需@或?</param>
        /// <param name="parmValue">参数值,如果为NULL则自动转换为DBNull.Value</param>
        /// <returns></returns>
        public DbParameter CreateParameter(string parmeterName, object parmValue)
        {
            return new SqlParameter("@" + parmeterName.TrimStart('@'), parmValue ?? DBNull.Value);
        }
        /// <summary>
        /// 创建一个数据适配器
        /// </summary>
        /// <returns></returns>
        public DbDataAdapter CreateDataAdapter()
        {
            return new SqlDataAdapter();
        }
        /// <summary>
        /// 使用指定的命令创建数据适配器
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public DbDataAdapter CreateDataAdapter(DbCommand cmd)
        {
            return new SqlDataAdapter(cmd as SqlCommand);
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
            return "[" + tableName + "]";
        }
        /// <summary>
        /// 格式化字段名
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public string FormatField(string fieldName)
        {
            return "[" + fieldName + "]";
        }
        /// <summary>
        /// 格式化参数名
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public string FormatParameter(string parameterName)
        {
            return "@" + parameterName;
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
                    return "cast(0x" + BitConverter.ToString(Encoding.Unicode.GetBytes((string)objValue)).Replace("-", "") + " as nvarchar(max))";
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
            {"getdate", "getdate()"},
            {"getyear", "datepart(yyyy,{0})"},
            {"getmonth", "datepart(mm,{0})"},
            {"getday", "datepart(dd,{0})"},
            {"datediff", "datediff({0},{1},{2})"},
            
            {"charindex", "charindex({0},{1})"},
            {"isnull", "isnull({0},{1})"},
            {"rowcount", "@@rowcount"},
            
            {"gettables", "(select top 100 percent [Name] as table_name from sysobjects where xtype='U' and name<>'dtproperties' order by [name]) usertables"},
            {"getcolumns", "(select top 100 percent [Name] as column_name from syscolumns where id=OBJECT_ID({0}) order by [colid]) usercolumns"},   
            {"getmaxid", "declare @id int;execute GetTablePKID {0},@id out;select @id;"},         
        };
        /// <summary>
        /// 格式化自定义函数
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public string FormatFunction(string functionName)
        {
            return sysFunctionMapper.ContainsKey(functionName) ? sysFunctionMapper[functionName] : ("dbo." + functionName + "()");
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
            string fnStr = sysFunctionMapper.ContainsKey(functionName) ? sysFunctionMapper[functionName] : ("dbo." + functionName + "(" + string.Join(",", parms.Select(d => "{" + index++ + "}").ToArray()) + ")");

            return string.Format(fnStr, parms.ToArray());
        }
        #endregion

        #endregion
    }
}
