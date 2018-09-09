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
    /// 文档内容
    /// </summary>
    [Table("STDArticleContent")]
    public class STDArticleContent : Entity
    {
        /// <summary>
        /// 文章编号
        /// </summary>
        public long? ArticleID { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        public string ContentHTML { get; set; }

        /// <summary>
        /// 文章扩展内容
        /// </summary>
        public object ContentXML { get; set; }

        /// <summary>
        /// 文章模板编号
        /// </summary>
        public long? TemplateID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? UnitID { get; set; }
    }

}
