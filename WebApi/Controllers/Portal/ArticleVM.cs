using BIStudio.Framework.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Controllers.Portal
{
    /// <summary>
    /// 文档视图模型
    /// </summary>
    public class ArticleVM : ViewModel
    {
        #region Write Model

        /// <summary>
        /// 文章标题
        /// </summary>
        [StringLength(400), Display(Name = "文章标题")]
        public string Title { get; set; }

        /// <summary>
        /// 文章摘要
        /// </summary>
        [Display(Name = "文章摘要")]
        public string Summary { get; set; }

        /// <summary>
        /// 文章链接
        /// </summary>
        [StringLength(255), RegularExpression(@"^[\t\r\n\u0020-\u007e]*$", ErrorMessage = "字段 {0} 必须是英文字母、数字或符号。"), Url, Display(Name = "文章链接")]
        public string ArticleUrl { get; set; }

        /// <summary>
        /// 封面图片链接
        /// </summary>
        [StringLength(255), RegularExpression(@"^[\t\r\n\u0020-\u007e]*$", ErrorMessage = "字段 {0} 必须是英文字母、数字或符号。"), Url, Display(Name = "封面图片链接")]
        public string CoverUrl { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [StringLength(200), RegularExpression(@"^[\t\r\n\u0020-\u007e]*$", ErrorMessage = "字段 {0} 必须是英文字母、数字或符号。"), Display(Name = "编号")]
        public string TagIDs { get; set; }

        /// <summary>
        /// 文章标签
        /// </summary>
        [StringLength(400), Display(Name = "文章标签")]
        public string TagNames { get; set; }

        /// <summary>
        /// 文章来源
        /// </summary>
        [StringLength(400), Display(Name = "文章来源")]
        public string Source { get; set; }

        /// <summary>
        /// 文章来源链接
        /// </summary>
        [StringLength(2000), RegularExpression(@"^[\t\r\n\u0020-\u007e]*$", ErrorMessage = "字段 {0} 必须是英文字母、数字或符号。"), Url, Display(Name = "文章来源链接")]
        public string SourceUrl { get; set; }

        /// <summary>
        /// 浏览量
        /// </summary>
        [Display(Name = "浏览量")]
        public int? Views { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        [Display(Name = "是否发布")]
        public bool? IsValid { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [Display(Name = "是否删除")]
        public bool? IsDelete { get; set; }

        /// <summary>
        /// 归档状态
        /// </summary>
        [Display(Name = "归档状态")]
        public int? ArchiveStatus { get; set; }

        /// <summary>
        /// 发布人
        /// </summary>
        [StringLength(400), Display(Name = "发布人")]
        public string Publisher { get; set; }

        /// <summary>
        /// 发布人的ID
        /// </summary>
        [Display(Name = "发布人的ID")]
        public long? PublisherID { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        [Display(Name = "发布时间")]
        public DateTime? PublishTime { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        [Display(Name = "过期时间")]
        public DateTime? ExpirationTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [StringLength(400), Display(Name = "创建人")]
        public string Inputer { get; set; }

        /// <summary>
        /// 创建人的ID
        /// </summary>
        [Display(Name = "创建人的ID")]
        public long? InputerID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public DateTime? InputTime { get; set; }

        /// <summary>
        /// Updater
        /// </summary>
        [StringLength(400), Display(Name = "Updater")]
        public string Updater { get; set; }

        /// <summary>
        /// UpdaterID
        /// </summary>
        [Display(Name = "UpdaterID")]
        public long? UpdaterID { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 引用来源
        /// </summary>
        [Display(Name = "引用来源")]
        public long? RefferID { get; set; }

        /// <summary>
        /// Sequence
        /// </summary>
        [Display(Name = "Sequence")]
        public int? Sequence { get; set; }

        /// <summary>
        /// UnitID
        /// </summary>
        [Display(Name = "UnitID")]
        public long? UnitID { get; set; }

        #endregion

        #region Read Model

        /// <summary>
        /// ID
        /// </summary>
        [Display(Name = "ID")]
        public long? ID { get; set; }

        #endregion
    }

}