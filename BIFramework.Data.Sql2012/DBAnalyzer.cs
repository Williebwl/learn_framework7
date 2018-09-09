
using BIStudio.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BIStudio.Framework.Data.Sql2012
{
    /// <summary>
    /// Sql语法解析器 标准版，需要Sql2012以上版本
    /// </summary>
    public sealed class DBAnalyzer : IDBAnalyzer
    {
        private static Regex rxColumns = new Regex(@"\A\s*SELECT\s+((?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|.)*?)(?<!,\s+)\bFROM\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex rxOrderBy = new Regex(@"\bORDER\s+BY\s+(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.\[\]])+(?:\s+(?:ASC|DESC))?(?:\s*,\s*(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.\[\]])+(?:\s+(?:ASC|DESC))?)*", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex rxOrderByWithoutTableName = new Regex(@"(\w)+\.", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex rxDistinct = new Regex(@"\ADISTINCT\s", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);

        #region 初始化

        /// <summary>
        /// 每页大小
        /// </summary>
        private int? pageSize;
        /// <summary>
        /// 当前页码
        /// </summary>
        private int? pageIndex;
        /// <summary>
        /// 原始Sql语句
        /// </summary>
        private string sql;

        /// <summary>
        /// 初始化Sql语法解析器
        /// </summary>
        /// <param name="sql">输入字符串</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        public IDBAnalyzer Init(string sql, int? pageIndex, int? pageSize,string sort=null)
        {
            return Init(sql + GetSort(sql, sort), pageIndex, pageSize, 0, new Dictionary<char, char>
            {
                { '(', ')' },
                { '\'', '\'' },
            });
        }

        /// <summary>
        /// 初始化Sql语法解析器
        /// </summary>
        /// <param name="sql">输入字符串</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="startAt">起始位置</param>
        /// <param name="pairs">成对的字符</param>
        private IDBAnalyzer Init(string sql, int? pageIndex, int? pageSize, int startAt, Dictionary<char, char> pairs)
        {
            this.sql = sql;
            this.pageSize = pageSize;
            this.pageIndex = pageIndex;
            this.startPos = startAt;
            this.pairs = pairs;

            //参数错误
            if (!rxColumns.Match(sql).Success)
                throw new ArgumentOutOfRangeException("DBSqlAnalyzer 未找到查询语句");
            if (sql.Length == 0 || startAt > sql.Length || pairs.Count == 0)
                throw new ArgumentOutOfRangeException("DBSqlAnalyzer 参数错误");

            //预处理
            this.input = this.sql.Replace("''", "  ");  //忽略字符串中的转义字符
            return this;
        }

        #endregion

        #region 查找OrderBy位置

        /// <summary>
        /// 已处理过的Sql语句
        /// </summary>
        private string input;
        /// <summary>
        /// 当前查找位置
        /// </summary>
        private int? currentPos = null;
        /// <summary>
        /// 当前查找堆栈
        /// </summary>
        private Stack<char> stack = new Stack<char>();
        /// <summary>
        /// 起始查找位置
        /// </summary>
        private int startPos;
        /// <summary>
        /// 闭合的字符对
        /// </summary>
        private Dictionary<char, char> pairs;

        /// <summary>
        /// 返回最后一次匹配结果的终止位置
        /// </summary>
        /// <returns></returns>
        protected int GetLastPosition()
        {
            this.currentPos = this.currentPos ?? this.startPos - 1;
            if (this.currentPos + 1 >= this.input.Length)
            {
                if (this.stack.Count == 0)
                    return this.currentPos.Value;
                else
                    throw new Exception("DBSqlAnalyzer 没有找到匹配的闭合字符：" + this.stack.First());
            }

            //当前起始字符
            if (this.stack.Count == 0)
            {
                char[] openChars = this.pairs.Keys.ToArray();
                //查找下一个起始字符
                int nextPos = this.input.IndexOfAny(openChars, this.currentPos.Value + 1);
                if (nextPos < 0 || nextPos + 1 > this.input.Length)
                    return this.currentPos.Value;
                //当前是最外层，并且包含order by语句
                string currentStr = this.input.Substring(this.currentPos.Value + 1, nextPos - this.currentPos.Value + 1);
                if (currentStr.IndexOf("order by", StringComparison.CurrentCultureIgnoreCase) > 0)
                    return this.currentPos.Value;

                char openChar = this.input[nextPos];
                this.stack.Push(openChar);
                this.currentPos = nextPos;
                return this.GetLastPosition();
            }
            //查找闭合字符
            else
            {
                char openChar = this.stack.First();
                char closeChar = this.pairs[this.stack.First()];

                char[] closeChars;
                if (openChar == '\'')
                    closeChars = new char[] { '\'' };
                else
                    closeChars = this.pairs.Keys.Concat(new char[] { closeChar }).ToArray();
                //查找关闭字符或嵌套的起始字符,引号内不允许嵌套
                int nextPos = this.input.IndexOfAny(closeChars, this.currentPos.Value + 1);
                if (nextPos < 0 || nextPos + 1 > this.input.Length)
                    throw new Exception("DBPairFinder 没有找到匹配的闭合字符：" + openChar);

                char currentChar = this.input[nextPos];
                if (currentChar == closeChar)
                {
                    //闭合字符
                    this.stack.Pop();
                    this.currentPos = nextPos;
                    return this.GetLastPosition();
                }
                else
                {
                    //嵌套的开始字符
                    this.stack.Push(currentChar);
                    this.currentPos = nextPos;
                    return this.GetLastPosition();
                }
            }
        }

        private int? lastPosition = null;
        /// <summary>
        /// 最后一次匹配结果的终止位置
        /// </summary>
        protected int LastPosition
        {
            get
            {
                if (this.lastPosition == null)
                {
                    this.lastPosition = this.GetLastPosition();
                }
                return this.lastPosition.Value;
            }
        }
        #endregion

        #region 获得OrderBy语句

        private string orderBy = null;
        /// <summary>
        /// 获得Order By语句
        /// </summary>
        protected string OrderBy
        {
            get
            {
                if (this.orderBy == null)
                {
                    int lastPos = this.LastPosition;
                    Match m;
                    if (lastPos > 0)
                        m = rxOrderBy.Match(this.sql, lastPos);
                    else
                        m = rxOrderBy.Match(this.sql);
                    if (!m.Success)
                    {
                        this.orderBy = null;
                    }
                    else
                    {
                        this.orderBy = m.Groups[0].ToString();
                    }
                }
                return this.orderBy;
            }
        }

        private string orderByWithoutTableName = null;
        /// <summary>
        /// 获得排除了表名的Order By语句
        /// </summary>
        protected string OrderByWithoutTableName
        {
            get
            {
                if (this.orderByWithoutTableName == null && this.OrderBy != null)
                {
                    this.orderByWithoutTableName = rxOrderByWithoutTableName.Replace(this.OrderBy, (Match m) =>
                    {
                        if (m.Value == "dbo.")
                            return m.Value;
                        else
                            return "";
                    });
                }
                return this.orderByWithoutTableName;
            }
        }

        private string selectWithoutOrderBy = null;
        /// <summary>
        /// 排除OrderBy语句之后的查询语句
        /// </summary>
        protected string SelectWithoutOrderBy
        {
            get
            {
                if (this.selectWithoutOrderBy == null)
                {
                    if (string.IsNullOrEmpty(this.OrderBy))
                    {
                        this.selectWithoutOrderBy = this.sql;
                    }
                    else
                    {
                        int sqlOrderBegin = this.input.IndexOf(this.OrderBy, this.LastPosition > 0 ? this.LastPosition : 0);
                        int sqlOrderEnd = sqlOrderBegin + this.OrderBy.Length;

                        this.selectWithoutOrderBy = this.sql.Substring(0, sqlOrderBegin) + this.sql.Substring(sqlOrderEnd);
                    }

                }
                return this.selectWithoutOrderBy;
            }
        }
        #endregion

        #region 获得Select语句

        private string count;
        /// <summary>
        /// 获得Count语句
        /// </summary>
        public string Count()
        {
            if (this.count == null)
            {
                this.count = "SELECT COUNT(*) FROM (" + this.SelectWithoutOrderBy + ") DBPager";
            }
            return this.count;
        }

        private string select;
        /// <summary>
        /// 获得SELECT语句
        /// </summary>
        public string Select()
        {
            if (this.select == null)
            {
                string orderBy = (string.IsNullOrEmpty(this.OrderByWithoutTableName) ? "ORDER BY (SELECT NULL)" : this.OrderByWithoutTableName);
                int? skip = (this.pageIndex - 1) * this.pageSize;
                int? take = this.pageSize;

                if (this.pageIndex == null || this.pageSize == null)
                {
                    //查询全部
                    this.select = string.Format(@"SELECT * FROM ({1}) DBPager {0}",
                        orderBy, this.SelectWithoutOrderBy, skip, skip + take);
                }
                else
                {
                    this.select = this.select = string.Format(@"{0}{1} OFFSET {2} ROWS FETCH NEXT {3} ROWS ONLY", this.SelectWithoutOrderBy, orderBy, skip, take);
                }
            }
            return this.select;
        }

        #endregion

        private string GetSort(string sql, string sortExpression)
        {
            //合并排序
            string orderby = string.Empty;
            if (string.IsNullOrEmpty(sortExpression))
                orderby = "";
            else if (sql.IndexOf("order by", StringComparison.CurrentCultureIgnoreCase) > 0)
                orderby += "," + sortExpression;
            else
                orderby = " order by " + sortExpression;
            return orderby;
        }
    }
}
