using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BIStudio.Framework;

using BIStudio.Framework.Utils;

using BIStudio.Framework.Domain;
using BIStudio.Framework.Data;


namespace BIStudio.Framework.Tag
{
    internal class Tag : TransientDependency, ITag
    {
        #region 内部方法

        private ISpecification<Entity> specification = null;
        internal Tag(ISpecification<Entity> specification = null)
        {
            this.specification = specification;
        }

        /// <summary>
        /// 单位编号
        /// </summary>
        public long? SystemID { get; set; }

        private SYSTagNodeBO _tagNodeBO;
        /// <summary>
        /// 标签节点管理
        /// </summary>
        protected SYSTagNodeBO tagNodeBO
        {
            get { _tagNodeBO = _tagNodeBO ?? (this.SystemID.HasValue ? this.Context.Resolve<SYSTagNodeBO>().With(d => d.systemID = this.SystemID) : this.Context.Resolve<SYSTagNodeBO>()); return _tagNodeBO; }
        }

        private SYSTagGroupBO _tagGroupBO;
        /// <summary>
        /// 标签组管理
        /// </summary>
        protected SYSTagGroupBO tagGroupBO
        {
            get { _tagGroupBO = _tagGroupBO ?? (this.SystemID.HasValue ? this.Context.Resolve<SYSTagGroupBO>().With(d => d.systemID = this.SystemID) : this.Context.Resolve<SYSTagGroupBO>()); return _tagGroupBO; }
        }

        private SYSTagClassBO _tagClassBO;
        /// <summary>
        /// 标签管理
        /// </summary>
        protected SYSTagClassBO tagClassBO
        {
            get { _tagClassBO = _tagClassBO ?? (this.SystemID.HasValue ? this.Context.Resolve<SYSTagClassBO>().With(d => d.systemID = this.SystemID) : this.Context.Resolve<SYSTagClassBO>()); return _tagClassBO; }
        }

        private SYSTagBO _tagBO;
        /// <summary>
        /// 标签项管理
        /// </summary>
        protected SYSTagBO tagBO
        {
            get { _tagBO = _tagBO ?? (this.SystemID.HasValue ? this.Context.Resolve<SYSTagBO>().With(d => d.systemID = this.SystemID) : this.Context.Resolve<SYSTagBO>()); return _tagBO; }
        }

        private SYSTagTargetBO _tagTargetBO;
        /// <summary>
        /// 标签贴入对象管理
        /// </summary>
        protected SYSTagTargetBO tagTargetBO
        {
            get { _tagTargetBO = _tagTargetBO ?? (this.SystemID.HasValue ? this.Context.Resolve<SYSTagTargetBO>().With(d => d.systemID = this.SystemID) : this.Context.Resolve<SYSTagTargetBO>()); return _tagTargetBO; }
        }

        private SYSTagApplyBO _tagApplyBO;
        /// <summary>
        /// 标签贴入数据管理
        /// </summary>
        protected SYSTagApplyBO tagApplyBO
        {
            get { _tagApplyBO = _tagApplyBO ?? (this.SystemID.HasValue ? this.Context.Resolve<SYSTagApplyBO>().With(d => d.systemID = this.SystemID) : this.Context.Resolve<SYSTagApplyBO>()); return _tagApplyBO; }
        }

        private SYSTagAuthorityBO _tagAuthorityBO;
        /// <summary>
        /// 标签权限管理
        /// </summary>
        protected SYSTagAuthorityBO tagAuthorityBO
        {
            get { _tagAuthorityBO = _tagAuthorityBO ?? (this.SystemID.HasValue ? this.Context.Resolve<SYSTagAuthorityBO>().With(d => d.systemID = this.SystemID) : this.Context.Resolve<SYSTagAuthorityBO>()); return _tagAuthorityBO; }
        }

        private SYSTagLogsBO _tagLogsBO;
        /// <summary>
        /// 标签贴入日志管理
        /// </summary>
        protected SYSTagLogsBO tagLogsBO
        {
            get { _tagLogsBO = _tagLogsBO ?? (this.SystemID.HasValue ? this.Context.Resolve<SYSTagLogsBO>().With(d => d.systemID = this.SystemID) : this.Context.Resolve<SYSTagLogsBO>()); return _tagLogsBO; }
        }

        private TagSpecificationBO _tagSpecificationBO;
        /// <summary>
        /// 契约管理
        /// </summary>
        protected TagSpecificationBO tagSpecificationBO
        {
            get { _tagSpecificationBO = _tagSpecificationBO ?? this.Context.Resolve<TagSpecificationBO>(); return _tagSpecificationBO; }
        }

        #endregion

        #region 数据筛选

        /// <summary>
        /// 筛选符合规约的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="infos"></param>
        /// <returns></returns>
        public List<T> FindBySpecification<T>(List<T> infos) where T : Entity, new()
        {
            return this.specification == null || infos == null ? infos : infos.FindAll(info => this.specification.Lambda.Compile()(info));
        }
        /// <summary>
        /// 筛选符合规约的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public T FindBySpecification<T>(T info) where T : Entity, new()
        {
            return this.specification == null || info == null ? info : (this.specification.Lambda.Compile()(info) ? info : new T());
        }

        #endregion

        #region 查询标签类型
        private Dictionary<int, string> tagDisplayLevels = null;
        /// <summary>
        /// 返回全部标签类型
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> TagDisplayLevels
        {
            get
            {
                if (tagDisplayLevels == null)
                {
                    Dictionary<int, string> dict = new Dictionary<int, string>();

                    if (ALConfig.DllConfigs.IsExists("BITag") && ALConfig.DllConfigs["BITag"].IsExists("TagDisplayLevelConfig"))
                    {
                        foreach (var configItem in ALConfig.DllConfigs["BITag"]["TagDisplayLevelConfig"])
                        {
                            dict.Add(ALConvert.ToInt0(configItem.Attribute("key").Value), configItem.Attribute("value").Value);
                        }
                    }
                    else
                    {
                        foreach (var enumItem in ALEnumDescription.GetFieldTexts(typeof(EnumSYSTagDisplayLevel)))
                        {
                            dict.Add(enumItem.EnumValue, enumItem.EnumDisplayText);
                        }
                    }

                    tagDisplayLevels = dict;
                }
                return tagDisplayLevels;
            }
        }
        /// <summary>
        /// 返回指定标签类型的名称
        /// </summary>
        /// <param name="tagDisplayLevel"></param>
        /// <returns></returns>
        public string GetTagDisplayLevel(int tagDisplayLevel)
        {
            return TagDisplayLevels[tagDisplayLevel];
        }
        #endregion

