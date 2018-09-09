using System;
using System.Collections.Generic;
using BIStudio.Framework.Domain;


namespace BIStudio.Framework.Tag
{
    public interface ITag : ITransientDependency
    {
        /// <summary>
        /// 单位编号
        /// </summary>
        long? SystemID { get; set; }

        #region 数据筛选

        /// <summary>
        ///     筛选符合规约的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="infos"></param>
        /// <returns></returns>
        List<T> FindBySpecification<T>(List<T> infos) where T : Entity, new();

        /// <summary>
        ///     筛选符合规约的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        T FindBySpecification<T>(T info) where T : Entity, new();

        #endregion

        #region 查询标签类型

        /// <summary>
        ///     返回全部标签类型
        /// </summary>
        /// <returns></returns>
        Dictionary<int, string> TagDisplayLevels { get; }

        /// <summary>
        ///     返回指定标签类型的名称
        /// </summary>
        /// <param name="tagDisplayLevel"></param>
        /// <returns></returns>
        string GetTagDisplayLevel(int tagDisplayLevel);

        #endregion

        #region 查询标签节点

        /// <summary>
        ///     返回全部标签节点
        /// </summary>
        /// <param name="tagGroupCode">标签组代码</param>
        /// <returns></returns>
        List<SYSTagNodeDTO> GetTagNodesByGroupCode(string tagGroupCode);

        /// <summary>
        ///     返回全部标签节点
        /// </summary>
        /// <param name="tagGroupCode">标签组代码</param>
        /// <param name="nodeType">标签节点类型,使用OR逻辑连接多个查询条件</param>
        /// <returns></returns>
        List<SYSTagNodeDTO> GetTagNodesByGroupCode(string tagGroupCode, EnumSYSTagNodeType nodeType);

        /// <summary>
        ///     返回全部标签节点
        /// </summary>
        /// <param name="tagGroupCode">标签组代码</param>
        /// <param name="nodeType">标签节点类型,使用OR逻辑连接多个查询条件</param>
        /// <param name="fixBrokenNodes">是否自动修复断开的节点</param>
        /// <returns></returns>
        List<SYSTagNodeDTO> GetTagNodesByGroupCode(string tagGroupCode, EnumSYSTagNodeType nodeType, bool fixBrokenNodes);

        /// <summary>
        ///     返回全部标签节点
        /// </summary>
        /// <param name="tagGroupCode">标签组代码</param>
        /// <param name="nodeType">标签节点类型,使用OR逻辑连接多个查询条件</param>
        /// <param name="fixBrokenNodes">是否自动修复断开的节点</param>
        /// <param name="displayLevel">标签类型,EnumTagDisplayLevel</param>
        /// <returns></returns>
        List<SYSTagNodeDTO> GetTagNodesByGroupCode(string tagGroupCode, EnumSYSTagNodeType nodeType, bool fixBrokenNodes,
            int? displayLevel);

        #endregion

        #region 查询标签组

        /// <summary>
        ///     返回全部标签组
        /// </summary>
        /// <returns></returns>
        List<SYSTagGroup> GetTagGroups();

        /// <summary>
        ///     返回制定的标签组
        /// </summary>
        /// <param name="tagGroupID"></param>
        /// <returns></returns>
        SYSTagGroup GetTagGroup(long tagGroupID);

        /// <summary>
        ///     返回制定的标签组
        /// </summary>
        /// <param name="tagGroupCode"></param>
        /// <returns></returns>
        SYSTagGroup GetTagGroup(string tagGroupCode);

        #endregion

        #region 查询标签

        /// <summary>
        ///     返回全部标签
        /// </summary>
        /// <returns></returns>
        List<SYSTagClass> GetTagClasses();

        /// <summary>
        ///     返回指定的标签
        /// </summary>
        /// <param name="tagClassIDs"></param>
        /// <returns></returns>
        List<SYSTagClass> GetTagClasses(List<long> tagClassIDs);

        /// <summary>
        ///     返回指定标签组内的全部标签
        /// </summary>
        /// <param name="tagGroupCode"></param>
        /// <returns></returns>
        List<SYSTagClass> GetTagClassesByGroupCode(string tagGroupCode);

        /// <summary>
        ///     返回指定标签组内的全部标签
        /// </summary>
        /// <param name="tagGroupCode"></param>
        /// <param name="displayLevel"></param>
        /// <returns></returns>
        List<SYSTagClass> GetTagClassesByGroupCode(string tagGroupCode, int? displayLevel);

        /// <summary>
        ///     返回指定的标签
        /// </summary>
        /// <param name="tagClassID"></param>
        /// <returns></returns>
        SYSTagClass GetTagClass(long tagClassID);

