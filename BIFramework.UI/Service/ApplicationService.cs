using System.Runtime.Serialization;
using System.Web.Http;
using System.Xml.Serialization;

namespace BIStudio.Framework.UI
{
    using BIStudio.Framework;
    using BIStudio.Framework.Data;
    using BIStudio.Framework.Domain;

    /// <summary>
    /// 应用服务
    /// </summary>
    [Authorize]
    public abstract class ApplicationService : ApiController, IApplicationService
    {
        public ApplicationService()
        {
            this.OnInject();
            this.DependOn(BoundedContext.Create(new UnitOfWorkOptions { IsTransactional = false }));
        }

        /// <summary>
        /// 将工作单元注入到实现了ITransientDependency接口的私有属性
        /// </summary>
        [NonAction]
        public virtual void OnInject()
        {
            CFConfig.Default.ScanField<ITransientDependency>(this.GetType(), field => field.SetValue(this, CFAspect.Resolve(field.FieldType)));
        }

        /// <summary>
        /// 将工作单元注入到实现了ITransientDependency接口的私有属性
        /// </summary>
        /// <param name="uow"></param>
        [NonAction]
        public virtual void DependOn(IBoundedContext uow)
        {
            this.Context = uow;
            CFConfig.Default.ScanField<ITransientDependency>(this.GetType(), field => (field.GetValue(this) as ITransientDependency)?.DependOn(uow));
        }

        /// <summary>
        /// 限界上下文
        /// </summary>
        [IgnoreDataMember, XmlIgnore, Column(IsExtend = true)]
        protected IBoundedContext Context
        {
            get;
            private set;
        }
        /// <summary>
        /// 工作单元
        /// </summary>
        [IgnoreDataMember, XmlIgnore, Column(IsExtend = true)]
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return this.Context.UnitOfWork;
            }
        }

    }
}
