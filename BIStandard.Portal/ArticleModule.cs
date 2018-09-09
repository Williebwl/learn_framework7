using BIStudio.Framework;
using BIStudio.Framework.Domain;
using BIStudio.Standard.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Standard.Portal
{
    public class ArticleModule : ApplicationModule
    {
        protected override void Init()
        {
            CFAspect.RegisterType<IRepository<STDArticle>, Repository<STDArticle>>();
            CFAspect.RegisterType<IRepository<STDArticleContent>, Repository<STDArticleContent>>();
        }
    }
}