        /// <summary>
        ///     返回指定的标签
        /// </summary>
        /// <param name="tagClassCode"></param>
        /// <returns></returns>
        SYSTagClass GetTagClass(string tagClassCode);

        /// <summary>
        ///     返回指定的标签
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        SYSTagClass GetTagClassByName(string name);

        /// <summary>
        ///     返回指定的标签
        /// </summary>
        /// <param name="tagClassInfo"></param>
        /// <returns></returns>
        SYSTagClass GetTagClass(SYSTagClass tagClassInfo);

        #endregion

        #region 查询标签项

        #region GetTags

        /// <summary>
        ///     返回全部标签项
        /// </summary>
        /// <returns></returns>
        List<SYSTag> GetTags();

        /// <summary>
        ///     返回多个指定的标签项
        /// </summary>
        /// <param name="tagIDs"></param>
        /// <returns></returns>
        List<SYSTag> GetTags(IEnumerable<long> tagIDs);

        #endregion

        #region GetTagsByParentID

        /// <summary>
        ///     返回指定标签的下级选项
        /// </summary>
        /// <param name="parentTagID"></param>
        /// <returns></returns>
        List<SYSTag> GetTagsByParentID(long parentTagID);

        #endregion

        #region GetTagsByGroupCode

        /// <summary>
        ///     返回指定标签组内的全部标签项
        /// </summary>
        /// <param name="tagGroupName"></param>
        /// <returns></returns>
        List<SYSTag> GetTagsByGroupCode(string tagGroupName);

        /// <summary>
        ///     返回指定标签组内的全部标签项
        /// </summary>
        /// <param name="tagGroupName"></param>
        /// <param name="displayLevel"></param>
        /// <returns></returns>
        List<SYSTag> GetTagsByGroupCode(string tagGroupName, int? displayLevel);

        #endregion

        #region GetTagsByClassID

        /// <summary>
        ///     返回指定标签的全部选项
        /// </summary>
        /// <param name="tagClassID"></param>
        /// <returns></returns>
        List<SYSTag> GetTagsByClassID(long tagClassID);

        /// <summary>
        ///     返回指定标签的下级选项
        /// </summary>
        /// <param name="tagClassID"></param>
        /// <param name="parentTagID"></param>
        /// <returns></returns>
        List<SYSTag> GetTagsByClassID(long tagClassID, long parentTagID);

        /// <summary>
        ///     返回指定标签的下级选项
        /// </summary>
        /// <param name="tagClassID"></param>
        /// <param name="parentTagName"></param>
        /// <returns></returns>
        List<SYSTag> GetTagsByClassID(long tagClassID, string parentTagName);

        #endregion

        #region GetTagsByClassCode

        /// <summary>
        ///     返回指定标签的全部选项
        /// </summary>
        /// <param name="tagClassCode"></param>
        /// <returns></returns>
        List<SYSTag> GetTagsByClassCode(string tagClassCode);

        /// <summary>
        ///     返回指定标签的下级选项
        /// </summary>
        /// <param name="tagClassCode"></param>
        /// <param name="parentTagID"></param>
        /// <returns></returns>
        List<SYSTag> GetTagsByClassCode(string tagClassCode, long parentTagID);

        /// <summary>
        ///     返回指定标签的下级选项
        /// </summary>
        /// <param name="tagClassCode"></param>
        /// <param name="parentTagName"></param>
        /// <returns></returns>
        List<SYSTag> GetTagsByClassCode(string tagClassCode, string parentTagName);

        #endregion

        #region GetTag

        /// <summary>
        ///     返回指定的标签项
        /// </summary>
        /// <param name="tagID"></param>
        /// <returns></returns>
        SYSTag GetTag(long tagID);

        #endregion

        #region GetTagByName

        /// <summary>
        ///     返回指定的标签项
        /// </summary>
        /// <param name="tagClassCode">标签代码</param>
        /// <param name="tagName">标签项名称</param>
        /// <returns></returns>
        [Obsolete("使用GetTagByName替代")]
        SYSTag GetTag(string tagClassCode, string tagName);

        /// <summary>
        ///     返回指定的标签项
        /// </summary>
        /// <param name="tagClassID">标签编号</param>
        /// <param name="tagName">标签项名称</param>
        /// <returns></returns>
        [Obsolete("使用GetTagByName替代")]
        SYSTag GetTag(long tagClassID, string tagName);

        /// <summary>
        ///     返回指定的标签项
        /// </summary>
        /// <param name="tagClassCode">标签代码</param>
        /// <param name="tagName">标签项名称</param>
        /// <returns></returns>
        SYSTag GetTagByName(string tagClassCode, string tagName);

