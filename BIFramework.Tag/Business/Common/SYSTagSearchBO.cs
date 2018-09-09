using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SqlClient;
using BIStudio.Framework;
using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Utils;


namespace BIStudio.Framework.Tag
{
    /// <summary>
    /// 标签搜索器
    /// </summary>
    public class SYSTagSearchBO : TransientDependency, IEnumerable<DataRow>
    {
        private List<string> srcTable = new List<string>();
        private StringBuilder strField = new StringBuilder("*");
        private StringBuilder strTable = new StringBuilder();
        private StringBuilder strWhere = new StringBuilder();
        private StringBuilder strOrder = new StringBuilder();
        private StringBuilder strGroup = new StringBuilder();
        private DBParameterList lstWhere = new DBParameterList();

        /// <summary>
        /// 返回当前查询参数
        /// </summary>
        public DBParameterList Parameters
        {
            get
            {
                return this.lstWhere;
            }
        }

        #region 初始化标准版

        /// <summary>
        /// 设置查询主表
        /// </summary>
        /// <param name="tableName">数据表名</param>
        internal SYSTagSearchBO(string tableName)
            : this(tableName, null)
        {
        }
        /// <summary>
        /// 设置查询主表，并筛选出已贴入指定标签项的数据
        /// </summary>
        /// <param name="tableName">数据表名</param>
        /// <param name="tagIDs">标签项ID列表</param>
        internal SYSTagSearchBO(string tableName, params long[] tagIDs)
            : this(tableName, EnumSYSTagSearch.Fuzzy, tagIDs)
        {
        }
        /// <summary>
        /// 设置查询主表，并筛选出已贴入指定标签项的数据
        /// </summary>
        /// <param name="tableName">数据表名</param>
        /// <param name="matchType">匹配方式 EnumTagSearch</param>
        /// <param name="tagIDs">标签项ID列表</param>
        internal SYSTagSearchBO(string tableName, EnumSYSTagSearch matchType, params long[] tagIDs)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentOutOfRangeException("tableName");
            srcTable.Add(tableName);
            strTable.AppendFormat(SYSTagSearchSql.TableNameSql, tableName, srcTable.Count);

            if (tagIDs != null && tagIDs.Length > 0)
            {
                switch (matchType)
                {
                    case EnumSYSTagSearch.Fuzzy:
                        //使用OR
                        strWhere.AppendFormat(SYSTagSearchSql.InSql,
                            srcTable.Count, this.Context.Resolve<SYSTagApplyBO>().GetTargetObjectIDByTargetIDAndTagIDs(this.Context.Resolve<SYSTagTargetBO>()[tableName].ID.Value, tagIDs.ToArray()));
                        break;
                    case EnumSYSTagSearch.Smart:
                        //使用AND+OR
                        int tagClassCount = 0;
                        strTable.AppendFormat(SYSTagSearchSql.InnerJoinSql,
                            srcTable.Count, this.Context.Resolve<SYSTagApplyBO>().GetTargetObjectIDWithGroupByTargetIDAndTagIDs(this.Context.Resolve<SYSTagTargetBO>()[tableName].ID.Value, tagIDs.ToArray(), out tagClassCount));
                        strWhere.Append(string.Format(SYSTagSearchSql.MatchRateEqualsSql, tagClassCount));
                        //strOrder.AppendFormat("tApply.[MatchRate] desc,t{0}.[ID] desc", srcTable.Count);
                        break;
                    case EnumSYSTagSearch.Exact:
                        //使用AND
                        strTable.AppendFormat(SYSTagSearchSql.InnerJoinSql,
                            srcTable.Count, this.Context.Resolve<SYSTagApplyBO>().GetTargetObjectIDWithMatchRateByTargetIDAndTagIDs(this.Context.Resolve<SYSTagTargetBO>()[tableName].ID.Value, tagIDs.ToArray()));
                        strWhere.Append(string.Format(SYSTagSearchSql.MatchRateEqualsSql, tagIDs.Length));
                        break;
                }
            }
            else
                strWhere.Append("1=1");
        }

        #endregion

        #region 初始化集团版

        protected long? systemID = null;

