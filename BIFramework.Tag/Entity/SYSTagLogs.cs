namespace BIStudio.Framework.Tag
{
	using System;
	using System.Text;
	using BIStudio;
    using BIStudio.Framework.Domain;
    using BIStudio.Framework.Data;
    
    
	/// <summary>
	///  在这里添加类说明。
	/// </summary>
	/// <remarks>
	/// 上海拜特信息技术有限公司[2011/5/12]
    /// </remarks>
    [Table("SYSTagLogs")]
    public class SYSTagLogs : Entity, ITenantAudited
    {
        /// <summary>
        /// 单位ID
        /// </summary>
        public long? SystemID { get; set; }

		/// <summary>
		/// 标签名称
		/// </summary>
		public string TagName { get; set; }

		/// <summary>
		/// 标签ID
		/// </summary>
		public long? TagID { get; set; }

		/// <summary>
		/// 标签类型名称
		/// </summary>
		public string TagClass { get; set; }

		/// <summary>
		/// 标签类型ID
		/// </summary>
		public long? TagClassID { get; set; }

		/// <summary>
		/// 目标对象的名称
		/// </summary>
		public string TargetObject { get; set; }

		/// <summary>
		/// 目标对象的ID
		/// </summary>
		public long? TargetObjectID { get; set; }

		/// <summary>
		/// 登记人的姓名
		/// </summary>
		public string Inputer { get; set; }

		/// <summary>
		/// 登记时间
		/// </summary>
		public DateTime? InputTime { get; set; }

		/// <summary>
		/// 登记人主机的IP地址
		/// </summary>
		public string InputIP { get; set; }

		/// <summary>
		/// 1.新增 2.删除 3.修改
		/// </summary>
        public int? OperateFlag { get; set; }
	}
}