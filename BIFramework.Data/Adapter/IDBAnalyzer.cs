using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Data
{
    /// <summary>
    /// 数据分析器，仅供开发人员使用
    /// </summary>
    public interface IDBAnalyzer
    {
        /// <summary>
        /// 初始化Sql语法解析器
        /// </summary>
        /// <param name="sql">输入字符串</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        IDBAnalyzer Init(string sql, int? pageIndex, int? pageSize, string sort = null);
        /// <summary>
        /// 获得Count语句
        /// </summary>
        string Count();
        /// <summary>
        /// 获得SELECT语句
        /// </summary>
        string Select();
    }
}