        /// <summary>
        /// 设置查询主表
        /// </summary>
        /// <param name="systemID">单位ID</param>
        /// <param name="tableName">数据表名</param>
        internal SYSTagSearchBO(long systemID, string tableName)
            : this(systemID, tableName, null)
        {
        }
        /// <summary>
        /// 设置查询主表，并筛选出已贴入指定标签项的数据
        /// </summary>
        /// <param name="systemID">单位ID</param>
        /// <param name="tableName">数据表名</param>
        /// <param name="tagIDs">标签项ID列表</param>
        internal SYSTagSearchBO(long systemID, string tableName, params long[] tagIDs)
            : this(systemID, tableName, EnumSYSTagSearch.Fuzzy, tagIDs)
        {
        }
        /// <summary>
        /// 设置查询主表，并筛选出已贴入指定标签项的数据
        /// </summary>
        /// <param name="systemID">单位ID</param>
        /// <param name="tableName">数据表名</param>
        /// <param name="matchType">匹配方式 EnumTagSearch</param>
        /// <param name="tagIDs">标签项ID列表</param>
        internal SYSTagSearchBO(long systemID, string tableName, EnumSYSTagSearch matchType, params long[] tagIDs)
        {
            this.systemID = systemID;
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentOutOfRangeException("tableName");
            srcTable.Add(tableName);
            strTable.AppendFormat(SYSTagSearchSql.TableNameSql, tableName, srcTable.Count);

            if (tagIDs != null && tagIDs.Length > 0)
            {
                switch (matchType)
                {
                    case EnumSYSTagSearch.Fuzzy:
                        //使用OR
                        strWhere.AppendFormat(SYSTagSearchSql.InSql,
                            srcTable.Count, this.Context.Resolve<SYSTagApplyBO>().GetTargetObjectIDByTargetIDAndTagIDs(this.Context.Resolve<SYSTagTargetBO>()[tableName].ID.Value, tagIDs.ToArray()));
                        break;
                    case EnumSYSTagSearch.Smart:
                        //使用AND+OR
                        int tagClassCount = 0;
                        strTable.AppendFormat(SYSTagSearchSql.InnerJoinSql,
                            srcTable.Count, this.Context.Resolve<SYSTagApplyBO>().GetTargetObjectIDWithGroupByTargetIDAndTagIDs(this.Context.Resolve<SYSTagTargetBO>()[tableName].ID.Value, tagIDs.ToArray(), out tagClassCount));
                        strWhere.Append(string.Format(SYSTagSearchSql.MatchRateEqualsSql, tagClassCount));
                        //strOrder.AppendFormat("tApply.[MatchRate] desc,t{0}.[ID] desc", srcTable.Count);
                        break;
                    case EnumSYSTagSearch.Exact:
                        //使用AND
                        strTable.AppendFormat(SYSTagSearchSql.InnerJoinSql,
                            srcTable.Count, this.Context.Resolve<SYSTagApplyBO>().GetTargetObjectIDWithMatchRateByTargetIDAndTagIDs(this.Context.Resolve<SYSTagTargetBO>()[tableName].ID.Value, tagIDs.ToArray()));
                        strWhere.Append(string.Format(SYSTagSearchSql.MatchRateEqualsSql, tagIDs.Length));
                        break;
                }
            }
            else
                strWhere.Append("1=1");
        }
        #endregion

        #region 设置表连接
        /// <summary>
        /// 从指定数据表中筛选出已贴入指定标签项的数据，然后内连接到主表
        /// </summary>
        /// <param name="tableName">数据表名</param>
        /// <param name="tableField">连接字段名</param>
        /// <returns></returns>
        public SYSTagSearchBO InnerJoin(string tableName, string tableField)
        {
            return this.Join("inner", "ID", tableName, tableField, null);
        }
        /// <summary>
        /// 从指定数据表中筛选出已贴入指定标签项的数据，然后内连接到主表
        /// </summary>
        /// <param name="tableName">数据表名</param>
        /// <param name="tableField">连接字段名</param>
        /// <param name="tagIDs">标签项ID列表</param>
        /// <returns></returns>
        public SYSTagSearchBO InnerJoin(string tableName, string tableField, params long[] tagIDs)
        {
            return this.Join("inner", "ID", tableName, tableField, tagIDs);
        }
        /// <summary>
        /// 从指定数据表中筛选出已贴入指定标签项的数据，然后内连接到主表
        /// </summary>
        /// <param name="joinField">连接字段名</param>
        /// <param name="tableName">目标表名</param>
        /// <param name="tableField">目标字段名</param>
        /// <param name="tagIDs">标签项ID列表</param>
        /// <returns></returns>
        public SYSTagSearchBO InnerJoin(string joinField, string tableName, string tableField, long[] tagIDs)
        {
            return this.Join("inner", joinField, tableName, tableField, tagIDs);
        }

