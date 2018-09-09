using BIStudio.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Data.MySql
{
    /// <summary>
    /// MySql语法解析器
    /// </summary>
    public sealed class DBAnalyzer : IDBAnalyzer
    {
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
        public IDBAnalyzer Init(string sql, int? pageIndex, int? pageSize, string sort = null)
        {
            this.sql = sql + GetSort(sql, sort);
            this.pageSize = pageSize;
            this.pageIndex = pageIndex;

            return this;
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
                this.count = "SELECT COUNT(*) FROM (" + this.sql + ") DBPager";
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
                int? skip = (this.pageIndex - 1) * this.pageSize;
                int? take = this.pageSize;

                if (this.pageIndex == null || this.pageSize == null)
                {
                    this.select = this.sql;
                }
                else
                {
                    this.select = string.Format(@"{0} LIMIT {1},{2}", this.sql, skip, take);
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