        /// <summary>
        ///     返回指定的标签项
        /// </summary>
        /// <param name="tagClassID">标签编号</param>
        /// <param name="tagName">标签项名称</param>
        /// <returns></returns>
        SYSTag GetTagByName(long tagClassID, string tagName);

        #endregion

        #region GetTagByCode

        /// <summary>
        ///     返回指定的标签项
        /// </summary>
        /// <param name="tagClassCode">标签代码</param>
        /// <param name="tagValue">标签项值</param>
        /// <returns></returns>
        [Obsolete("使用GetTagByCode替代")]
        SYSTag GetTag(string tagClassCode, long tagCode);

        /// <summary>
        ///     返回指定的标签项
        /// </summary>
        /// <param name="tagClassCode">标签代码</param>
        /// <param name="tagCode">标签项代码</param>
        /// <returns></returns>
        SYSTag GetTagByCode(string tagClassCode, string tagCode);

        /// <summary>
        ///     返回指定的标签项
        /// </summary>
        /// <param name="tagClassID">标签编号</param>
        /// <param name="tagCode">标签项代码</param>
        /// <returns></returns>
        SYSTag GetTagByCode(long tagClassID, string tagCode);

        #endregion

        #region GetTagByValue

        /// <summary>
        ///     返回指定的标签项
        /// </summary>
        /// <param name="tagClassID">标签编号</param>
        /// <param name="tagValue">标签项值</param>
        /// <returns></returns>
        [Obsolete("使用GetTagByValue替代")]
        SYSTag GetTag(long tagClassID, int tagValue);

        /// <summary>
        ///     返回指定的标签项
        /// </summary>
        /// <param name="tagClassCode">标签代码</param>
        /// <param name="tagValue">标签项值</param>
        /// <returns></returns>
        SYSTag GetTagByValue(string tagClassCode, string tagValue);

        /// <summary>
        ///     返回指定的标签项
        /// </summary>
        /// <param name="tagClassID">标签编号</param>
        /// <param name="tagValue">标签项值</param>
        /// <returns></returns>
        SYSTag GetTagByValue(long tagClassID, string tagValue);

        #endregion

        #endregion

        #region 查询标签贴入

        /// <summary>
        ///     返回指定实体已贴入的标签项编号
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">贴入标签的实体，需要指定ID属性</param>
        /// <returns>已贴入实体的标签项编号</returns>
        List<long> GetTagApplies<T>(T entity) where T : Entity, new();

        /// <summary>
        ///     返回指定实体已贴入的标签项
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">贴入标签的实体，需要指定ID属性</param>
        /// <returns>已贴入实体的标签项</returns>
        List<SYSTag> GetTagApplyInfos<T>(T entity) where T : Entity, new();

        /// <summary>
        ///     返回指定实体已贴入的标签项
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">贴入标签的实体，需要指定ID属性</param>
        /// <param name="tagClassInfo">查询条件，需要指定ID或ClassCode或ClassName属性</param>
        /// <returns>已贴入实体的标签项</returns>
        List<SYSTag> GetTagApplyInfos<T>(T entity, SYSTagClass tagClassInfo) where T : Entity, new();

        /// <summary>
        ///     返回指定实体已贴入的标签项
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">贴入标签的实体，需要指定ID属性</param>
        /// <param name="tagGroupInfo">查询条件，需要指定ID或GroupCode或GroupName属性</param>
        /// <returns>已贴入实体的标签项</returns>
        List<SYSTag> GetTagApplyInfos<T>(T entity, SYSTagGroup tagGroupInfo) where T : Entity, new();

        /// <summary>
        ///     返回指定实体已贴入的标签项
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">贴入标签的实体，需要指定ID属性</param>
        /// <param name="tagGroupInfo">查询条件，需要指定ID或GroupCode或GroupName属性</param>
        /// <param name="displayLevel">标签类型</param>
        /// <returns>已贴入实体的标签项</returns>
        List<SYSTag> GetTagApplyInfos<T>(T entity, SYSTagGroup tagGroupInfo, EnumSYSTagDisplayLevel? displayLevel)
            where T : Entity, new();

        /// <summary>
        ///     返回指定实体已贴入的标签项
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">贴入标签的实体，需要指定ID属性</param>
        /// <param name="tagGroupInfo">查询条件，需要指定ID或GroupCode或GroupName属性</param>
        /// <param name="displayLevel">标签类型</param>
        /// <returns>已贴入实体的标签项</returns>
        List<SYSTag> GetTagApplyInfos<T>(T entity, SYSTagGroup tagGroupInfo, int? displayLevel)
            where T : Entity, new();