        /// <summary>
        /// 从指定数据表中筛选出已贴入指定标签项的数据，然后左连接到主表
        /// </summary>
        /// <param name="tableName">数据表名</param>
        /// <param name="tableField">连接字段名</param>
        /// <returns></returns>
        public SYSTagSearchBO LeftJoin(string tableName, string tableField)
        {
            return this.Join("left", "ID", tableName, tableField, null);
        }
        /// <summary>
        /// 从指定数据表中筛选出已贴入指定标签项的数据，然后左连接到主表
        /// </summary>
        /// <param name="tableName">数据表名</param>
        /// <param name="tableField">连接字段名</param>
        /// <param name="tagIDs">标签项ID列表</param>
        /// <returns></returns>
        public SYSTagSearchBO LeftJoin(string tableName, string tableField, params long[] tagIDs)
        {
            return this.Join("left", "ID", tableName, tableField, tagIDs);
        }
        /// <summary>
        /// 从指定数据表中筛选出已贴入指定标签项的数据，然后左连接到主表
        /// </summary>
        /// <param name="joinField">连接字段名</param>
        /// <param name="tableName">目标表名</param>
        /// <param name="tableField">目标字段名</param>
        /// <param name="tagIDs">标签项ID列表</param>
        /// <returns></returns>
        public SYSTagSearchBO LeftJoin(string joinField, string tableName, string tableField, long[] tagIDs)
        {
            return this.Join("left", joinField, tableName, tableField, tagIDs);
        }

        /// <summary>
        /// 从指定数据表中筛选出已贴入指定标签项的数据，然后连接到主表
        /// </summary>
        /// <param name="joinType">连接类型</param>
        /// <param name="joinField">连接字段名</param>
        /// <param name="tableName">目标表名</param>
        /// <param name="tableField">目标字段名</param>
        /// <param name="tagIDs">标签项ID列表</param>
        /// <returns></returns>
        private SYSTagSearchBO Join(string joinType, string joinField, string tableName, string tableField, params long[] tagIDs)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentOutOfRangeException(tableName);
            srcTable.Add(tableName);
            strTable.AppendFormat(" {3} " + SYSTagSearchSql.JoinSql, tableName, srcTable.Count, tableField, joinType, joinField);

