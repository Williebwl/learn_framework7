using BIStudio.Framework;
using BIStudio.Framework.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BIStudio.Framework.Data
{

    /// <summary>
    /// 表示一条SQL命令
    /// </summary>
    public sealed class DBBuilder
    {
        #region 内部属性

        private StringBuilder sqlBuilder;
        /// <summary>
        /// 语法分析
        /// </summary>
        private bool smartAnalyzing;
        /// <summary>
        /// Sql语句
        /// </summary>
        public string Sql
        {
            get
            {
                return this.sqlBuilder.ToString();
            }
            set
            {
                this.sqlBuilder = new StringBuilder(value);
            }
        }
        /// <summary>
        /// Sql参数
        /// </summary>
        public DBParameterList Parameters { get; set; }
        /// <summary>
        /// 执行方式
        /// </summary>
        public CommandType? CommandType { get; set; }
        /// <summary>
        /// 连接
        /// </summary>
        public IDbTransaction Transaction { get; set; }

        /// <summary>
        /// 使用语句和参数初始化SQL命令
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="param">查询参数，例如：new {UserID = 1, UserName = "张三"}</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="fragmentArguments">片段参数</param>
        /// <param name="smartAnalyzing">语法分析，禁用此选项规约系统可能无法正常工作</param>
        private DBBuilder(string sql = null, dynamic param = null, CommandType? commandType = null, Dictionary<string, Func<IDBAdapter, string>> fragmentArguments = null, bool smartAnalyzing = true)
        {
            this.sqlBuilder = (sql == null ? new StringBuilder() : new StringBuilder(sql));
            this.Parameters = (param == null ? new DBParameterList() : new DBParameterList(param));
            this.CommandType = commandType;
            this.fragmentArguments = (fragmentArguments == null ? new Dictionary<string, Func<IDBAdapter, string>>() : fragmentArguments);
            this.smartAnalyzing = smartAnalyzing;
        }
        /// <summary>
        /// 使用语句和参数初始化SQL命令
        /// </summary>
        /// <param name="dbBuilder"></param>
        /// <param name="sql">查询语句</param>
        /// <param name="param">查询参数，例如：new {UserID = 1, UserName = "张三"}</param>
        /// <param name="fragmentArguments">片段参数</param>
        private DBBuilder(DBBuilder dbBuilder, string sql = null, dynamic param = null, Dictionary<string, Func<IDBAdapter, string>> fragmentArguments = null)
        {
            this.sqlBuilder = new StringBuilder(dbBuilder.sqlBuilder.ToString());
            if (sql != null)
                this.sqlBuilder.Append(sql);

            this.Parameters = new DBParameterList(dbBuilder.Parameters);
            if (param != null)
                this.Parameters.AddExpandoParams(param);

            this.fragmentArguments = new Dictionary<string, Func<IDBAdapter, string>>();
            if (fragmentArguments != null)
                fragmentArguments.ForEach(kv => this.Replace(kv.Key, kv.Value));

            this.CommandType = dbBuilder.CommandType;
            this.Transaction = dbBuilder.Transaction;
        }

        public DBBuilder SetTransaction(IDbTransaction transaction)
        {
            if (transaction != null && transaction.Connection != null && transaction.Connection.State == ConnectionState.Open)
                this.Transaction = transaction;
            return this;
        }
        private static Dictionary<string, string[]> sysTableFields = new Dictionary<string, string[]>();
        /// <summary>
        /// 获得指定数据表的全部字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private string[] GetTableFields(string tableName)
        {
            if (!sysTableFields.ContainsKey(tableName))
            {
                //DataTable dt = CFAspect.Resolve<IDBQuery>().ToDataTable(new DBBuilder("SELECT * FROM " + DBAdapter.FormatFunction("getcolumns", DBAdapter.FormatValue(tableName))));
                DataTable dt = CFAspect.Resolve<IDBQuery>().ToDataTable(new DBBuilder("SELECT * FROM ").Append(dbAdapter => dbAdapter.FormatFunction("getcolumns", dbAdapter.FormatValue(tableName))));
                sysTableFields[tableName] = dt.AsEnumerable().Select(d => d[0].ToString()).ToArray();
            }
            return sysTableFields[tableName];
        }
        #endregion

        #region 初始化

        /// <summary>
        /// 创建SQL片段
        /// </summary>
        /// <param name="dbBuilder"></param>
        /// <returns></returns>
        public static DBBuilder Define(DBBuilder dbBuilder)
        {
            return new DBBuilder(dbBuilder.sqlBuilder.ToString(), dbBuilder.Parameters, dbBuilder.CommandType, dbBuilder.fragmentArguments, dbBuilder.smartAnalyzing).SetTransaction(dbBuilder.Transaction);
        }
        /// <summary>
        /// 创建SQL片段
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="param">查询参数，例如：new {UserID = 1, UserName = "张三"}</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="fragmentArguments">片段参数</param>
        /// <param name="smartAnalyzing">语法分析，禁用此选项规约系统可能无法正常工作</param>
        /// <returns></returns>
        public static DBBuilder Define(string sql = null, dynamic param = null, CommandType? commandType = null, Dictionary<string, Func<IDBAdapter, string>> fragmentArguments = null, bool smartAnalyzing = true)
        {
            return new DBBuilder(sql, param, commandType, fragmentArguments, smartAnalyzing);
        }
        /// <summary>
        /// 创建以select FieldName1,FieldName2 from TableName开头的SQL语句
        /// </summary>
        /// <param name="tablename">要查询的表名</param>
        /// <param name="whereParam">要查询的字段名</param>
        /// <returns></returns>
        public static DBBuilder Select(string tablename = null, object whereParam = null)
        {
            var builder = new DBBuilder("SELECT ");
            if (tablename != null)
                builder.Append("*").From(tablename);
            if (whereParam != null)
                builder.Where(whereParam);
            return builder;
        }
        /// <summary>
        /// 创建以update开头语的SQL语句
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="setParam"></param>
        /// <param name="whereParam"></param>
        /// <returns></returns>
        public static DBBuilder Update(string tablename = null, dynamic setParam = null, dynamic whereParam = null)
        {
            var builder = new DBBuilder("UPDATE ");
            if (tablename != null)
                builder.Table(tablename);
            if (setParam != null)
                builder.Set(setParam);
            if (whereParam != null)
                builder.Where(whereParam);
            return builder;
        }
        /// <summary>
        /// 创建以delete from开头语的SQL语句
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="whereParam"></param>
        /// <returns></returns>
        public static DBBuilder Delete(string tablename = null, dynamic whereParam = null)
        {
            var builder = new DBBuilder("DELETE ");
            if (tablename != null)
                builder.From(tablename);
            if (whereParam != null)
                builder.Where(whereParam);
            return builder;
        }
        /// <summary>
        /// 创建以insert into开头语的SQL语句
        /// </summary>
        /// <returns></returns>
        public static DBBuilder Insert(string tablename = null, dynamic valueParam = null)
        {
            var builder = new DBBuilder("INSERT INTO ");
            if (tablename != null && valueParam != null)
            {
                var dp = new DBParameterList(valueParam);
                builder.Table(tablename, dp.ParameterNames.ToArray()).Values(dp);
            }
            else if (tablename != null)
            {
                builder.Table(tablename);
            }
            return builder;
        }

        /// <summary>
        /// 当前语句是否为查询语句
        /// </summary>
        public bool IsSelect
        {
            get
            {
                return !smartAnalyzing || this.sqlBuilder.ToString().TrimStart().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase);
            }
        }
        /// <summary>
        /// 当前语句是否为更新语句
        /// </summary>
        private bool IsUpdate
        {
            get
            {
                return !smartAnalyzing || this.sqlBuilder.ToString().TrimStart().StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase);
            }
        }
        /// <summary>
        /// 当前语句是否为删除语句
        /// </summary>
        private bool IsDelete
        {
            get
            {
                return !smartAnalyzing || this.sqlBuilder.ToString().TrimStart().StartsWith("DELETE", StringComparison.OrdinalIgnoreCase);
            }
        }
        /// <summary>
        /// 当前语句是否为插入语句
        /// </summary>
        private bool IsInsert
        {
            get
            {
                return !smartAnalyzing || this.sqlBuilder.ToString().TrimStart().StartsWith("INSERT", StringComparison.OrdinalIgnoreCase);
            }
        }
        /// <summary>
        /// 当前语句是否为命令语句
        /// </summary>
        public bool IsCommand
        {
            get
            {
                return this.IsInsert || this.IsDelete || this.IsUpdate;
            }
        }

        #endregion

        #region 基础操作

        /// <summary>
        /// 向现有查询语句末尾增加新条件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public DBBuilder Append(DBBuilder builder)
        {
            return Append(builder.sqlBuilder.ToString(), builder.Parameters, builder.fragmentArguments);
        }
        /// <summary>
        /// 向现有查询语句末尾增加新条件
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="fragmentArguments"></param>
        /// <returns></returns>
        public DBBuilder Append(string sql = null, dynamic param = null, Dictionary<string, Func<IDBAdapter, string>> fragmentArguments = null)
        {
            if (sql != null)
                this.sqlBuilder.Append(sql);
            if (param != null)
                this.Parameters.AddExpandoParams(param);
            if (fragmentArguments != null)
                fragmentArguments.ForEach(kv => this.Replace(kv.Key, kv.Value));
            return this;
        }
        /// <summary>
        /// 表示 Prefix (dbBuilder)
        /// </summary>
        /// <param name="dbBuilder"></param>
        /// <param name="prefix"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public DBBuilder Clause(DBBuilder dbBuilder, string prefix = null, bool predicate = true)
        {
            return Clause(d => d.Append(dbBuilder), prefix, predicate);
        }
        /// <summary>
        /// 表示 Prefix (dbBuilder)
        /// </summary>
        /// <param name="dbBuilder"></param>
        /// <param name="prefix"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public DBBuilder Clause(Action<DBBuilder> dbBuilder = null, string prefix = null, bool predicate = true)
        {
            if (!predicate)
                return this;
            if (prefix != null)
                this.Append(prefix);
            if (dbBuilder != null)
            {
                this.Append("(");
                dbBuilder(this);
                this.Append(")");
            }
            return this;
        }
        /// <summary>
        /// 表示[TableName]([FieldName1],[FieldName2])
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldNames"></param>
        /// <returns></returns>
        public DBBuilder Table(string tableName = null, params string[] fieldNames)
        {
            if (tableName != null)
                this.Append(dbAdapter => dbAdapter.FormatTable(tableName));
            if (fieldNames != null && fieldNames.Length > 0)
                this.Clause(d => d.Field(tableName, fieldNames, false));
            return this;
        }
        /// <summary>
        /// 表示TableName.FieldName1,TableName.FieldName2
        /// </summary>
        /// <param name="tableName">数据表</param>
        /// <param name="fieldNames">要查询的字段，*列出所有字段，-排除指定字段</param>
        /// <returns></returns>
        public DBBuilder Field(string tableName = null, params string[] fieldNames)
        {
            if (fieldNames != null && fieldNames.Length > 0)
                Field(tableName, fieldNames, true);
            else if (tableName != null)
                Append(dbAdapter => dbAdapter.FormatTable(tableName));
            return this;
        }

        private static Func<char, bool> IsSqlFunc = (item =>
        {
            return new char[] { ' ', '(', ')' }.Contains(item);
        });
        /// <summary>
        /// 表示TableName.FieldName1,TableName.FieldName2
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldNames"></param>
        /// <param name="outputTableName"></param>
        /// <returns></returns>
        protected DBBuilder Field(string tableName, string[] fieldNames, bool outputTableName)
        {
            if (tableName != null)
            {
                //列出所有字段
                if (fieldNames.Length == 1 && fieldNames[0] == "*")
                    fieldNames = GetTableFields(tableName);
                //列出排除字段
                if (fieldNames.All(d => d.StartsWith("-")))
                    fieldNames = GetTableFields(tableName).Except(fieldNames.Select(d => d.TrimStart('-'))).ToArray();
            }
            //列出选择的字段
            this.Append(dbAdapter => string.Join(",", fieldNames.Select(fieldName =>
            {
                if (fieldName.Any(IsSqlFunc))
                    return fieldName;
                else if (fieldName.IndexOf('.') > 0)
                    return string.Join(".", fieldName.Split('.').Select(d => dbAdapter.FormatField(d)));
                else
                    return ((tableName != null && outputTableName) ? dbAdapter.FormatTable(tableName) + "." : "") + dbAdapter.FormatField(fieldName);
            })));
            return this;
        }
        /// <summary>
        /// 表示@ParamName1,@ParamName2
        /// </summary>
        /// <param name="paramNames"></param>
        /// <returns></returns>
        public DBBuilder Param(params string[] paramNames)
        {
            if (paramNames != null && paramNames.Length > 0)
                this.Append(dbAdapter => string.Join(",", paramNames.Select(d => dbAdapter.FormatParameter(d))));
            return this;
        }
        /// <summary>
        /// 表示@ParamName1,@ParamName2和参数值
        /// </summary>
        /// <param name="param">Append a whole object full of params to the dynamic EG: (new {A = 1, B = 2}) will add property A and B to the dynamic</param>
        /// <returns></returns>
        public DBBuilder Param(dynamic param)
        {
            if (param != null)
            {
                var dp = new DBParameterList(param);
                Param(dp.ParameterNames.ToArray());
                this.Append(null, dp);
            }
            return this;
        }
        /// <summary>
        /// 表示Value1,Value2
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public DBBuilder Value<T>(IEnumerable<T> values)
        {
            if (values is string)
                this.Append(dbAdapter => dbAdapter.FormatValue(values));
            else if (values != null && values.Count() > 0)
                this.Append(dbAdapter => string.Join(",", values.Select(d => dbAdapter.FormatValue(d))));
            return this;
        }
        /// <summary>
        /// 表示Value1,Value2
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public DBBuilder Value(params object[] values)
        {
            if (values != null && values.Length > 0)
                this.Append(dbAdapter => string.Join(",", values.Select(d => dbAdapter.FormatValue(d))));
            return this;
        }

        #endregion

        #region 片段操作

        private Dictionary<string, Func<IDBAdapter, string>> fragmentArguments = new Dictionary<string, Func<IDBAdapter, string>>();

        /// <summary>
        /// 在当前位置插入一个片段
        /// </summary>
        /// <param name="fragmentArgument">片段内容</param>
        /// <returns></returns>
        public DBBuilder Append(Func<IDBAdapter, string> fragmentArgument)
        {
            return Append(ALUtils.GetGUIDShort(), fragmentArgument);
        }
        /// <summary>
        /// 在当前位置插入一个片段
        /// </summary>
        /// <param name="fragmentKey">片段名称</param>
        /// <param name="fragmentArgument">片段内容</param>
        /// <returns></returns>
        public DBBuilder Append(string fragmentKey, Func<IDBAdapter, string> fragmentArgument)
        {
            this.Append("/**" + fragmentKey + "**/");
            this.fragmentArguments.Add(fragmentKey, fragmentArgument);
            return this;
        }
        /// <summary>
        /// 更新已有的片段
        /// </summary>
        /// <param name="fragmentKey">片段名称</param>
        /// <param name="fragmentArgument">片段内容</param>
        /// <returns></returns>
        public DBBuilder Replace(string fragmentKey, Func<IDBAdapter, string> fragmentArgument)
        {
            if (this.fragmentArguments.ContainsKey(fragmentKey))
                this.fragmentArguments[fragmentKey] = fragmentArgument;
            else
                this.fragmentArguments.Add(fragmentKey, fragmentArgument);
            return this;
        }
        /// <summary>
        /// 删除已有片段
        /// </summary>
        /// <param name="fragmentKey">片段名称</param>
        /// <returns></returns>
        public DBBuilder Remove(string fragmentKey)
        {
            var index = this.sqlBuilder.ToString().IndexOf("/**" + fragmentKey + "**/");
            if (index >= 0)
                this.sqlBuilder.Remove(index, fragmentKey.Length);
            if (this.fragmentArguments.ContainsKey(fragmentKey))
                this.fragmentArguments.Remove(fragmentKey);
            return this;
        }
        /// <summary>
        /// 查询已有片段
        /// </summary>
        /// <param name="fragmentKey">片段名称</param>
        /// <returns></returns>
        public bool Exists(string fragmentKey)
        {
            return this.sqlBuilder.ToString().IndexOf(fragmentKey) >= 0;
        }
        /// <summary>
        /// 执行所有片段
        /// </summary>
        /// <returns></returns>
        public DBBuilder Compile(IDBAdapter dbAdapter)
        {
            this.fragmentArguments.ForEach(kv => this.sqlBuilder.Replace("/**" + kv.Key + "**/", kv.Value(dbAdapter)));
            this.fragmentArguments.Clear();
            return this;
        }

        #endregion

        #region 关键字
        /// <summary>
        /// 表示where 1=1
        /// </summary>
        /// <returns></returns>
        public DBBuilder Where(bool predicate)
        {
            return this.Append("\nWHERE 1=" + (predicate ? 1 : 0));
        }
        /// <summary>
        /// 表示where
        /// </summary>
        /// <returns></returns>
        public DBBuilder Where(DBBuilder builder)
        {
            if (builder == null || builder.sqlBuilder.Length == 0)
                return this;
            return this.Append("\nWHERE ").Append(builder);
        }
        /// <summary>
        /// 表示where
        /// </summary>
        /// <returns></returns>
        public DBBuilder Where(dynamic param = null)
        {
            this.Append("\rWHERE ");
            if (param != null)
                this.Eq(param, "AND", false);
            return this;
        }
        /// <summary>
        /// 表示where
        /// </summary>
        /// <returns></returns>
        public DBBuilder Where(string tableName, dynamic param = null)
        {
            this.Append("\nWHERE ");
            if (param != null)
                this.Eq(tableName, param, useDirectOutput: false);
            return this;
        }
        /// <summary>
        /// 表示TableName.FieldName1,TableName.FieldName2 from
        /// </summary>
        /// <returns></returns>
        public DBBuilder From(string tableName = null, params string[] fieldNames)
        {
            if (fieldNames != null && fieldNames.Length > 0)
                this.Field(tableName, fieldNames);
            if (tableName != null)
                return this.Append(dbAdapter => "\nFROM " + dbAdapter.FormatTable(tableName) + " ");
            else
                return this.Append("\nFROM ");
        }
        /// <summary>
        /// 表示order by
        /// </summary>
        /// <returns></returns>
        public DBBuilder OrderBy()
        {
            return this.Append("\nORDER BY ");
        }
        /// <summary>
        /// 表示set
        /// </summary>
        /// <returns></returns>
        public DBBuilder Set(dynamic param = null)
        {
            this.Append(" SET ");
            if (param != null)
                this.Eq(param, ",");
            return this;
        }
        /// <summary>
        /// 表示as
        /// </summary>
        /// <returns></returns>
        public DBBuilder As()
        {
            return this.Append(" AS ");
        }
        /// <summary>
        /// 表示values
        /// </summary>
        /// <returns></returns>
        public DBBuilder Values(dynamic param = null)
        {
            this.Append(" VALUES ");
            if (param != null)
                this.Append("(").Param(param).Append(")");
            return this;
        }
        /// <summary>
        /// 表示top [N] percent
        /// </summary>
        /// <param name="topN"></param>
        /// <param name="usePercentValue"></param>
        /// <returns></returns>
        public DBBuilder Top(int topN, bool usePercentValue = false)
        {
            if (usePercentValue)
                return this.Append(" TOP " + topN + " PERCENT ");
            else
                return this.Append(" TOP " + topN + " ");
        }
        /// <summary>
        /// 表示count(*)
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DBBuilder Count(string sql = "*")
        {
            return this.Append(" COUNT(" + sql + ") ");
        }
        /// <summary>
        /// 表示distinct
        /// </summary>
        /// <returns></returns>
        public DBBuilder Distinct()
        {
            return this.Append(" DISTINCT ");
        }
        #endregion

        #region 连接查询

        /// <summary>
        /// 表示内连接
        /// </summary>
        /// <param name="srcTableName"></param>
        /// <param name="srcFieldName"></param>
        /// <param name="targetTableName"></param>
        /// <param name="targetFieldName"></param>
        /// <returns></returns>
        public DBBuilder InnerJoin(string srcTableName, string srcFieldName, string targetTableName, string targetFieldName)
        {
            return this.Append("\n INNER JOIN ").Table(targetTableName).Append(" on ").Field(srcTableName, srcFieldName).Append("=").Field(targetTableName, targetFieldName);
        }
        /// <summary>
        /// 表示左连接
        /// </summary>
        /// <param name="srcTableName"></param>
        /// <param name="srcFieldName"></param>
        /// <param name="targetTableName"></param>
        /// <param name="targetFieldName"></param>
        /// <returns></returns>
        public DBBuilder LeftJoin(string srcTableName, string srcFieldName, string targetTableName, string targetFieldName)
        {
            return this.Append("\n LEFT JOIN ").Table(targetTableName).Append(" on ").Field(srcTableName, srcFieldName).Append("=").Field(targetTableName, targetFieldName);
        }
        /// <summary>
        /// 表示右连接
        /// </summary>
        /// <param name="srcTableName"></param>
        /// <param name="srcFieldName"></param>
        /// <param name="targetTableName"></param>
        /// <param name="targetFieldName"></param>
        /// <returns></returns>
        public DBBuilder RightJoin(string srcTableName, string srcFieldName, string targetTableName, string targetFieldName)
        {
            return this.Append("\n RIGHT JOIN ").Table(targetTableName).Append(" on ").Field(srcTableName, srcFieldName).Append("=").Field(targetTableName, targetFieldName);
        }
        #endregion

        #region 逻辑判断

        public DBBuilder And(DBBuilder dbBuilder, bool predicate = true)
        {
            return Clause(dbBuilder, "\n AND ", predicate);
        }
        public DBBuilder And(Action<DBBuilder> dbBuilder = null, bool predicate = true)
        {
            return Clause(dbBuilder, "\n AND ", predicate);
        }
        public DBBuilder Or(DBBuilder dbBuilder, bool predicate = true)
        {
            return Clause(dbBuilder, "\n OR ", predicate);
        }
        public DBBuilder Or(Action<DBBuilder> dbBuilder = null, bool predicate = true)
        {
            return Clause(dbBuilder, "\n OR ", predicate);
        }
        public DBBuilder Not(DBBuilder dbBuilder, bool predicate = true)
        {
            return Clause(dbBuilder, " NOT ");
        }
        public DBBuilder Not(Action<DBBuilder> dbBuilder = null, bool predicate = true)
        {
            return Clause(dbBuilder, " NOT ");
        }
        public DBBuilder In(DBBuilder dbBuilder, bool predicate = true)
        {
            return Clause(dbBuilder, " IN ", predicate);
        }
        public DBBuilder In(Action<DBBuilder> dbBuilder = null, bool predicate = true)
        {
            return Clause(dbBuilder, " IN ", predicate);
        }
        public DBBuilder NotIn(DBBuilder dbBuilder, bool predicate = true)
        {
            return Clause(dbBuilder, " NOT IN ", predicate);
        }
        public DBBuilder NotIn(Action<DBBuilder> dbBuilder = null, bool predicate = true)
        {
            return Clause(dbBuilder, " NOT IN ", predicate);
        }
        public DBBuilder Exists(DBBuilder dbBuilder, bool predicate = true)
        {
            return Clause(dbBuilder, " EXISTS ", predicate);
        }
        public DBBuilder Exists(Action<DBBuilder> dbBuilder = null, bool predicate = true)
        {
            return Clause(dbBuilder, " EXISTS ", predicate);
        }
        public DBBuilder NotExists(DBBuilder dbBuilder, bool predicate = true)
        {
            return Clause(dbBuilder, " NOT EXISTS ", predicate);
        }
        public DBBuilder NotExists(Action<DBBuilder> dbBuilder = null, bool predicate = true)
        {
            return Clause(dbBuilder, " NOT EXISTS ", predicate);
        }

        #endregion

        #region 关系判断

        /// <summary>
        /// 表示ParamName1=@ParamValue1 and ParamName2=@ParamValue2
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="param"></param>
        /// <param name="symbol"></param>
        /// <param name="joiner"></param>
        /// <param name="useDirectOutput"></param>
        /// <returns></returns>
        public DBBuilder Operate(string tableName = null, dynamic param = null, string symbol = "=", string joiner = "AND", bool useDirectOutput = true)
        {
            if (param != null)
            {
                var dp = new DBParameterList(param);
                if (useDirectOutput || tableName != null)
                    this.Append(dbAdapter => string.Join(" " + joiner + " ", dp.ParameterNames.Select(d =>
                        (tableName != null ? dbAdapter.FormatTable(tableName) + "." : "") + dbAdapter.FormatField(d) + " " + symbol + " " + dbAdapter.FormatValue(dp.Get<object>(d))
                        )));
                else
                    this.Append(dbAdapter => string.Join(" " + joiner + " ", dp.ParameterNames.Select(d =>
                        (tableName != null ? dbAdapter.FormatTable(tableName) + "." : "") + dbAdapter.FormatField(d) + " " + symbol + " " + dbAdapter.FormatParameter(d)
                        ))).Append(null, dp);
            }
            else
            {
                this.Append(symbol);
            }
            return this;
        }
        /// <summary>
        /// 等于
        /// </summary>
        /// <returns></returns>
        public DBBuilder Eq(dynamic param = null, string joiner = "AND", bool useDirectOutput = true)
        {
            return Operate(null, param, "=", joiner, useDirectOutput);
        }
        /// <summary>
        /// 等于
        /// </summary>
        /// <returns></returns>
        public DBBuilder Eq(string tableName, dynamic param = null, string joiner = "AND", bool useDirectOutput = true)
        {
            return Operate(tableName, param, "=", joiner, useDirectOutput);
        }
        /// <summary>
        /// 不等于
        /// </summary>
        /// <returns></returns>
        public DBBuilder Ne(dynamic param = null, string joiner = "AND", bool useDirectOutput = true)
        {
            return Operate(null, param, "<>", joiner, useDirectOutput);
        }
        /// <summary>
        /// 不等于
        /// </summary>
        /// <returns></returns>
        public DBBuilder Ne(string tableName, dynamic param = null, string joiner = "AND", bool useDirectOutput = true)
        {
            return Operate(tableName, param, "<>", joiner, useDirectOutput);
        }
        /// <summary>
        /// 大于
        /// </summary>
        /// <returns></returns>
        public DBBuilder Gt(dynamic param = null, string joiner = "AND", bool useDirectOutput = true)
        {
            return Operate(null, param, ">", joiner, useDirectOutput);
        }
        /// <summary>
        /// 大于
        /// </summary>
        /// <returns></returns>
        public DBBuilder Gt(string tableName, dynamic param = null, string joiner = "AND", bool useDirectOutput = true)
        {
            return Operate(tableName, param, ">", joiner, useDirectOutput);
        }
        /// <summary>
        /// 小于
        /// </summary>
        /// <returns></returns>
        public DBBuilder Lt(dynamic param = null, string joiner = "AND", bool useDirectOutput = true)
        {
            return Operate(null, param, "<", joiner, useDirectOutput);
        }
        /// <summary>
        /// 小于
        /// </summary>
        /// <returns></returns>
        public DBBuilder Lt(string tableName, dynamic param = null, string joiner = "AND", bool useDirectOutput = true)
        {
            return Operate(tableName, param, "<", joiner, useDirectOutput);
        }
        /// <summary>
        /// 大于等于
        /// </summary>
        /// <returns></returns>
        public DBBuilder Ge(dynamic param = null, string joiner = "AND", bool useDirectOutput = true)
        {
            return Operate(null, param, ">=", joiner, useDirectOutput);
        }
        /// <summary>
        /// 大于等于
        /// </summary>
        /// <returns></returns>
        public DBBuilder Ge(string tableName, dynamic param = null, string joiner = "AND", bool useDirectOutput = true)
        {
            return Operate(tableName, param, ">=", joiner, useDirectOutput);
        }
        /// <summary>
        /// 小于等于
        /// </summary>
        /// <returns></returns>
        public DBBuilder Le(dynamic param = null, string joiner = "AND", bool useDirectOutput = true)
        {
            return Operate(null, param, "<=", joiner, useDirectOutput);
        }
        /// <summary>
        /// 小于等于
        /// </summary>
        /// <returns></returns>
        public DBBuilder Le(string tableName, dynamic param = null, string joiner = "AND", bool useDirectOutput = true)
        {
            return Operate(tableName, param, "<=", joiner, useDirectOutput);
        }
        /// <summary>
        /// Like
        /// </summary>
        /// <returns></returns>
        public DBBuilder Like(dynamic param = null, string joiner = "AND", bool useDirectOutput = true)
        {
            return Operate(null, param, " LIKE ", joiner, useDirectOutput);
        }
        /// <summary>
        /// Like
        /// </summary>
        /// <returns></returns>
        public DBBuilder Like(string tableName, dynamic param = null, string joiner = "AND", bool useDirectOutput = true)
        {
            return Operate(tableName, param, " LIKE ", joiner, useDirectOutput);
        }

        #endregion

        #region 系统函数

        /// <summary>
        /// 获得当前时间
        /// </summary>
        /// <returns></returns>
        public DBBuilder GetDate()
        {
            return this.Append(dbAdapter => dbAdapter.FormatFunction("getdate"));
        }
        /// <summary>
        /// 获得指定表达式在字符串中的位置
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="strValue">搜索表达式</param>
        /// <returns></returns>
        public DBBuilder CharIndex(string fieldName, string strValue)
        {
            return this.Append(dbAdapter => dbAdapter.FormatFunction("charindex", dbAdapter.FormatValue(strValue), dbAdapter.FormatField(fieldName)));
        }
        /// <summary>
        /// 获得指定表达式在字符串中的位置
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="strValue">搜索表达式</param>
        /// <returns></returns>
        public DBBuilder CharIndex(string tableName, string fieldName, string strValue)
        {
            return this.Append(dbAdapter => dbAdapter.FormatFunction("charindex", dbAdapter.FormatValue(strValue), dbAdapter.FormatTable(tableName) + "." + dbAdapter.FormatField(fieldName)));
        }
        /// <summary>
        /// 判断指定的字段是否为空
        /// </summary>
        /// <returns></returns>
        public DBBuilder IsNull(string fieldName = null)
        {
            return this.Append(dbAdapter => dbAdapter.FormatField(fieldName) + " IS NULL");
        }
        /// <summary>
        /// 判断指定的字段是否为空
        /// </summary>
        /// <returns></returns>
        public DBBuilder IsNotNull(string fieldName = null)
        {
            return this.Append(dbAdapter => dbAdapter.FormatField(fieldName) + " IS NOT NULL");
        }
        /// <summary>
        /// 判断指定的字段是否为空
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="objValue">默认值</param>
        /// <returns></returns>
        public DBBuilder IsNull(string fieldName, object objValue)
        {
            return this.Append(dbAdapter => dbAdapter.FormatFunction("isnull", dbAdapter.FormatField(fieldName), dbAdapter.FormatValue(objValue)));
        }
        /// <summary>
        /// 判断指定的字段是否为空
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="objValue">默认值</param>
        /// <returns></returns>
        public DBBuilder IsNull(string tablename, string fieldName, object objValue)
        {
            return this.Append(dbAdapter => dbAdapter.FormatFunction("isnull", dbAdapter.FormatTable(tablename) + "." + dbAdapter.FormatField(fieldName), dbAdapter.FormatValue(objValue)));
        }

        #endregion

    }
}
