using System.Collections.Generic;
using System.Linq;
using BIStudio.Framework;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Institution;
using BIStudio.Framework.Tenant;
using BIStudio.Framework.UI;

namespace WebApi.Controllers.Tenant
{
    public partial class AppController : ApplicationService
    {
        protected IRepository<SYSApp> _appBO;
        protected IRepository<SYSMenu> _menuBO;
        protected IRepository<SYSAppAccess> _appAccessBO;
        protected IRepository<SYSGroup> _groupBO;

        protected IAppService _appService;

        protected virtual AppVM GetInfo(long id)
        {
            var q = from d in _appBO.Entities
                    where d.ID == id
                    select d;

            return q.Single().Map<SYSApp, AppVM>();
        }


        protected virtual IList<AppVM> GetInfos()
        {
            var q = from d in _appBO.Entities
                    orderby d.AppTypeID, d.Sequence
                    select d;

            return q.Map<SYSApp, AppVM>().ToArray();
        }

        protected virtual AppEditVM GetEditVM(long id)
        {
            var vm = new AppEditVM();

            var appq = from d in _appBO.Entities
                       where d.ID == id
                       select d;

            vm.App = appq.Single().Map<SYSApp, AppVM>();

            var menuq = from d in _menuBO.Entities
                        where d.AppID == id && d.Layer == 0
                        select d;

            vm.Menu = menuq.SingleOrDefault().Map<SYSMenu, MenuVM>();

            var appAccessq = from d in _appAccessBO.Entities
                             from b in _groupBO.Entities
                             where d.AppID == id && b.AppID == id && b.ID == d.GroupID
                             select new AppAccessVM { ID = d.ID, AppID = id, GroupID = d.GroupID, GroupCode = b.GroupCode, GroupName = b.GroupName };

            vm.AppGroups = appAccessq.ToList();

            return vm;
        }

        protected virtual long SaveVM(AppEditVM vm)
        {
            var dto = vm.Map<AppEditVM, SYSAppRegistDTO>();

            return _appService.SaveApp(dto) ? dto.App.ID.Value : 0;
        }
    }
}