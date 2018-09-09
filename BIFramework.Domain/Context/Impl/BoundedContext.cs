using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    /// <summary>
    /// 限界上下文
    /// </summary>
    public class BoundedContext : DomainResolver, IBoundedContext
    {
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public static IBoundedContext Create()
        {
            return new BoundedContext(UnitOfWorkOptions.Default);
        }
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public static IBoundedContext Create(string connectionName)
        {
            return new BoundedContext(new UnitOfWorkOptions { ConnectionName = connectionName });
        }
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public static IBoundedContext Create(UnitOfWorkOptions options)
        {
            return new BoundedContext(options);
        }
        private BoundedContext(UnitOfWorkOptions options)
        {
            getInstance = () => CFAspect.Resolve<IUnitOfWork>(options);
        }
        private BoundedContext(IUnitOfWork uow)
        {
            getInstance = () => uow;
        }

        #region IAspectResolver<ITransientDependency>

        public override ITransientDependency Resolve(Type target)
        {
            var instance = base.Resolve(target);
            instance.DependOn(this);
            return instance;
        }

        public override T Resolve<T>()
        {
            var instance = base.Resolve<T>();
            instance.DependOn(this);
            return instance;
        }

        #endregion

        #region IBoundedContext

        private IUnitOfWork instance;
        private Func<IUnitOfWork> getInstance;
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return this.instance = this.instance ?? getInstance();
            }
            set
            {
                this.instance = value;
            }
        }
        /// <summary>
        /// 执行事务
        /// </summary>
        public void Commit()
        {
            this.UnitOfWork.Commit();
        }
        /// <summary>
        /// 取消事务
        /// </summary>
        public void Rollback()
        {
            this.UnitOfWork.Rollback();
        }
        public void Dispose()
        {
            this.UnitOfWork.Dispose();
        }

        #endregion
        
    }
}