        #region 查询标签节点
        /// <summary>
        /// 返回全部标签节点
        /// </summary>
        /// <param name="tagGroupCode">标签组代码</param>
        /// <returns></returns>
        public List<SYSTagNodeDTO> GetTagNodesByGroupCode(string tagGroupCode)
        {
            return this.GetTagNodesByGroupCode(tagGroupCode, EnumSYSTagNodeType.TagGroup | EnumSYSTagNodeType.TagClass | EnumSYSTagNodeType.Tag);
        }
        /// <summary>
        /// 返回全部标签节点
        /// </summary>
        /// <param name="tagGroupCode">标签组代码</param>
        /// <param name="nodeType">标签节点类型,使用OR逻辑连接多个查询条件</param>
        /// <returns></returns>
        public List<SYSTagNodeDTO> GetTagNodesByGroupCode(string tagGroupCode, EnumSYSTagNodeType nodeType)
        {
            return this.GetTagNodesByGroupCode(tagGroupCode, nodeType, false);
        }
        /// <summary>
        /// 返回全部标签节点
        /// </summary>
        /// <param name="tagGroupCode">标签组代码</param>
        /// <param name="nodeType">标签节点类型,使用OR逻辑连接多个查询条件</param>
        /// <param name="fixBrokenNodes">是否自动修复断开的节点</param>
        /// <returns></returns>
        public List<SYSTagNodeDTO> GetTagNodesByGroupCode(string tagGroupCode, EnumSYSTagNodeType nodeType, bool fixBrokenNodes)
        {
            return this.GetTagNodesByGroupCode(tagGroupCode, nodeType, fixBrokenNodes, null);
        }
        /// <summary>
        /// 返回全部标签节点
        /// </summary>
        /// <param name="tagGroupCode">标签组代码</param>
        /// <param name="nodeType">标签节点类型,使用OR逻辑连接多个查询条件</param>
        /// <param name="fixBrokenNodes">是否自动修复断开的节点</param>
        /// <param name="displayLevel">标签类型,EnumTagDisplayLevel</param>
        /// <returns></returns>
        public List<SYSTagNodeDTO> GetTagNodesByGroupCode(string tagGroupCode, EnumSYSTagNodeType nodeType, bool fixBrokenNodes, int? displayLevel)
        {
            List<SYSTagNodeDTO> nodes = new List<SYSTagNodeDTO>();
            //获得有效节点
            if ((nodeType | EnumSYSTagNodeType.TagGroup) == nodeType)
            {
                var tagGroupInfo = this.GetTagGroup(tagGroupCode);
                if (tagGroupInfo.ID.HasValue)
                    nodes.Add(SYSTagNodeDTO.Parse(tagGroupInfo));
            }
            if ((nodeType | EnumSYSTagNodeType.TagClass) == nodeType)
            {
                nodes.AddRange(this.GetTagClassesByGroupCode(tagGroupCode, displayLevel).Select(d => SYSTagNodeDTO.Parse(d)));
            }
            if ((nodeType | EnumSYSTagNodeType.Tag) == nodeType)
            {
                nodes.AddRange(this.GetTagsByGroupCode(tagGroupCode, displayLevel).Select(d => SYSTagNodeDTO.Parse(d)));
            }
            //修复断开节点
            if (fixBrokenNodes)
            {
                nodes = tagNodeBO.FixBrokenNodes(nodes);
            }
            return nodes;
        }
        #endregion

        #region 查询标签组
        /// <summary>
        /// 返回全部标签组
        /// </summary>
        /// <returns></returns>
        public List<SYSTagGroup> GetTagGroups()
        {
            return this.FindBySpecification(tagGroupBO.GetAll().OrderBy(d => d.Sequence).ToList());
        }

        /// <summary>
        /// 返回制定的标签组
        /// </summary>
        /// <param name="tagGroupID"></param>
        /// <returns></returns>
        public SYSTagGroup GetTagGroup(long tagGroupID)
        {
            return this.FindBySpecification(tagGroupBO.Get(tagGroupID));
        }

        /// <summary>
        /// 返回制定的标签组
        /// </summary>
        /// <param name="tagGroupCode"></param>
        /// <returns></returns>
        public SYSTagGroup GetTagGroup(string tagGroupCode)
        {
            return this.FindBySpecification(tagGroupBO.Get(new SYSTagGroup { GroupCode = tagGroupCode }.AsSpec()));
        }

        #endregion

        #region 查询标签
        /// <summary>
        /// 查询全部标签
        /// </summary>
        /// <returns></returns>
        public List<SYSTagClass> GetTagClasses()
        {
            return this.FindBySpecification(tagClassBO.GetTagClass().ToList<SYSTagClass>());
        }
        /// <summary>
        /// 返回指定的标签
        /// </summary>
        /// <param name="tagClassIDs"></param>
        /// <returns></returns>
        public List<SYSTagClass> GetTagClasses(List<long> tagClassIDs)
        {
            return this.FindBySpecification(tagClassBO.GetAll(new IDSpec<SYSTagClass>(tagClassIDs)).ToList());
        }
        /// <summary>
        /// 返回指定标签组内的全部标签
        /// </summary>
        /// <param name="tagGroupCode"></param>
        /// <returns></returns>
        public List<SYSTagClass> GetTagClassesByGroupCode(string tagGroupCode)
        {
            return this.FindBySpecification(this.GetTagClassesByGroupCode(tagGroupCode, null));
        }

