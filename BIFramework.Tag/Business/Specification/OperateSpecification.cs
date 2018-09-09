using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using System.Data;
using BIStudio.Framework.Domain;

using BIStudio.Framework.Data;

namespace BIStudio.Framework.Tag.Internal
{
    /// <summary>
    /// 规约--操作数据
    /// </summary>
    public abstract class OperateSpecification<TEntity> : SpecificationBase<TEntity>
        where TEntity : Entity
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        protected abstract EnumSYSTagOperate OperateType { get; }
        /// <summary>
        /// 用户编号
        /// </summary>
        protected readonly int UserID;
        /// <summary>
        /// 当前用户可用权限
        /// </summary>
        protected List<SYSTagObjectDTO> authorities;

        protected OperateSpecification()
        {
            this.authorities = new SYSTagAuthorityBO()
                .GetAuthorityByCurrentUser(this.OperateType)
                .ToList<SYSTagObjectDTO>();
        }
        protected OperateSpecification(long userID)
        {
            this.authorities = new SYSTagAuthorityBO()
                .GetAuthorityByCurrentUser(this.OperateType, userID)
                .ToList<SYSTagObjectDTO>();
        }

        public bool IsSatisfiedBy<T>(T info)
        {
            if (info is SYSTagGroup)
            {
                return authorities.Exists(d => d.ObjectType == EnumSYSTagNodeType.TagGroup.ToString() && d.ObjectValue == (info as SYSTagGroup).ID);
            }
            if (info is SYSTagClass)
            {
                return authorities.Exists(d => d.ObjectType == EnumSYSTagNodeType.TagClass.ToString() && d.ObjectValue == (info as SYSTagClass).ID);
            }
            if (info is SYSTag)
            {
                return authorities.Exists(d => d.ObjectType == EnumSYSTagNodeType.Tag.ToString() && (info as SYSTag).ID == d.ObjectValue);
            }
            else
            {
                return true;
            }
        }


        public override Expression<Func<TEntity, bool>> Lambda
        {
            get
            {
                if (typeof(TEntity) == typeof(SYSTagGroup))
                {
                    return item => authorities.Any(d => d.ObjectType == EnumSYSTagNodeType.TagGroup.ToString() && d.ObjectValue == item.ID);
                }
                if (typeof(TEntity) == typeof(SYSTagClass))
                {
                    return item => authorities.Any(d => d.ObjectType == EnumSYSTagNodeType.TagClass.ToString() && d.ObjectValue == item.ID);
                }
                if (typeof(TEntity) == typeof(SYSTag))
                {
                    return item => authorities.Any(d => d.ObjectType == EnumSYSTagNodeType.Tag.ToString() && d.ObjectValue == item.ID);
                }
                else
                {
                    return item => true;
                }
            }
        }
        public override DBBuilder Sql
        {
            get
            {
                if (typeof(TEntity) == typeof(SYSTagGroup))
                {
                    return DBBuilder.Define().Eq(new { ObjectType = EnumSYSTagNodeType.TagGroup.ToString() }).And().Field("ObjectValue").Eq().Param("ObjectValue");
                }
                if (typeof(TEntity) == typeof(SYSTagClass))
                {
                    return DBBuilder.Define().Eq(new { ObjectType = EnumSYSTagNodeType.TagClass.ToString() }).And().Field("ObjectValue").Eq().Param("ObjectValue");
                }
                if (typeof(TEntity) == typeof(SYSTag))
                {
                    return DBBuilder.Define().Eq(new { ObjectType = EnumSYSTagNodeType.Tag.ToString() }).And().Field("ObjectValue").Eq().Param("ObjectValue");
                }
                else
                {
                    return DBBuilder.Define("1=1");
                }
            }
        }
    }
}
