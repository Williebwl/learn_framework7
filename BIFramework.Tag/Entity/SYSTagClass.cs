using System;

namespace BIStudio.Framework.Tag
{
    using Data;
    using Domain;

    /// <summary>
    /// ��ǩ
    /// </summary>
    [Table("SYSTagClass")]
    public class SYSTagClass : Entity, IInputAudited, ITenantAudited
    {
        /// <summary>
        /// 
        /// </summary>
        public long? SystemID { get; set; }

        /// <summary>
        /// ��ǩ����
        /// </summary>
        public long? AppID { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// code��ʾ��
        /// </summary>
        public string ClassCode { get; set; }

        /// <summary>
        /// ��ǩ����
        /// </summary>
        public int? DisplayLevel { get; set; }

        /// <summary>
        /// ��ǩ�ȼ�
        /// </summary>
        public int? IsBuiltIn { get; set; }

        /// <summary>
        /// ���к�
        /// </summary>
        public int? Sequence { get; set; }

        /// <summary>
        /// ˵��
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public string Inputer { get; set; }

        /// <summary>
        /// �����˵�ID
        /// </summary>
        public long? InputerID { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime? InputTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClassValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Views { get; set; }
    }

}