using BIStudio.Framework.Data;
using BIStudio.Framework.UI;
using BIStudio.Standard.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace WebApi.Controllers.Portal
{
    public class ArticleController : ApplicationService<ArticleVM, PagedQuery, STDArticle>
    {
        public ArticleController() : base("Title") { }
    }
}