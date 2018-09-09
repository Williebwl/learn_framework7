namespace BIStudio.Framework.Tag
{
	using System;
	using System.Text;
	using BIStudio;
    using BIStudio.Framework.Domain;
    using BIStudio.Framework.Data;
    
    
	/// <summary>
	///  �����������˵����
	/// </summary>
	/// <remarks>
	/// �Ϻ�������Ϣ�������޹�˾[2011/5/12]
    /// </remarks>
    [Table("SYSTagLogs")]
    public class SYSTagLogs : Entity, ITenantAudited
    {
        /// <summary>
        /// ��λID
        /// </summary>
        public long? SystemID { get; set; }

		/// <summary>
		/// ��ǩ����
		/// </summary>
		public string TagName { get; set; }

		/// <summary>
		/// ��ǩID
		/// </summary>
		public long? TagID { get; set; }

		/// <summary>
		/// ��ǩ��������
		/// </summary>
		public string TagClass { get; set; }

		/// <summary>
		/// ��ǩ����ID
		/// </summary>
		public long? TagClassID { get; set; }

		/// <summary>
		/// Ŀ����������
		/// </summary>
		public string TargetObject { get; set; }

		/// <summary>
		/// Ŀ������ID
		/// </summary>
		public long? TargetObjectID { get; set; }

		/// <summary>
		/// �Ǽ��˵�����
		/// </summary>
		public string Inputer { get; set; }

		/// <summary>
		/// �Ǽ�ʱ��
		/// </summary>
		public DateTime? InputTime { get; set; }

		/// <summary>
		/// �Ǽ���������IP��ַ
		/// </summary>
		public string InputIP { get; set; }

		/// <summary>
		/// 1.���� 2.ɾ�� 3.�޸�
		/// </summary>
        public int? OperateFlag { get; set; }
	}
}