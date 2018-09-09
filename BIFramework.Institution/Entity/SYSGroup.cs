﻿using System;
using BIStudio.Framework.Domain;
using BIStudio.Framework.Data;


namespace BIStudio.Framework.Institution
{
    /// <summary>
    /// 平台用户组
    /// </summary>
    [Table("SYSGroup")]
    public class SYSGroup : Entity
    {
        #region 数据属性
        /// <summary>
        ///  服务提供商ID
        /// </summary>
        public long? SystemID { get; set; }
        /// <summary>
        ///  服务提供商ID
        /// </summary>
        public long? AppID { get; set; }
        /// <summary>
        /// 用户组代码
        /// </summary>
        [Column(IsExact = true)]
        public string GroupCode { get; set; }
        /// <summary>
        /// 用户组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 用户组类型
        /// </summary>
        public string GroupType { get; set; }
        /// <summary>
        /// 用户组类型代码
        /// </summary>
        public long? GroupTypeID { get; set; }
        /// <summary>
        /// 所有者标志
        /// </summary>
        public string GroupFlag { get; set; }
        /// <summary>
        /// 所有者标志代码
        /// </summary>
        public long? GroupFlagID { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? Sequence { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Inputer { get; set; }
        /// <summary>
        /// 创建人的ID
        /// </summary>
        public long? InputerID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? InputTime { get; set; }
        #endregion

       
    }

}
