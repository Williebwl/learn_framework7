using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BIStudio.Framework.Data;
using BIStudio.Framework.Tag.Internal;
using BIStudio.Framework.Utils;
using BIStudio.Framework.Domain;


namespace BIStudio.Framework.Tag
{
    /// <summary>
    /// 标签权限管理
    /// </summary>
    internal class SYSTagAuthorityBO : SYSTagBase<SYSTagAuthority>
    {
        /// <summary>
        /// 得到当前用户可用权限
        /// </summary>
        /// <param name="tagOperate"></param>
        /// <returns></returns>
        internal DataTable GetAuthorityByCurrentUser(EnumSYSTagOperate tagOperate)
        {
            return GetAuthorityByCurrentUser(tagOperate, CFContext.User.ID);
        }
        /// <summary>
        /// 得到指定用户可用权限
        /// </summary>
        /// <param name="tagOperate"></param>
        /// <returns></returns>
        internal DataTable GetAuthorityByCurrentUser(EnumSYSTagOperate tagOperate, long userID)
        {
            return this.UnitOfWork.ToDataTable(DBBuilder.Define(SYSTagAuthoritySql.GetAuthorityByCurrentUserSql, new
                {
                    CurrentUserID = userID,
                    AcceptOperate = tagOperate,
                    SystemID = systemID,
                }));
        }
        /// <summary>
        /// 授予标签权限
        /// </summary>
        /// <param name="dto"></param>
        internal void SetTagAuthority(SYSTagConferAuthorityDTO dto)
        {
            SetTagAuthority(dto, true);
        }
        
        /// <summary>
        /// 授予标签权限
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="conferOrRevoke"></param>
        /// <returns></returns>
        internal void SetTagAuthority(SYSTagConferAuthorityDTO dto, bool conferOrRevoke)
        {
            foreach (var tagInfo in this.getInfo(dto, conferOrRevoke))
            {
                if (conferOrRevoke)
                    (tagInfo.ID.HasValue ? (Func<SYSTagAuthority, bool>)this.Modify : this.Add)(tagInfo);
                else
                    this.Remove(tagInfo.ID.Value);
            }
        }
        /// <summary>
        /// 解析授予的权限
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="conferOrRevoke"></param>
        /// <returns></returns>
        private SYSTagAuthority[] getInfo(SYSTagConferAuthorityDTO dto, bool conferOrRevoke)
        {
            List<SYSTagAuthority> infos = new List<SYSTagAuthority>();
            foreach (var kv in dto.TargetContent)
            {
                foreach (var node in getNodes(dto))
                {
                    SYSTagAuthority info = new SYSTagAuthority();
                    info.AuthorityType = dto.TargetType.ToString();
                    info.AuthorityText = kv.AuthorityText;
                    info.AuthorityValue = kv.AuthorityValue;
                    info.ObjectType = node.NodeType.ToString();
                    info.ObjectText = node.Name;
                    info.ObjectValue = node.ID;
                    info.AcceptOperate = (int)dto.AuthorityOperate;
                    if (conferOrRevoke)
                    {
                        //授予权限时排除现有权限
                        if (this.GetAll(info.AsSpec()).Count() == 0)
                        {
                            info.InputTime = DateTime.Now;
                            info.Inputer = CFContext.User.UserName;
                            info.InputerID = CFContext.User.ID;
                            info.Remark = dto.Remark;
                            infos.Add(info);
                        }
                    }
                    else
                    {
                        infos.AddRange(this.GetAll(info.AsSpec()));
                    }
                }
            }
            return infos.ToArray();
        }
        /// <summary>
        /// 解析被授权的标签
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private SYSTagNodeDTO[] getNodes(SYSTagConferAuthorityDTO dto)
        {
            ITag tag = TagService.GetInstance();
            tag.DependOn(this.Context);

            List<SYSTagNodeDTO> dtos = new List<SYSTagNodeDTO>();
            if (dto.TagNode.NodeType==EnumSYSTagNodeType.Tag)
            {
                //新增标签项权限
                if ((dto.AuthorityRange | EnumSYSTagRange.Parents) == dto.AuthorityRange)
                {
                    var tagInfo = tag.GetTag(dto.TagNode.ID.Value);
                    var tagclassInfo = tag.GetTagClass(tagInfo.TagClassID.Value);
                    var taggroupInfo = tag.GetTagGroup(tagclassInfo.AppID.Value);

                    dtos.Add(SYSTagNodeDTO.Parse(taggroupInfo));
                    dtos.Add(SYSTagNodeDTO.Parse(tagclassInfo));
                    //返回当前标签项的父级节点
                    ALConvert.ToList<int>(tagInfo.Path)
                        .FindAll(d => d != 0 && d != tagInfo.ID)
                        .ForEach(d => dtos.Add(SYSTagNodeDTO.Parse(tag.GetTag(d))));
                }
                if ((dto.AuthorityRange | EnumSYSTagRange.Current) == dto.AuthorityRange)
                {
                    dtos.Add(SYSTagNodeDTO.Parse(tag.GetTag(dto.TagNode.ID.Value)));
                }
                if ((dto.AuthorityRange | EnumSYSTagRange.Children) == dto.AuthorityRange)
                {
                    tag.GetTagsByParentID(dto.TagNode.ID.Value)
                        .ForEach(d => dtos.Add(SYSTagNodeDTO.Parse(d)));
                }
            }
            else if (dto.TagNode.NodeType == EnumSYSTagNodeType.TagClass)
            {
                //新增标签权限
                if ((dto.AuthorityRange | EnumSYSTagRange.Parents) == dto.AuthorityRange)
                {
                    dtos.Add(SYSTagNodeDTO.Parse(tag.GetTagGroup(dto.TagNode.ParentID.Value)));
                }
                if ((dto.AuthorityRange | EnumSYSTagRange.Current) == dto.AuthorityRange)
                {
                    dtos.Add(SYSTagNodeDTO.Parse(tag.GetTagClass(dto.TagNode.ID.Value)));
                }
                if ((dto.AuthorityRange | EnumSYSTagRange.Children) == dto.AuthorityRange)
                {
                    tag.GetTagsByClassID(dto.TagNode.ID.Value)
                        .ForEach(d => dtos.Add(SYSTagNodeDTO.Parse(d)));
                }
            }
            else if (dto.TagNode.NodeType == EnumSYSTagNodeType.TagGroup)
            {
                //新增标签组权限
                if ((dto.AuthorityRange | EnumSYSTagRange.Current) == dto.AuthorityRange)
                {
                    dtos.Add(SYSTagNodeDTO.Parse(tag.GetTagGroup(dto.TagNode.ID.Value)));
                }
                if ((dto.AuthorityRange | EnumSYSTagRange.Children) == dto.AuthorityRange)
                {
                    tag.GetTagClassesByGroupCode(dto.TagNode.Code, (int?)dto.TagDisplayLevel)
                        .ForEach(d => dtos.Add(SYSTagNodeDTO.Parse(d)));
                    tag.GetTagsByGroupCode(dto.TagNode.Code, (int?)dto.TagDisplayLevel)
                        .ForEach(d => dtos.Add(SYSTagNodeDTO.Parse(d)));
                }
            }
            return dtos.ToArray();
        }

    }
}