        /// <summary>
        /// 返回指定标签组内的全部标签
        /// </summary>
        /// <param name="tagGroupCode"></param>
        /// <param name="displayLevel"></param>
        /// <returns></returns>
        public List<SYSTagClass> GetTagClassesByGroupCode(string tagGroupCode, int? displayLevel)
        {
            return this.FindBySpecification(tagClassBO.GetTagClass(tagGroupCode, displayLevel).ToList<SYSTagClass>());
        }

        /// <summary>
        /// 返回指定的标签
        /// </summary>
        /// <param name="tagClassID"></param>
        /// <returns></returns>
        public SYSTagClass GetTagClass(long tagClassID)
        {
            return this.FindBySpecification(tagClassBO.Get(tagClassID));
        }
        /// <summary>
        /// 返回指定的标签
        /// </summary>
        /// <param name="tagClassCode"></param>
        /// <returns></returns>
        public SYSTagClass GetTagClass(string tagClassCode)
        {
            return this.FindBySpecification(this.GetTagClass(new SYSTagClass { ClassCode = tagClassCode }));
        }

        public SYSTagClass GetTagClassByName(string name)
        {
            return this.FindBySpecification(this.GetTagClass(new SYSTagClass
            {
                ClassName = name
            }));
        }

        /// <summary>
        /// 返回指定的标签
        /// </summary>
        /// <param name="tagClassInfo"></param>
        /// <returns></returns>
        public SYSTagClass GetTagClass(SYSTagClass tagClassInfo)
        {
            return this.FindBySpecification(tagClassBO.Get(tagClassBO.GetIDByTagClass(tagClassInfo) ?? 0));
        }
        #endregion

        #region 查询标签项

        #region GetTags

        /// <summary>
        /// 返回全部标签项
        /// </summary>
        /// <returns></returns>
        public List<SYSTag> GetTags()
        {
            return this.FindBySpecification(tagBO.GetAll().ToList());
        }
        /// <summary>
        /// 返回多个指定的标签项
        /// </summary>
        /// <param name="tagIDs"></param>
        /// <returns></returns>
        public List<SYSTag> GetTags(IEnumerable<long> tagIDs)
        {
            if (tagIDs == null)
                return new List<SYSTag>();

            return this.FindBySpecification(tagBO.GetAll(new IDSpec<SYSTag>(tagIDs)).ToList());
        }

        #endregion

        #region GetTagsByParentID

        /// <summary>
        /// 返回指定标签的下级选项
        /// </summary>
        /// <param name="parentTagID"></param>
        /// <returns></returns>
        public List<SYSTag> GetTagsByParentID(long parentTagID)
        {
            return this.FindBySpecification(tagBO.GetTagsByParentID(parentTagID).ToList<SYSTag>());
        }

        #endregion

        #region GetTagsByGroupCode

        /// <summary>
        /// 返回指定标签组内的全部标签项
        /// </summary>
        /// <param name="tagGroupCode"></param>
        /// <returns></returns>
        public List<SYSTag> GetTagsByGroupCode(string tagGroupCode)
        {
            return this.FindBySpecification(this.GetTagsByGroupCode(tagGroupCode, null));
        }
        /// <summary>
        /// 返回指定标签组内的全部标签项
        /// </summary>
        /// <param name="tagGroupCode"></param>
        /// <param name="displayLevel"></param>
        /// <returns></returns>
        public List<SYSTag> GetTagsByGroupCode(string tagGroupCode, int? displayLevel)
        {
            return this.FindBySpecification(tagBO.GetTagClass(tagGroupCode, displayLevel).ToList<SYSTag>());
        }

        #endregion

        #region GetTagsByClassID

        /// <summary>
        /// 返回指定标签的全部选项
        /// </summary>
        /// <param name="tagClassID"></param>
        /// <returns></returns>
        public List<SYSTag> GetTagsByClassID(long tagClassID)
        {
            return this.FindBySpecification(tagBO.GetTagByTagClassID(tagClassID, (long?)null).ToList<SYSTag>());
        }
        /// <summary>
        /// 返回指定标签的下级选项
        /// </summary>
        /// <param name="tagClassID"></param>
        /// <param name="parentTagID"></param>
        /// <returns></returns>
        public List<SYSTag> GetTagsByClassID(long tagClassID, long parentTagID)
        {
            return this.FindBySpecification(tagBO.GetTagByTagClassID(tagClassID, parentTagID).ToList<SYSTag>());
        }
        /// <summary>
        /// 返回指定标签的下级选项
        /// </summary>
        /// <param name="tagClassID"></param>
        /// <param name="parentTagName"></param>
        /// <returns></returns>
        public List<SYSTag> GetTagsByClassID(long tagClassID, string parentTagName)
        {
            return this.FindBySpecification(tagBO.GetTagByTagClassID(tagClassID, parentTagName).ToList<SYSTag>());
        }

        #endregion

        #region GetTagsByClassCode

        /// <summary>
        /// 返回指定标签的全部选项
        /// </summary>
        /// <param name="tagClassCode"></param>
        /// <returns></returns>
        public List<SYSTag> GetTagsByClassCode(string tagClassCode)
        {
            return this.FindBySpecification(tagBO.GetTagByTagClassCode(tagClassCode).ToList<SYSTag>());
        }

        /// <summary>
        /// 返回指定标签的下级选项
        /// </summary>
        /// <param name="tagClassCode"></param>
        /// <param name="parentTagID"></param>
        /// <returns></returns>
        public List<SYSTag> GetTagsByClassCode(string tagClassCode, long parentTagID)
        {
            return this.FindBySpecification(tagBO.GetTagsByParentID(tagClassCode, parentTagID).ToList<SYSTag>());
        }
        /// <summary>
        /// 返回指定标签的下级选项
        /// </summary>
        /// <param name="tagClassCode"></param>
        /// <param name="parentTagName"></param>
        /// <returns></returns>
        public List<SYSTag> GetTagsByClassCode(string tagClassCode, string parentTagName)
        {
            return this.FindBySpecification(tagBO.GetTagsByParentID(tagClassCode, parentTagName).ToList<SYSTag>());
        }

