using BIStudio.Framework.Data;
using BIStudio.Framework.Tag;
using BIStudio.Framework.UI;

namespace WebApi.Controllers.Tag
{

    public class TagGroupController : ApplicationService<TagGroupVM, PagedQuery, SYSTagGroup>
    {
        public TagGroupController() : base("GroupName") { }
    }
}