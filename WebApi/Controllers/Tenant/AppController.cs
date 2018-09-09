using System.Collections.Generic;
using System.Web.Http;
using BIStudio.Framework.Tenant;
using BIStudio.Framework.UI;

namespace WebApi.Controllers.Tenant
{
    /// <summary>
    /// 应用
    /// </summary>
    [RoutePrefix("api/App")]
    public partial class AppController : ApplicationService
    {
        #region 查询

        /// <summary>
        /// 查询指定应用信息
        /// </summary>
        /// <param name="id">应用id</param>
        /// <returns>应用信息</returns>
        public virtual AppVM Get(long id) => GetInfo(id);

        /// <summary>
        /// 获取所有应用信息
        /// </summary>
        /// <returns>所有应用信息</returns>
        [HttpGet]
        public virtual IList<AppVM> GetAll() => GetInfos();

        /// <summary>
        /// 获取需要编辑的应用信息
        /// </summary>
        /// <param name="id">应用id</param>
        /// <returns>应用信息</returns>
        [HttpGet]
        public virtual AppEditVM GetEditInfo(long id) => GetEditVM(id);

        #endregion 查询

        #region 编辑

        /// <summary>
        /// 添加应用
        /// </summary>
        /// <param name="vm">应用信息</param>
        /// <returns>应用id</returns>
        [HttpPost]
        public virtual long Post([FromBody]AppEditVM vm) => SaveVM(vm);

        /// <summary>
        /// 修改应用信息
        /// </summary>
        /// <param name="vm">应用信息</param>
        /// <returns>应用id</returns>
        [HttpPut]
        public virtual long Put([FromBody]AppEditVM vm) => SaveVM(vm);

        /// <summary>
        /// 设置应用状态
        /// </summary>
        /// <param name="id">应用id</param>
        /// <param name="status">状态值</param>
        /// <returns>是否成功</returns>
        [HttpPut, Route("SetStatus/{id}/{status}")]
        public virtual bool SetStatus(long id, int status)=> _appBO.Modify(new SYSApp { ID = id, IsValid = status });

        #endregion 编辑
    }
}