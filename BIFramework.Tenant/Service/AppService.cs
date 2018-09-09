using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Tenant
{
    using BIStudio.Framework.Domain;
    using Utils;    //using Omu.ValueInjecter;

    /// <summary>
    /// app Service
    /// </summary>
    [Ioc(typeof(IAppService))]
    public class AppService : DomainService, IAppService
    {
        private IRepository<SYSSystem> _systemRepository;
        private IRepository<SYSSystemCertificate> _certificateRepository;

        protected IRepository<SYSApp> _appBO;
        protected IRepository<SYSMenu> _menuBO;
        protected IRepository<SYSAppAccess> _appAccessBO;


        #region 系统
        /// <summary>
        /// 获取系统信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public SYSSystem GetSystemInfo(string code)
        {
            var system = _systemRepository.Get(item => item.SystemCode == code);
            if (!system.ID.HasValue)
                throw CFException.Create(OperateResult.NotFound);
            return system;
        }
        /// <summary>
        /// 获取系统信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public SYSSystem GetSystemInfo(long systemId)
        {
            var system = _systemRepository.Get(systemId);
            if (!system.ID.HasValue)
                throw CFException.Create(OperateResult.NotFound);
            return system;
        }

        /// <summary>
        /// 系统注册
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public void SystemRegist(SYSSystemRegistDTO dto)
        {
            if (string.IsNullOrEmpty(dto.SystemName) || string.IsNullOrEmpty(dto.SystemCode))
                throw CFException.Create(SYSSystemRegistResult.NameOrCodeNotFound);
            try
            {
                var system = _systemRepository.Get(item => item.SystemCode == dto.SystemCode);
                if (system.ID.HasValue)
                {
                    if (system.GetVersion() > dto.GetVersion())
                        throw CFException.Create(SYSSystemRegistResult.CodeAlreadyExists);
                }
                else
                    system = new SYSSystem(dto.SystemCode);

                system = CFMapper.Map(dto, system);
                if (system.ID.HasValue)
                {
                    _systemRepository.Modify(system);
                }
                else
                {
                    _systemRepository.Add(system);
                }
                return;
            }
            catch (Exception ex)
            {
                throw CFException.Create(SYSSystemRegistResult.Fail, ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除系统
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public void UnRegisterSystem(string code)
        {
            var entity = _systemRepository.Get(item => item.SystemCode == code);
            if (!entity.ID.HasValue)
                throw CFException.Create(OperateResult.NotFound);
            try
            {
                _systemRepository.Remove(entity);
                return;
            }
            catch (Exception ex)
            {
                throw CFException.Create(OperateResult.Fail, ex.Message, ex);
            }
        }

        /// <summary>
        /// 为指定系统颁发新证书
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public SYSSystemCertificate CertificateIssue(SYSSystemCertificateIssueDTO dto)
        {
            if (string.IsNullOrEmpty(dto.SystemCode) || string.IsNullOrEmpty(dto.ApiKey) || string.IsNullOrEmpty(dto.CertificateName))
                throw CFException.Create(STDCertificateIssueResult.NameOrCodeNotFound);

            try
            {
                SYSSystem system = _systemRepository.Get(item => item.SystemCode == dto.SystemCode);
                if (system.ID == null)
                    throw CFException.Create(STDCertificateIssueResult.SystemCodeInvalid);

                SYSSystemCertificate entity = dto.Map<SYSSystemCertificateIssueDTO, SYSSystemCertificate>();
                var prevCertificate = _certificateRepository.Get(item => item.ApiKey == dto.ApiKey);
                if (prevCertificate.ID.HasValue)
                    throw CFException.Create(STDCertificateIssueResult.CodeAlreadyExists);

                entity.SystemID = system.ID;
                entity.ApiKey = entity.ApiKey;
                entity.Secret = ALUtils.GetGUIDShort();
                entity.IsValid = true;
                entity.InputTime = DateTime.Now;
                entity.Inputer = CFContext.User.UserName;
                entity.InputerID = CFContext.User.ID;
                _certificateRepository.Add(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw CFException.Create(STDCertificateIssueResult.Fail, ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除证书
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        /// <exception cref="BIStudio.Framework.DefinedException"></exception>
        public void RecyleCertificate(string apiKey)
        {
            var entity = _certificateRepository.Get(item => item.ApiKey == apiKey);
            if (!entity.ID.HasValue)
                throw CFException.Create(OperateResult.NotFound);
            try
            {
                _certificateRepository.Remove(entity);
                return;
            }
            catch (Exception ex)
            {

                throw CFException.Create(OperateResult.Fail, ex.Message, ex);
            }
        }
        #endregion

        #region 保存应用

        /// <summary>
        /// 保存应用信息
        /// </summary>
        /// <param name="dto">应用信息</param>
        /// <returns>是否成功</returns>
        public virtual bool SaveApp(SYSAppRegistDTO dto)
        {
            using (var dbContext = BoundedContext.Create())
            {
                if (!(SaveApp(dbContext, dto) &&
                    SaveMenu(dbContext, dto) &&
                    SaveAppAccess(dbContext, dto))) return false;

                dbContext.Commit();
                return true;
            }
        }

        /// <summary>
        /// 保存应用信息
        /// </summary>
        /// <param name="dbContext">上下文对象</param>
        /// <param name="dto">应用信息</param>
        /// <returns>是否成功</returns>
        protected virtual bool SaveApp(IBoundedContext dbContext, SYSAppRegistDTO dto)
        {
            if (!(dto.App.ID > 0)) InitApp(dto.App);

            return dto.App.ID.HasValue ? dbContext.Modify(dto.App) : dbContext.Add(dto.App);
        }

        /// <summary>
        /// 初始化应用参数
        /// </summary>
        /// <param name="app">应用信息</param>
        protected virtual void InitApp(SYSApp app)
        {
            if (app.ID > 0) return;

            if (!app.Sequence.HasValue) app.Sequence = 0;

            if (!app.IsBuiltIn.HasValue) app.IsBuiltIn = 1;

            if (!app.IsValid.HasValue) app.IsValid = 1;
        }

        /// <summary>
        /// 保存应用菜单信息
        /// </summary>
        /// <param name="dbContext">上下文对象</param>
        /// <param name="dto">应用信息</param>
        /// <returns>是否成功</returns>
        protected virtual bool SaveMenu(IBoundedContext dbContext, SYSAppRegistDTO dto)
        {
            var menu = dbContext.Repository<SYSMenu>();

            var q = from d in menu.Entities
                    where d.AppID == dto.App.ID
                    && d.Layer == 0
                    select d;

            var target = q.FirstOrDefault();

            if (target != null) dto.Menu.Map(target);
            else target = dto.Menu;

            InitMenu(dto.App, target);

            return target.ID > 0 ? menu.Modify(target) : menu.Add(target);
        }

        /// <summary>
        /// 初始化应用菜单信息
        /// </summary>
        /// <param name="app">应用信息</param>
        /// <param name="menu">菜单信息</param>
        protected virtual void InitMenu(SYSApp app, SYSMenu menu)
        {
            if (!(menu.ID > 0))
            {
                menu.AppID = app.ID;
                menu.ParentID = 0;
                menu.Path = ",";
                menu.Layer = 0;
            }

            menu.MenuCode = app.AppCode;
            menu.ShortName = menu.MenuName = app.AppName;
            menu.PageRoute = app.AppCode;
            menu.Sequence = app.Sequence;
            menu.Remarks = app.Remark;

            if (string.IsNullOrEmpty(menu.DisplayMode)) menu.DisplayMode = "默认";

            if (!menu.DisplayModeID.HasValue) menu.DisplayModeID = 0;

            if (!menu.IsShow.HasValue) menu.IsShow = false;
        }

        /// <summary>
        /// 保存应用用户组关联信息
        /// </summary>
        /// <param name="dbContext">上下文对象</param>
        /// <param name="dto">应用信息</param>
        /// <returns>是否成功</returns>
        protected virtual bool SaveAppAccess(IBoundedContext dbContext, SYSAppRegistDTO dto)
        {
            var result = true;
            var appAccess = dbContext.Repository<SYSAppAccess>();

            if (dto.App.ID > 0) appAccess.Remove(new Spec<SYSAppAccess>(d => d.AppID == dto.App.ID));

            if (dto.AppGroups != null) foreach (var appAccessInfo in dto.AppGroups) { appAccessInfo.AppID = dto.App.ID; if (!(result = appAccess.Add(appAccessInfo))) break; }

            return result;
        }

        #endregion 保存应用

    }
}