            if (tagIDs != null && tagIDs.Length > 0)
            {
                strWhere.AppendFormat(" and " + SYSTagSearchSql.InSql,
                    srcTable.Count,
                    (this.systemID.HasValue ? this.Context.Resolve<SYSTagApplyBO>().With(d => d.systemID = this.systemID.Value) : this.Context.Resolve<SYSTagApplyBO>()).GetTargetObjectIDByTargetIDAndTagIDs(this.Context.Resolve<SYSTagTargetBO>()[tableName].ID.Value, tagIDs.ToArray()));
            }
            return this;
        }

        #endregion

        #region 设置查询条件
        /// <summary>
        /// 增加约束条件
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns></returns>
        public SYSTagSearchBO Where(string sql)
        {
            return this.Where(sql, (DbParameter[])null);
        }
        /// <summary>
        /// 增加约束条件，使用DbParameter参数
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="args">DbParameter查询参数</param>
        /// <returns></returns>
        public SYSTagSearchBO Where(string sql, object args = null)
        {
            if (args != null)
                lstWhere.AddExpandoParams(args);

            strWhere.Append(" and (" + this.resolveTableName(sql) + ")");
            return this;
        }
        /// <summary>
        /// 增加约束条件，使用DbParameter参数
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns></returns>
        public SYSTagSearchBO Where<T>(T query) where T : Entity, new()
        {
            new QueryHelper(this).GetQuerySql(query, strWhere, lstWhere);
            return this;
        }
        /// <summary>
        /// 增加约束条件，使用string.Format参数
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="args">string.Format查询参数</param>
        /// <returns></returns>
        public SYSTagSearchBO Where(string sql, params object[] args)
        {
            strWhere.AppendFormat(" and (" + this.resolveTableName(sql) + ")", args);
            return this;
        }
        #endregion

        #region 获取数据

        #region 查询DataTable（同步）

        /// <summary>
        /// 执行搜索并返回全部字段
        /// </summary>
        /// <returns></returns>
        public DataTable Select()
        {
            return this.UnitOfWork.ToDataTable(DBBuilder.Define(this.ToString(), lstWhere));
        }
        /// <summary>
        /// 执行搜索并返回指定的字段
        /// </summary>
        /// <param name="columns">查询字段</param>
        /// <returns></returns>
        public DataTable Select(params string[] columns)
        {
            strField = new StringBuilder(this.resolveTableName(string.Join(",", columns)));
            return this.UnitOfWork.ToDataTable(DBBuilder.Define(this.ToString(), lstWhere));
        }
        /// <summary>
        /// 执行分页搜索并返回指定的字段
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="itemsPerPage">每页行数</param>
        /// <param name="columns">查询字段</param>
        /// <returns></returns>
        public PagedList Select(int page, int itemsPerPage, params string[] columns)
        {
            strField = new StringBuilder(this.resolveTableName(string.Join(",", columns)));
            return this.UnitOfWork.ToDataTable(DBBuilder.Define(this.ToString(), lstWhere), page, itemsPerPage);
        }
        #endregion

        #region 查询DataTable（异步）
        /// <summary>
        /// 异步执行分页搜索并返回全部字段
        /// </summary>
        /// <returns></returns>
        public GetPagedList SelectAsync()
        {
            return this.SelectAsync("*");
        }
        /// <summary>
        /// 异步执行分页搜索并返回指定的字段
        /// </summary>
        /// <param name="columns">查询字段</param>
        /// <returns></returns>
        public GetPagedList SelectAsync(params string[] columns)
        {
            return new GetPagedList((int pageIndex, int pageSize, out int rowCount, string sortExpressionDirection) =>
            {
                if (!string.IsNullOrEmpty(sortExpressionDirection))
                    strOrder = new StringBuilder(sortExpressionDirection);

                PagedList list = this.Select(pageIndex + 1, pageSize, columns);
                rowCount = list.TotalItems;
                return list;
            });
        }
        #endregion

        #region 查询DataEntityList（同步）
        /// <summary>
        /// 执行搜索并返回实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> Select<T>() where T : Entity, new()
        {
            return this.Select().ToList<T>();
        }
        /// <summary>
        /// 执行搜索并返回指定的字段
        /// </summary>
        /// <typeparam name="T">数据实体类型</typeparam>
        /// <param name="columns">查询字段</param>
        /// <returns></returns>
        public List<T> Select<T>(params string[] columns) where T : Entity, new()
        {
            return this.Select(string.Join(",", columns)).ToList<T>();
        }
        /// <summary>
        /// 执行分页搜索并返回指定的字段
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="itemsPerPage">每页行数</param>
        /// <param name="columns">查询字段</param>
        /// <returns></returns>
        public PagedList<T> Select<T>(int page, int itemsPerPage, params string[] columns) where T : Entity, new()
        {
            strField = new StringBuilder(this.resolveTableName(string.Join(",", columns)));
            return this.UnitOfWork.ToList<T>(DBBuilder.Define(this.ToString(), lstWhere), page, itemsPerPage, null);
        }
        #endregion

        #region 查询DataEntityList（异步）
        /// <summary>
        /// 异步执行分页搜索并返回全部字段
        /// </summary>
        /// <returns></returns>
        public GetPagedList<T> SelectAsync<T>() where T : Entity, new()
        {
            return this.SelectAsync<T>("*");
        }
        /// <summary>
        /// 异步执行分页搜索并返回指定的字段
        /// </summary>
        /// <param name="columns">查询字段</param>
        /// <returns></returns>
        public GetPagedList<T> SelectAsync<T>(params string[] columns) where T : Entity, new()
        {
            return (int pageIndex, int pageSize, out int rowCount, string sortExpressionDirection) =>
            {
                if (!string.IsNullOrEmpty(sortExpressionDirection))
                    strOrder = new StringBuilder(sortExpressionDirection);

                PagedList<T> list = this.Select<T>(pageIndex + 1, pageSize, columns);
                rowCount = list.TotalItems;
                return list;
            };
        }
        #endregion

        #region 查询DataObjectList（同步）
        /// <summary>
        /// 执行搜索并返回实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> SelectObject<T>() where T : new()
        {
            return this.Select().ToList<T>();
        }
        /// <summary>
        /// 执行搜索并返回指定的字段
        /// </summary>
        /// <typeparam name="T">数据实体类型</typeparam>
        /// <param name="columns">查询字段</param>
        /// <returns></returns>
        public List<T> SelectObject<T>(params string[] columns) where T : new()
        {
            return this.Select(string.Join(",", columns)).ToList<T>();
        }
        /// <summary>
        /// 执行分页搜索并返回指定的字段
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="itemsPerPage">每页行数</param>
        /// <param name="columns">查询字段</param>
        /// <returns></returns>
        public PagedList<T> SelectObject<T>(int page, int itemsPerPage, params string[] columns) where T : Entity, new()
        {
            strField = new StringBuilder(this.resolveTableName(string.Join(",", columns)));
            return this.UnitOfWork.ToList<T>(DBBuilder.Define(this.ToString(), lstWhere), page, itemsPerPage);
        }
        #endregion

        #region 查询DataObjectList（异步）
        /// <summary>
        /// 异步执行分页搜索并返回全部字段
        /// </summary>
        /// <returns></returns>
        public GetPagedList<T> SelectObjectAsync<T>() where T : Entity, new()
        {
            return this.SelectObjectAsync<T>("*");
        }
        /// <summary>
        /// 异步执行分页搜索并返回指定的字段
        /// </summary>
        /// <param name="columns">查询字段</param>
        /// <returns></returns>
        public GetPagedList<T> SelectObjectAsync<T>(params string[] columns) where T : Entity, new()
        {
            return (int pageIndex, int pageSize, out int rowCount, string sortExpressionDirection) =>
            {
                if (!string.IsNullOrEmpty(sortExpressionDirection))
                    strOrder = new StringBuilder(sortExpressionDirection);

                PagedList<T> list = this.SelectObject<T>(pageIndex + 1, pageSize, columns);
                rowCount = list.TotalItems;
                return list;
            };
        }
        #endregion

        #endregion

        #region 排序和分组
        /// <summary>
        /// 设置搜索结果按指定字段正序排列
        /// </summary>
        /// <param name="columns">参与排序的字段</param>
        /// <returns></returns>
        public SYSTagSearchBO OrderBy(params string[] columns)
        {
            if (columns == null || columns.Length == 0)
                return this;

            if (strOrder.Length > 0)
                strOrder.Append(",");
            strOrder.Append(this.resolveTableName(string.Join(",", columns)));
            return this;
        }
        /// <summary>
        /// 设置搜索结果按指定字段倒序排列
        /// </summary>
        /// <param name="columns">参与排序的字段</param>
        /// <returns></returns>
        public SYSTagSearchBO OrderByDescending(params string[] columns)
        {
            if (columns == null || columns.Length == 0)
                return this;

            if (strOrder.Length > 0)
                strOrder.Append(",");
            strOrder.Append(this.resolveTableName(string.Join(" desc,", columns) + " desc"));
            return this;
        }
        /// <summary>
        /// 设置分组
        /// </summary>
        /// <param name="columns">参与分组的字段</param>
        /// <returns></returns>
        public SYSTagSearchBO GroupBy(params string[] columns)
        {
            if (columns == null || columns.Length == 0)
                return this;

            if (strGroup.Length > 0)
                strGroup.Append(",");
            strGroup.Append(this.resolveTableName(string.Join(",", columns)));
            return this;
        }

        #endregion

        #region 内部方法
        public override string ToString()
        {
            string sql = "select " + strField + " from " + strTable + " where " + strWhere;
            if (this.strGroup.Length > 0)
                sql += " group by " + strGroup;
            if (this.strOrder.Length > 0)
                sql += " order by " + strOrder;
            return sql;
        }
        /// <summary>
        /// 将查询语句中的表名替换为t[index]格式
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private string resolveTableName(string tableName)
        {
            for (int i = 1; i <= srcTable.Count; i++)
            {
                //string fullTableName = @"(\" + adapter.FormatTable(@")?" + srcTable[i - 1] + @"(\") + @")?\.";
                //string shortTableName = adapter.FormatTable("t" + i) + @".";
                string fullTableName = @"(\" + @")?" + srcTable[i - 1] + @"(\" + @")?\.";
                string shortTableName = "t" + i + @".";
                tableName = Regex.Replace(tableName, fullTableName, shortTableName, RegexOptions.IgnoreCase);
            }
            return tableName;
        }

        #endregion

        #region IEnumerable<DataRow> 成员

        public IEnumerator<DataRow> GetEnumerator()
        {
            foreach (DataRow dr in this.Select().Rows)
                yield return dr;
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        private sealed class QueryHelper
        {
            private SYSTagSearchBO searchBO;
            public QueryHelper(SYSTagSearchBO searchBO)
            {
                this.searchBO = searchBO;
            }
            public void GetQuerySql(Entity query, StringBuilder sql, DBParameterList parms)
            {
                var builder = new EntitySpec<Entity>(query).Sql;
                if (!string.IsNullOrEmpty(builder.Sql))
                    sql.Append(" and (" + this.searchBO.resolveTableName(builder.Sql) + ")");
                if (builder.Parameters.ParameterNames.Count() > 0)
                    parms.AddExpandoParams(builder.Parameters);
            }
        }
    }
}
