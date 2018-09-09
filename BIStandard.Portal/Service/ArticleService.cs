using BIStudio.Framework;
using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Standard.Portal
{
    [Ioc(typeof(IArticleService))]
    public class ArticleService : DomainService, IArticleService
    {
    }
}
