using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Standard.Portal
{
    /// <summary>
    /// 文档
    /// </summary>
    [Table("STDArticle")]
    public class STDArticle : Entity
    {
        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 文章摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 文章链接
        /// </summary>
        public string ArticleUrl { get; set; }

        /// <summary>
        /// 封面图片链接
        /// </summary>
        public string CoverUrl { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string TagIDs { get; set; }

        /// <summary>
        /// 文章标签
        /// </summary>
        public string TagNames { get; set; }

        /// <summary>
        /// 文章来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 文章来源链接
        /// </summary>
        public string SourceUrl { get; set; }

        /// <summary>
        /// 浏览量
        /// </summary>
        public int? Views { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        public bool? IsValid { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool? IsDelete { get; set; }

        /// <summary>
        /// 归档状态
        /// </summary>
        public int? ArchiveStatus { get; set; }

        /// <summary>
        /// 发布人
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// 发布人的ID
        /// </summary>
        public long? PublisherID { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? PublishTime { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? ExpirationTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Inputer { get; set; }

        /// <summary>
        /// 创建人的ID
        /// </summary>
        public long? InputerID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? InputTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Updater { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? UpdaterID { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 引用来源
        /// </summary>
        public long? RefferID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Sequence { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? UnitID { get; set; }
    }

}