        #endregion

        #region GetTag

        /// <summary>
        /// 返回指定的标签项
        /// </summary>
        /// <param name="tagID"></param>
        /// <returns></returns>
        public SYSTag GetTag(long tagID)
        {
            return this.FindBySpecification(tagBO.Get(tagID));
        }

        #endregion

        #region GetTagByName

        /// <summary>
        /// 返回指定的标签项
        /// </summary>
        /// <param name="tagClassCode">标签代码</param>
        /// <param name="tagName">标签项名称</param>
        /// <returns></returns>
        [Obsolete("使用GetTagByName替代")]
        public SYSTag GetTag(string tagClassCode, string tagName)
        {
            return this.GetTagByName(tagClassCode, tagName);
        }
        /// <summary>
        /// 返回指定的标签项
        /// </summary>
        /// <param name="tagClassID">标签编号</param>
        /// <param name="tagName">标签项名称</param>
        /// <returns></returns>
        [Obsolete("使用GetTagByName替代")]
        public SYSTag GetTag(long tagClassID, string tagName)
        {
            return this.GetTagByName(tagClassID, tagName);
        }

        /// <summary>
        /// 返回指定的标签项
        /// </summary>
        /// <param name="tagClassCode">标签代码</param>
        /// <param name="tagName">标签项名称</param>
        /// <returns></returns>
        public SYSTag GetTagByName(string tagClassCode, string tagName)
        {
            DataRow dr = tagBO.GetTagByTagClassCode(tagClassCode, tagName).AsEnumerable().FirstOrDefault();
            if (dr == null)
                return new SYSTag();
            else
                return this.FindBySpecification(dr.As<SYSTag>());
        }
        /// <summary>
        /// 返回指定的标签项
        /// </summary>
        /// <param name="tagClassID">标签编号</param>
        /// <param name="tagName">标签项名称</param>
        /// <returns></returns>
        public SYSTag GetTagByName(long tagClassID, string tagName)
        {
            return this.FindBySpecification(tagBO.Get(new SYSTag { TagClassID = tagClassID, TagName = tagName }.AsSpec()) ?? new SYSTag());
        }

        #endregion

        #region GetTagByCode

        /// <summary>
        /// 返回指定的标签项
        /// </summary>
        /// <param name="tagClassCode">标签代码</param>
        /// <param name="tagValue">标签项值</param>
        /// <returns></returns>
        [Obsolete("使用GetTagByCode替代")]
        public SYSTag GetTag(string tagClassCode, long tagCode)
        {
            return this.GetTagByCode(tagClassCode, tagCode.ToString());
        }

        /// <summary>
        /// 返回指定的标签项
        /// </summary>
        /// <param name="tagClassCode">标签代码</param>
        /// <param name="tagCode">标签项代码</param>
        /// <returns></returns>
        public SYSTag GetTagByCode(string tagClassCode, string tagCode)
        {
            var dt = tagBO.GetTagByTagClassCode(tagClassCode, tagCode, null);
            if (dt == null)
                return new SYSTag();
            DataRow dr = dt.AsEnumerable().FirstOrDefault();
            if (dr == null)
                return new SYSTag();
            else
                return this.FindBySpecification(dr.As<SYSTag>());
        }
        /// <summary>
        /// 返回指定的标签项
        /// </summary>
        /// <param name="tagClassID">标签编号</param>
        /// <param name="tagCode">标签项代码</param>
        /// <returns></returns>
        public SYSTag GetTagByCode(long tagClassID, string tagCode)
        {
            return this.FindBySpecification(tagBO.Get(new SYSTag { TagClassID = tagClassID, TagCode = tagCode }.AsSpec()) ?? new SYSTag());
        }

        #endregion

        #region GetTagByValue

        /// <summary>
        /// 返回指定的标签项
        /// </summary>
        /// <param name="tagClassID">标签编号</param>
        /// <param name="tagValue">标签项值</param>
        /// <returns></returns>
        [Obsolete("使用GetTagByValue替代")]
        public SYSTag GetTag(long tagClassID, int tagValue)
        {
            return this.GetTagByValue(tagClassID, tagValue.ToString());
        }

        /// <summary>
        /// 返回指定的标签项
        /// </summary>
        /// <param name="tagClassCode">标签代码</param>
        /// <param name="tagValue">标签项值</param>
        /// <returns></returns>
        public SYSTag GetTagByValue(string tagClassCode, string tagValue)
        {
            var dt = tagBO.GetTagByTagClassCode(tagClassCode, null, tagValue);
            if (dt == null)
                return new SYSTag();
            DataRow dr = dt.AsEnumerable().FirstOrDefault();
            if (dr == null)
                return new SYSTag();
            else
                return this.FindBySpecification(dr.As<SYSTag>());
        }
        /// <summary>
        /// 返回指定的标签项
        /// </summary>
        /// <param name="tagClassID">标签编号</param>
        /// <param name="tagValue">标签项值</param>
        /// <returns></returns>
        public SYSTag GetTagByValue(long tagClassID, string tagValue)
        {
            return this.FindBySpecification(tagBO.Get(new SYSTag { TagClassID = tagClassID, TagValue = tagValue }.AsSpec()) ?? new SYSTag());
        }

        #endregion

        #endregion

