using System.Collections.Generic;
using System.Data;

namespace BIStudio.Framework.Data
{
    /// <summary>
    ///     含分页信息的强类型数据实体集合
    /// </summary>
    /// <typeparam name="T">数据实体类型</typeparam>
    public class PagedList<T>
    {
        public PagedList() { }
        public PagedList(int currentPage, int itemsPerPage, int totalItems, List<T> items)
        {
            this.CurrentPage = currentPage;
            this.ItemsPerPage = itemsPerPage;
            this.TotalItems = totalItems;
            this.TotalPages = totalItems % itemsPerPage == 0 ? totalItems / itemsPerPage : (totalItems / itemsPerPage) + 1;
            this.Items = items;
        }
        /// <summary>
        ///     当前页码
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        ///     总页码
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        ///     总行数
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        ///     每页行数
        /// </summary>
        public int ItemsPerPage { get; set; }

        /// <summary>
        ///     数据实体
        /// </summary>
        public List<T> Items { get; set; }
    }

    /// <summary>
    ///     含分页信息的弱类型数据实体集合
    /// </summary>
    public class PagedList : PagedList<DataRow>
    {
    }
}