        /// <summary>
        ///     返回指定实体已贴入的标签日志
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tagInfo">标签，需要指定ID属性</param>
        /// <param name="entity">贴入标签的实体，需要指定ID属性</param>
        /// <returns></returns>
        List<SYSTagLogs> GetTagLogs<T>(SYSTag tagInfo, T entity) where T : Entity, new();

        /// <summary>
        ///     返回指定实体已贴入的标签组日志
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tagClassInfo">查询条件，需要指定ID或ClassCode或ClassName属性</param>
        /// <param name="entity">贴入标签的实体，需要指定ID属性</param>
        /// <returns></returns>
        List<SYSTagLogs> GetTagLogs<T>(SYSTagClass tagClassInfo, T entity) where T : Entity, new();

        /// <summary>
        ///     返回指定条件的标签日志
        /// </summary>
        /// <param name="tagLogsInfo"></param>
        /// <returns></returns>
        List<SYSTagLogs> GetTagLogs(SYSTagLogs tagLogsInfo);

        #endregion

        #region 查询标签权限

        /// <summary>
        ///     返回指定的标签权限
        /// </summary>
        /// <param name="tagAuthorityID"></param>
        /// <returns></returns>
        SYSTagAuthority GetTagAuthority(long tagAuthorityID);

        /// <summary>
        ///     返回指定的标签权限
        /// </summary>
        /// <param name="tagAuthorityInfo"></param>
        /// <returns></returns>
        List<SYSTagAuthority> GetTagAuthorities(SYSTagAuthority tagAuthorityInfo);

        /// <summary>
        ///     授予标签权限
        /// </summary>
        /// <param name="dto"></param>
        void SetTagAuthority(SYSTagConferAuthorityDTO dto);

        /// <summary>
        ///     授予标签权限
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="conferOrRevoke">是授予权限True,还是取消权限False</param>
        void SetTagAuthority(SYSTagConferAuthorityDTO dto, bool conferOrRevoke);

        #endregion

        #region 修复断开的标签节点

        /// <summary>
        ///     修复断开的节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        List<SYSTagNodeDTO> FixBrokenNodes(List<SYSTagNodeDTO> nodes);

        /// <summary>
        ///     修复断开的节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        List<SYSTagNodeDTO> FixBrokenNodes(List<SYSTagGroup> nodes);

        /// <summary>
        ///     修复断开的节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        List<SYSTagNodeDTO> FixBrokenNodes(List<SYSTagClass> nodes);

        /// <summary>
        ///     修复断开的节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        List<SYSTagNodeDTO> FixBrokenNodes(List<SYSTag> nodes);

        #endregion

        #region 更新

        /// <summary>
        ///     删除指定的标签，标签选项，标签日志，标签Apply数据
        /// </summary>
        /// <param name="tagClassInfo">查询条件，需要指定ID或ClassCode或ClassName属性</param>
        /// <returns></returns>
        void Delete(SYSTagClass tagClassInfo);

        /// <summary>
        ///     删除指定的标签项，子级标签项，标签日志，标签Apply数据
        /// </summary>
        /// <param name="tagInfo">查询条件，需要指定ID或TagCode或TagName属性</param>
        void Delete(SYSTag tagInfo);

        /// <summary>
        ///     删除指定的标签权限
        /// </summary>
        /// <param name="tagAuthorityInfo"></param>
        void Delete(SYSTagAuthority tagAuthorityInfo);

        /// <summary>
        ///     保存指定的标签组
        /// </summary>
        /// <param name="tagGroupInfo"></param>
        void Save(SYSTagGroup tagGroupInfo);

        /// <summary>
        ///     保存指定的标签
        /// </summary>
        /// <param name="tagClassInfo"></param>
        void Save(SYSTagClass tagClassInfo);

        /// <summary>
        ///     保存指定的标签项
        /// </summary>
        /// <param name="tagInfo"></param>
        void Save(SYSTag tagInfo);

        /// <summary>
        ///     保存指定的标签权限
        /// </summary>
        /// <param name="tagAuthorityInfo"></param>
        void Save(SYSTagAuthority tagAuthorityInfo);

        /// <summary>
        ///     保存标签日志
        /// </summary>
        /// <param name="tagLogsInfo"></param>
        void Save(SYSTagLogs[] tagLogsInfo);

        /// <summary>
        ///     更新标签项排序
        /// </summary>
        /// <param name="idAndSequence"></param>
        void UpdateTagSequence(Dictionary<long, int> idAndSequence);