        #region 查询标签贴入
        /// <summary>
        /// 返回指定实体已贴入的标签项编号
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">贴入标签的实体，必须指定ID</param>
        /// <returns>已贴入实体的标签项编号</returns>
        public List<long> GetTagApplies<T>(T entity) where T : Entity, new()
        {
            long targetID = this.tagTargetBO[entity.Property.TableAttribute.TableName].ID.Value;
            long targetObjectID = ALConvert.ToLong0(entity.Property.GetValue(entity.Property.TableAttribute.PrimaryKey));

            if (this.specification == null)
                return tagApplyBO.GetTagIDValuesByTargetIDAndTargetObjectID(targetID, targetObjectID);
            else
            {
                return this.FindBySpecification(tagApplyBO.GetTagsByTargetIDAndTargetObjectID(targetID, targetObjectID, null, null, null)
                .ToList<SYSTag>()).Select(d => d.ID.Value).ToList();
            }
        }
        /// <summary>
        /// 返回指定实体已贴入的标签项
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">贴入标签的实体，必须指定ID</param>
        /// <returns>已贴入实体的标签项</returns>
        public List<SYSTag> GetTagApplyInfos<T>(T entity) where T : Entity, new()
        {
            long targetID = this.tagTargetBO[entity.Property.TableAttribute.TableName].ID.Value;
            long targetObjectID = ALConvert.ToLong0(entity.ID);

            return this.FindBySpecification(tagApplyBO.GetTagsByTargetIDAndTargetObjectID(targetID, targetObjectID, null, null, null)
                .ToList<SYSTag>());
        }
        /// <summary>
        /// 返回指定实体已贴入的标签项
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">贴入标签的实体，需指定ID属性</param>
        /// <param name="tagClassInfo">查询条件，需要指定ID或ClassCode或ClassName属性</param>
        /// <returns>已贴入实体的标签项</returns>
        public List<SYSTag> GetTagApplyInfos<T>(T entity, SYSTagClass tagClassInfo) where T : Entity, new()
        {
            long targetID = this.tagTargetBO[entity.Property.TableAttribute.TableName].ID.Value;
            long targetObjectID = ALConvert.ToLong0(entity.Property.GetValue(entity.Property.TableAttribute.PrimaryKey));
            long? tagClassID = tagClassBO.GetIDByTagClass(tagClassInfo);

            return this.FindBySpecification(tagApplyBO.GetTagsByTargetIDAndTargetObjectID(targetID, targetObjectID, tagClassID, null, null)
                .ToList<SYSTag>());
        }
        /// <summary>
        /// 返回指定实体已贴入的标签项
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">贴入标签的实体，需要指定ID属性</param>
        /// <param name="tagGroupInfo">查询条件，需要指定ID或GroupCode或GroupName属性</param>
        /// <returns>已贴入实体的标签项</returns>
        public List<SYSTag> GetTagApplyInfos<T>(T entity, SYSTagGroup tagGroupInfo) where T : Entity, new()
        {
            long targetID = this.tagTargetBO[entity.Property.TableAttribute.TableName].ID.Value;
            long targetObjectID = ALConvert.ToLong0(entity.Property.GetValue(entity.Property.TableAttribute.PrimaryKey));
            long? tagGroupID = tagGroupBO.GetIDByTagGroup(tagGroupInfo);

            return this.FindBySpecification(tagApplyBO.GetTagsByTargetIDAndTargetObjectID(targetID, targetObjectID, null, tagGroupID, null)
                .ToList<SYSTag>());
        }
        /// <summary>
        /// 返回指定实体已贴入的标签项
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">贴入标签的实体，需要指定ID属性</param>
        /// <param name="tagGroupInfo">查询条件，需要指定ID或GroupCode或GroupName属性</param>
        /// <param name="displayLevel">标签类型</param>
        /// <returns>已贴入实体的标签项</returns>
        public List<SYSTag> GetTagApplyInfos<T>(T entity, SYSTagGroup tagGroupInfo, EnumSYSTagDisplayLevel? displayLevel) where T : Entity, new()
        {
            return GetTagApplyInfos(entity, tagGroupInfo, (int?)displayLevel);
        }
        /// <summary>
        /// 返回指定实体已贴入的标签项
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">贴入标签的实体，需要指定ID属性</param>
        /// <param name="tagGroupInfo">查询条件，需要指定ID或GroupCode或GroupName属性</param>
        /// <param name="displayLevel">标签类型</param>
        /// <returns>已贴入实体的标签项</returns>
        public List<SYSTag> GetTagApplyInfos<T>(T entity, SYSTagGroup tagGroupInfo, int? displayLevel) where T : Entity, new()
        {
            long targetID = this.tagTargetBO[entity.Property.TableAttribute.TableName].ID.Value;
            long targetObjectID = ALConvert.ToLong0(entity.Property.GetValue(entity.Property.TableAttribute.PrimaryKey));
            long? tagGroupID = tagGroupBO.GetIDByTagGroup(tagGroupInfo);

            return this.FindBySpecification(tagApplyBO.GetTagsByTargetIDAndTargetObjectID(targetID, targetObjectID, null, tagGroupID, displayLevel)
                .ToList<SYSTag>());
        }
        /// <summary>
        /// 返回指定实体已贴入的标签日志
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tagInfo">标签，需要指定ID属性</param>
        /// <param name="entity">贴入标签的实体，需要指定ID属性</param>
        /// <returns></returns>
        public List<SYSTagLogs> GetTagLogs<T>(SYSTag tagInfo, T entity) where T : Entity, new()
        {
            //long targetID = this.tagTargetBO[entity.TableAttribute.TableName].ID.Value;
            long targetObjectID = ALConvert.ToLong0(entity.Property.GetValue(entity.Property.TableAttribute.PrimaryKey));

            return this.FindBySpecification(tagLogsBO.GetAll(new SYSTagLogs
            {
                TagID = tagInfo.ID,
                TagName = tagInfo.TagName,
                //标签日志暂时不区分数据表 TargetObject = targetID.ToString(),
                TargetObjectID = targetObjectID
            }.AsSpec()).ToList());
        }
        /// <summary>
        /// 返回指定实体已贴入的标签组日志
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tagClassInfo">查询条件，需要指定ID或ClassCode或ClassName属性</param>
        /// <param name="entity">贴入标签的实体，需要指定ID属性</param>
        /// <returns></returns>
        public List<SYSTagLogs> GetTagLogs<T>(SYSTagClass tagClassInfo, T entity) where T : Entity, new()
        {
            //int targetID = this.tagTargetBO[entity.TableAttribute.TableName].ID.Value;
            long targetObjectID = ALConvert.ToLong0(entity.Property.GetValue(entity.Property.TableAttribute.PrimaryKey));
            long? tagClassID = tagClassBO.GetIDByTagClass(tagClassInfo);

            return this.FindBySpecification(tagLogsBO.GetAll(new SYSTagLogs
            {
                TagClassID = tagClassID.Value,
                //标签日志暂时不区分数据表 TargetObject = targetID.ToString(),
                TargetObjectID = targetObjectID
            }.AsSpec()).ToList());
        }
        /// <summary>
        /// 返回指定条件的标签日志
        /// </summary>
        /// <param name="tagLogsInfo"></param>
        /// <returns></returns>
        public List<SYSTagLogs> GetTagLogs(SYSTagLogs tagLogsInfo)
        {
            return this.FindBySpecification(tagLogsBO.GetAll(tagLogsInfo.AsSpec()).ToList());
        }
        #endregion

