namespace BIStudio.Framework.Data
{
    /// <summary>
    ///     异步查询请求，支持分页、排序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pageIndex">当前页码</param>
    /// <param name="pageSize">每页行数</param>
    /// <param name="rowCount">总行数</param>
    /// <param name="sortExpression">排序字段</param>
    /// <returns></returns>
    public delegate PagedList<T> GetPagedList<T>(int pageIndex, int pageSize, out int rowCount, string sortExpression);

    /// <summary>
    ///     异步查询请求，支持分页、排序
    /// </summary>
    /// <param name="pageIndex">当前页码</param>
    /// <param name="pageSize">每页行数</param>
    /// <param name="rowCount">总行数</param>
    /// <param name="sortExpression">排序字段</param>
    /// <returns></returns>
    public delegate PagedList GetPagedList(int pageIndex, int pageSize, out int rowCount, string sortExpression);
}