        /// <summary>
        ///     更新标签排序
        /// </summary>
        /// <param name="idAndSequence"></param>
        void UpdateTagClassSequence(Dictionary<long, int> idAndSequence);

        /// <summary>
        ///     更新标签组排序
        /// </summary>
        /// <param name="idAndSequence"></param>
        void UpdateTagGroupSequence(Dictionary<long, int> idAndSequence);
        
        #endregion

        #region 贴入标签

        /// <summary>
        ///     将指定的标签项贴入实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tagInfo">标签，需要指定ID属性</param>
        /// <param name="entity">要贴入标签的实体，需要指定ID属性</param>
        /// <returns></returns>
        ITag Apply<T>(SYSTag tagInfo, T entity) where T : Entity, new();

        /// <summary>
        ///     将指定的标签项批量贴入实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tagInfo">标签，需要指定ID属性</param>
        /// <param name="entity">要贴入标签的实体，需要指定ID属性</param>
        /// <returns></returns>
        ITag Apply<T>(List<SYSTag> tagInfo, T entity) where T : Entity, new();

        /// <summary>
        ///     将指定的标签项批量贴入实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tagInfos"></param>
        /// <param name="entity">要贴入标签的实体，需要指定ID属性</param>
        ITag Apply<T>(IEnumerable<long> tagIDs, T entity) where T : Entity, new();

        /// <summary>
        ///     将指定的标签项贴入多个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tagInfo">标签，需要指定ID属性</param>
        /// <param name="entity">要贴入标签的实体，需要指定ID属性</param>
        ITag Apply<T>(SYSTag tagInfo, List<T> entity) where T : Entity, new();

        /// <summary>
        ///     删除已贴入实体的标签项
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">要取消贴入标签的实体，需要指定ID属性</param>
        /// <param name="tagInfo">查询条件，需要指定ID或TagCode或TagName属性</param>
        ITag UnApply<T>(T entity, SYSTag tagInfo) where T : Entity, new();

        /// <summary>
        ///     删除已贴入实体的标签
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">要取消贴入标签的实体，需要指定ID属性</param>
        /// <param name="tagClassInfo">查询条件，需要指定ID或ClassCode或ClassName属性</param>
        /// <returns></returns>
        ITag UnApply<T>(T entity, SYSTagClass tagClassInfo) where T : Entity, new();

        /// <summary>
        ///     删除已贴入实体的标签
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">要取消贴入标签的实体，需要指定ID属性</param>
        /// <param name="tagClassIDs">标签ID</param>
        /// <returns></returns>
        ITag UnApply<T>(T entity, IEnumerable<long> tagClassIDs) where T : Entity, new();

        /// <summary>
        ///     删除已贴入实体的标签
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">要取消贴入标签的实体，需要指定ID属性</param>
        /// <param name="tagGroupInfo">查询条件，需要指定ID或GroupCode或GroupName属性</param>
        /// <returns></returns>
        ITag UnApply<T>(T entity, SYSTagGroup tagGroupInfo) where T : Entity, new();

        /// <summary>
        ///     删除已贴入实体的全部标签项
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">要取消贴入标签的实体，需要指定ID属性</param>
        ITag UnApply<T>(T entity) where T : Entity, new();

        #endregion

        #region 搜索

        /// <summary>
        ///     设置查询主表
        /// </summary>
        /// <param name="tableName">数据表名</param>
        SYSTagSearchBO CreateSearch(string tableName);

        /// <summary>
        ///     设置查询主表，并筛选出已贴入指定标签项的数据
        /// </summary>
        /// <param name="tableName">数据表名</param>
        /// <param name="tagIDs">标签项ID列表</param>
        SYSTagSearchBO CreateSearch(string tableName, params long[] tagIDs);

        /// <summary>
        ///     设置查询主表，并筛选出已贴入指定标签项的数据
        /// </summary>
        /// <param name="tableName">数据表名</param>
        /// <param name="matchType">匹配方式 EnumTagSearch</param>
        /// <param name="tagIDs">标签项ID列表</param>
        SYSTagSearchBO CreateSearch(string tableName, EnumSYSTagSearch matchType, params long[] tagIDs);

        /// <summary>
        ///     返回指定表名的贴入对象编号
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        SYSTagTarget CreateTarget(string tableName);

        #endregion

        #region 契约管理

        /// <summary>
        ///     获得指定类型的契约，如果目标包含多个类型，则使用AND连接
        /// </summary>
        /// <param name="operates"></param>
        /// <returns></returns>
        ISpecification<T> GetSpecification<T>(EnumSYSTagOperate? operates) where T : Entity, new();

        #endregion
    }

}