        #region 标签权限
        /// <summary>
        /// 返回指定的标签权限
        /// </summary>
        /// <param name="tagAuthorityInfo"></param>
        /// <returns></returns>
        public SYSTagAuthority GetTagAuthority(long tagAuthorityID)
        {
            return this.tagAuthorityBO.Get(tagAuthorityID);
        }
        /// <summary>
        /// 返回指定的标签权限
        /// </summary>
        /// <param name="tagAuthorityInfo"></param>
        /// <returns></returns>
        public List<SYSTagAuthority> GetTagAuthorities(SYSTagAuthority tagAuthorityInfo)
        {
            return this.tagAuthorityBO.GetAll(tagAuthorityInfo.AsSpec()).ToList();
        }
        /// <summary>
        /// 授予标签权限
        /// </summary>
        /// <param name="dto"></param>
        public void SetTagAuthority(SYSTagConferAuthorityDTO dto)
        {
            this.tagAuthorityBO.SetTagAuthority(dto);
        }
        /// <summary>
        /// 授予标签权限
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="conferOrRevoke">是授予权限True,还是取消权限False</param>
        public void SetTagAuthority(SYSTagConferAuthorityDTO dto, bool conferOrRevoke)
        {
            this.tagAuthorityBO.SetTagAuthority(dto, conferOrRevoke);
        }
        #endregion

        #region 修复断开的标签节点
        /// <summary>
        /// 修复断开的节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public List<SYSTagNodeDTO> FixBrokenNodes(List<SYSTagNodeDTO> nodes)
        {
            return tagNodeBO.FixBrokenNodes(nodes);
        }
        /// <summary>
        /// 修复断开的节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public List<SYSTagNodeDTO> FixBrokenNodes(List<SYSTagGroup> nodes)
        {
            return tagNodeBO.FixBrokenNodes(nodes.Select(d => SYSTagNodeDTO.Parse(d)).ToList());
        }
        /// <summary>
        /// 修复断开的节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public List<SYSTagNodeDTO> FixBrokenNodes(List<SYSTagClass> nodes)
        {
            return tagNodeBO.FixBrokenNodes(nodes.Select(d => SYSTagNodeDTO.Parse(d)).ToList());
        }
        /// <summary>
        /// 修复断开的节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public List<SYSTagNodeDTO> FixBrokenNodes(List<SYSTag> nodes)
        {
            return tagNodeBO.FixBrokenNodes(nodes.Select(d => SYSTagNodeDTO.Parse(d)).ToList());
        }
        #endregion

        #region 更新

        /// <summary>
        /// 删除指定的标签，标签选项，标签日志，标签Apply数据
        /// </summary>
        /// <param name="tagClassInfo">查询条件，需要指定ID或ClassCode或ClassName属性</param>
        /// <returns></returns>
        public void Delete(SYSTagClass tagClassInfo)
        {
            long? tagClassID = tagClassBO.GetIDByTagClass(tagClassInfo);
            //删除标签选项
            tagBO.GetTagByTagClassID(tagClassInfo.ID.ToString())
                .ToList<SYSTag>().ForEach(d => this.Delete(d));
            //删除标签
            tagClassBO.Remove(tagClassID.Value);
            tagBO.DeleteTagByTagClass(tagClassID.ToString());
        }
        /// <summary>
        /// 删除指定的标签项，子级标签项，标签日志，标签Apply数据
        /// </summary>
        /// <param name="tagInfo">查询条件，需要指定ID或TagCode或TagName属性</param>
        public void Delete(SYSTag tagInfo)
        {
            long? tagID = tagBO.GetIDByTag(tagInfo);
            //删除下级选项
            tagBO.GetTagsByParentID(tagID.Value)
                .ToList<SYSTag>().ForEach(d => this.Delete(d));
            //删除标签选项
            tagLogsBO.DeleteByTagID(tagID.Value);
            tagApplyBO.DeleteByTagID(tagID.Value);
            tagBO.Remove(tagID.Value);
        }
        /// <summary>
        /// 删除指定的标签权限
        /// </summary>
        /// <param name="tagAuthorityInfo"></param>
        public void Delete(SYSTagAuthority tagAuthorityInfo)
        {
            long? authorityID = tagAuthorityInfo.ID;
            tagAuthorityBO.Remove(authorityID ?? 0);
        }

