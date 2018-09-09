using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace BIStudio.Framework.UI
{
    using Data;
    using Domain;

    public abstract class ApplicationService<TViewModel, TQuery> : ApplicationService
        where TViewModel : class, IViewModel, new()
        where TQuery : Query, new()
    {
        #region 内部属性

        /// <summary>
        /// 视图验证器
        /// </summary>
        protected readonly IModelValidator validator;
        /// <summary>
        /// 需要查询的字段
        /// </summary>
        public ApplicationService()
        {
            this.validator = ModelValidator.Default;
        }

        #endregion

        #region Query

        /// <summary>
        /// 查询视图模型
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual IEnumerable<TViewModel> GetAll([FromUri]TQuery info)
        {
            throw new HttpResponseException(HttpStatusCode.NotImplemented);
        }

        /// <summary>
        /// 分页查询视图模型
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual PagedList<TViewModel> Get([FromUri]TQuery info)
        {
            throw new HttpResponseException(HttpStatusCode.NotImplemented);
        }

        /// <summary>
        /// 获取视图模型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TViewModel Get(long id)
        {
            throw new HttpResponseException(HttpStatusCode.NotImplemented);
        }

        #endregion

        #region Command

        /// <summary>
        /// 验证视图模型
        /// </summary>
        /// <param name="vm"></param>
        protected virtual void Validate(TViewModel vm)
        {
            var result = validator.Validate(vm);
            if (result.Any())
                throw new ValidationException(string.Join("\r", result.Select(item => "[" + string.Join(",", item.MemberNames) + "]" + item.ErrorMessage)));
        }

        /// <summary>
        /// 创建视图模型
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual TViewModel Post([FromBody]TViewModel vm)
        {
            throw new HttpResponseException(HttpStatusCode.NotImplemented);
        }

        /// <summary>
        /// 更新视图模型
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPut]
        public virtual TViewModel Put(long id, [FromBody]TViewModel vm)
        {
            throw new HttpResponseException(HttpStatusCode.NotImplemented);
        }

        /// <summary>
        /// 删除视图模型
        /// </summary>
        /// <param name="id">id或ids（id用,隔开）</param>
        /// <returns></returns>
        [HttpDelete]
        public virtual bool Delete(string id)
        {
            throw new HttpResponseException(HttpStatusCode.NotImplemented);
        }

        /// <summary>
        /// 排序视图模型
        /// </summary>
        /// <param name="vm">需要保存的数据</param>
        /// <returns>是否保存成功</returns>
        [HttpPut]
        public virtual bool Sequence([FromBody]List<SortVM> vm)
        {
            throw new HttpResponseException(HttpStatusCode.NotImplemented);
        }

        #endregion
    }
}
