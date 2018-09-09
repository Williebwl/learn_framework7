using System;

namespace BIStudio.Framework.Tag
{
    using Data;
    using Domain;

    /// <summary>
    /// ��ǩѡ��
    /// </summary>
    [Table("SYSTag")]
    public class SYSTag : Entity, IInputAudited, ITenantAudited
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
        /// ��ǩ��������
        /// </summary>
        public string TagClass { get; set; }

        /// <summary>
        /// ��ǩ����ID
        /// </summary>
        public long? TagClassID { get; set; }

        /// <summary>
        /// ��ǩ����
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// code��ʾ��
        /// </summary>
        public string TagCode { get; set; }

        /// <summary>
        /// ��ǩֵ��Χ,����(0~10000)
        /// </summary>
        public string TagValue { get; set; }

        /// <summary>
        /// ��ǩ��ȫ·��
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// ��ǰ���ڲ�
        /// </summary>
        public int? Layer { get; set; }

        /// <summary>
        /// �༶�����ǩʱ�ϼ���ǩid��Ĭ��Ϊ0
        /// </summary>
        public long? ParentID { get; set; }

        /// <summary>
        /// �Ƿ�Ҷ�ӣ�1��ʾ��Ҷ�ӣ�����Ϊ0��ֻ��û���ӽڵ�ʱ����Ҷ��
        /// </summary>
        public int? IsLeaf { get; set; }

        /// <summary>
        /// ��ǩ��ȼ�
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
        public string ComputedSequence { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Views { get; set; }
    }

}