        /// <summary>
        /// 保存指定的标签组
        /// </summary>
        /// <param name="tagGroupInfo"></param>
        public void Save(SYSTagGroup tagGroupInfo)
        {
            (tagGroupInfo.ID.HasValue ? (Func<SYSTagGroup, bool>)tagGroupBO.Modify : tagGroupBO.Add)(tagGroupInfo);
        }
        /// <summary>
        /// 保存指定的标签
        /// </summary>
        /// <param name="tagClassInfo"></param>
        public void Save(SYSTagClass tagClassInfo)
        {
            tagClassBO.Save(tagClassInfo);
        }
        /// <summary>
        /// 保存指定的标签项
        /// </summary>
        /// <param name="tagInfo"></param>
        public void Save(SYSTag tagInfo)
        {
            tagBO.Save(tagInfo);
        }
        /// <summary>
        /// 保存指定的标签权限
        /// </summary>
        /// <param name="tagAuthorityInfo"></param>
        public void Save(SYSTagAuthority tagAuthorityInfo)
        {
            (tagAuthorityInfo.ID.HasValue ? (Func<SYSTagAuthority, bool>)tagAuthorityBO.Modify : tagAuthorityBO.Add)(tagAuthorityInfo);
        }
        /// <summary>
        /// 保存标签日志
        /// </summary>
        /// <param name="tagLogsInfo"></param>
        public void Save(SYSTagLogs[] tagLogsInfo)
        {
            foreach (var info in tagLogsInfo)
                (info.ID.HasValue ? (Func<SYSTagLogs, bool>)tagLogsBO.Modify : tagLogsBO.Add)(info);
        }
        /// <summary>
        /// 更新标签项排序
        /// </summary>
        /// <param name="idAndSequence"></param>
        public void UpdateTagSequence(Dictionary<long, int> idAndSequence)
        {
            tagBO.UpdateSequence(idAndSequence);
        }
        /// <summary>
        /// 更新标签排序
        /// </summary>
        /// <param name="idAndSequence"></param>
        public void UpdateTagClassSequence(Dictionary<long, int> idAndSequence)
        {
            foreach (var kv in idAndSequence)
                tagClassBO.Modify(new SYSTagClass { ID = kv.Key, Sequence = kv.Value });
        }
        /// <summary>
        /// 更新标签组排序
        /// </summary>
        /// <param name="idAndSequence"></param>
        public void UpdateTagGroupSequence(Dictionary<long, int> idAndSequence)
        {
            foreach (var kv in idAndSequence)
                tagGroupBO.Modify(new SYSTagGroup { ID = kv.Key, Sequence = kv.Value });
        }

        #endregion

        #region 贴入标签

        /// <summary>
        /// 将指定的标签项贴入实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tagInfo">标签，需要指定ID属性</param>
        /// <param name="entity">要贴入标签的实体，需要指定ID属性</param>
        /// <returns></returns>
        public ITag Apply<T>(SYSTag tagInfo, T entity) where T : Entity, new()
        {
            tagApplyBO.Save(new SYSTagApply
            {
                TagID = tagInfo.ID,
                TargetID = tagTargetBO[entity.Property.TableAttribute.TableName].ID,
                TargetObjectID = ALConvert.ToLong(entity.Property.GetValue(entity.Property.TableAttribute.PrimaryKey))
            });
            return this;
        }
        /// <summary>
        /// 将指定的标签项批量贴入实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tagInfos"></param>
        /// <param name="entity">要贴入标签的实体，需要指定ID属性</param>
        public ITag Apply<T>(List<SYSTag> tagInfos, T entity) where T : Entity, new()
        {
            List<SYSTagApply> tagApplyInfo = new List<SYSTagApply>();
            tagInfos.ForEach(d =>
            {
                tagApplyInfo.Add(new SYSTagApply
                {
                    TagID = d.ID,
                    TargetID = tagTargetBO[entity.Property.TableAttribute.TableName].ID,
                    TargetObjectID = ALConvert.ToLong(entity.Property.GetValue(entity.Property.TableAttribute.PrimaryKey))
                });
            });
            tagApplyBO.Save(tagApplyInfo);
            return this;
        }
        /// <summary>
        /// 将指定的标签项批量贴入实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tagIDs"></param>
        /// <param name="entity">要贴入标签的实体，需要指定ID属性</param>
        public ITag Apply<T>(IEnumerable<long> tagIDs, T entity) where T : Entity, new()
        {
            return this.Apply<T>(tagIDs.Select(d => new SYSTag { ID = d }).ToList(), entity);
        }
        /// <summary>
        /// 将指定的标签项贴入多个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tagInfo">标签，需要指定ID属性</param>
        /// <param name="entity">要贴入标签的实体，需要指定ID属性</param>
        public ITag Apply<T>(SYSTag tagInfo, List<T> entity) where T : Entity, new()
        {
            List<SYSTagApply> tagApplyInfo = new List<SYSTagApply>();
            entity.ForEach(d =>
            {
                tagApplyInfo.Add(new SYSTagApply
                {
                    TagID = tagInfo.ID,
                    TargetID = tagTargetBO[d.Property.TableAttribute.TableName].ID,
                    TargetObjectID = ALConvert.ToLong(d.Property.GetValue(d.Property.TableAttribute.PrimaryKey))
                });
            });
            tagApplyBO.Save(tagApplyInfo);
            return this;
        }

        #region 待定
        ///// <summary>
        ///// 删除已贴入实体的标签项
        ///// </summary>
        ///// <typeparam name="T">标签贴入对象</typeparam>
        ///// <param name="pkid">标签贴入对象的主键值</param>
        ///// <param name="tagID">标签值</param>
        ///// <returns>是否操作成功</returns>
        //public ITag UnApply<T>(int pkid, int tagID) where T : Entity
        //{
        //    TableAttribute table = typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault() as TableAttribute;
        //    if (table != null && !string.IsNullOrEmpty(table.TableName) && !string.IsNullOrEmpty(table.PrimaryKey))
        //    {
        //        if (table.TableName == "Company" && companyTagBO.CompatibleMode)
        //        {
        //            companyTagBO.DeleteByCompanyIDAndTagID(pkid, tagID) > 0;
        //        }
        //        else
        //        {
        //            tagApplyBO.DeleteByCompanyIDAndTagID(tagTargetBO[table.TableName].ID.Value, pkid, tagID) > 0;
        //        }
        //    }
        //    return this;
        //}

