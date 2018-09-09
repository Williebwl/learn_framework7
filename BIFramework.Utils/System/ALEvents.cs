using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIStudio.Framework.Utils
{
    /// <summary>
    /// 异步查询请求，支持分页
    /// </summary>
    /// <param name="pageIndex">当前页码</param>
    /// <param name="pageSize">每页行数</param>
    /// <param name="rowCount">总行数</param>
    /// <returns></returns>
    public delegate object GetCustomPageData(int pageIndex, int pageSize, out int rowCount);
    /// <summary>
    /// 异步查询请求，支持分页、排序
    /// </summary>
    /// <param name="pageIndex">当前页码</param>
    /// <param name="pageSize">每页行数</param>
    /// <param name="rowCount">总行数</param>
    /// <param name="sortExpressionDirection">排序字段</param>
    /// <returns></returns>
    public delegate object GetCustomSortPageData(int pageIndex, int pageSize, out int rowCount, string sortExpressionDirection);

}