        ///// <summary>
        ///// 删除已贴入实体的全部标签项
        ///// </summary>
        ///// <typeparam name="T">标签贴入对象</typeparam>
        ///// <param name="pkid">标签贴入对象的主键值</param>
        ///// <returns>是否操作成功</returns>
        //public ITag UnApply<T>(long pkid) where T : Entity
        //{
        //    TableAttribute table = typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault() as TableAttribute;
        //    if (table != null && !string.IsNullOrEmpty(table.TableName) && !string.IsNullOrEmpty(table.PrimaryKey))
        //    {
        //        if (table.TableName == "Company" && companyTagBO.CompatibleMode)
        //        {
        //            companyTagBO.DeleteByCompanyID(pkid) > 0;
        //        }
        //        else
        //        {
        //            tagApplyBO.DeleteByCompanyID(tagTargetBO[table.TableName].ID.Value, pkid) > 0;
        //        }
        //    }
        //    return this;
        //}
        #endregion

        /// <summary>
        /// 删除已贴入实体的标签项
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">要取消贴入标签的实体，需要指定ID属性</param>
        /// <param name="tagInfo">查询条件，需要指定ID或TagCode或TagName属性</param>
        public ITag UnApply<T>(T entity, SYSTag tagInfo) where T : Entity, new()
        {
            long? tagID = tagBO.GetIDByTag(tagInfo);
            tagApplyBO.DeleteByCompanyIDAndTagID(
                tagTargetBO[entity.Property.TableAttribute.TableName].ID.Value,
                ALConvert.ToLong0(entity.Property.GetValue("ID")),
                tagID.Value);
            return this;
        }

        /// <summary>
        /// 删除已贴入实体的标签
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">要取消贴入标签的实体，需要指定ID属性</param>
        /// <param name="tagClassInfo">查询条件，需要指定ID或ClassCode或ClassName属性</param>
        /// <returns></returns>
        public ITag UnApply<T>(T entity, SYSTagClass tagClassInfo) where T : Entity, new()
        {
            long? tagClassID = tagClassBO.GetIDByTagClass(tagClassInfo);
            tagApplyBO.DeleteByCompanyIDAndTagClassID(
                tagTargetBO[entity.Property.TableAttribute.TableName].ID.Value,
                ALConvert.ToLong0(entity.Property.GetValue("ID")),
                tagClassID.Value);
            return this;
        }

        /// <summary>
        /// 删除已贴入实体的标签
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">要取消贴入标签的实体，需要指定ID属性</param>
        /// <param name="tagClassIDs">标签ID</param>
        /// <returns></returns>
        public ITag UnApply<T>(T entity, IEnumerable<long> tagClassIDs) where T : Entity, new()
        {
            foreach (long tagClassID in tagClassIDs)
            {
                tagApplyBO.DeleteByCompanyIDAndTagClassID(
                    tagTargetBO[entity.Property.TableAttribute.TableName].ID.Value,
                    ALConvert.ToLong0(entity.Property.GetValue("ID")),
                    tagClassID);
            }
            return this;
        }

        /// <summary>
        /// 删除已贴入实体的标签
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">要取消贴入标签的实体，需要指定ID属性</param>
        /// <param name="tagGroupInfo">查询条件，需要指定ID或GroupCode或GroupName属性</param>
        /// <returns></returns>
        public ITag UnApply<T>(T entity, SYSTagGroup tagGroupInfo) where T : Entity, new()
        {
            long? tagGroupID = tagGroupBO.GetIDByTagGroup(tagGroupInfo);
            tagApplyBO.DeleteByCompanyIDAndTagGroupID(
                tagTargetBO[entity.Property.TableAttribute.TableName].ID.Value,
                ALConvert.ToLong0(entity.Property.GetValue("ID")),
                tagGroupID.Value);
            return this;
        }

        /// <summary>
        /// 删除已贴入实体的全部标签项
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">要取消贴入标签的实体，需要指定ID属性</param>
        public ITag UnApply<T>(T entity) where T : Entity, new()
        {
            tagApplyBO.DeleteByCompanyID(tagTargetBO[entity.Property.TableAttribute.TableName].ID.Value, ALConvert.ToLong0(entity.Property.GetValue("ID")));
            return this;
        }
        #endregion

        #region 搜索
        /// <summary>
        /// 设置查询主表
        /// </summary>
        /// <param name="tableName">数据表名</param>
        public SYSTagSearchBO CreateSearch(string tableName)
        {
            var bo = new SYSTagSearchBO(tableName);
            bo.DependOn(this.Context);
            return bo;
        }
        /// <summary>
        /// 设置查询主表，并筛选出已贴入指定标签项的数据
        /// </summary>
        /// <param name="tableName">数据表名</param>
        /// <param name="tagIDs">标签项ID列表</param>
        public SYSTagSearchBO CreateSearch(string tableName, params long[] tagIDs)
        {
            var bo = new SYSTagSearchBO(tableName, tagIDs);
            bo.DependOn(this.Context);
            return bo;
        }
        /// <summary>
        /// 设置查询主表，并筛选出已贴入指定标签项的数据
        /// </summary>
        /// <param name="tableName">数据表名</param>
        /// <param name="matchType">匹配方式 EnumTagSearch</param>
        /// <param name="tagIDs">标签项ID列表</param>
        public SYSTagSearchBO CreateSearch(string tableName, EnumSYSTagSearch matchType, params long[] tagIDs)
        {
            var bo = new SYSTagSearchBO(tableName, matchType, tagIDs);
            bo.DependOn(this.Context);
            return bo;
        }
        /// <summary>
        /// 返回指定表名的贴入对象编号
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public SYSTagTarget CreateTarget(string tableName)
        {
            return tagTargetBO[tableName];
        }
        #endregion

        #region 契约管理
        /// <summary>
        /// 获得指定类型的契约，如果目标包含多个类型，则使用AND连接
        /// </summary>
        /// <param name="operates"></param>
        /// <returns></returns>
        public ISpecification<TEntity> GetSpecification<TEntity>(EnumSYSTagOperate? operates) where TEntity : Entity, new()
        {
            return this.tagSpecificationBO.GetSpecification<TEntity>(operates, null);
        }
        #endregion

    }
}
