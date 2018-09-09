namespace BIStudio.Framework.Institution
{
    using System;
    using BIStudio;    
    using BIStudio.Framework.Domain;
    using BIStudio.Framework.Data;

    /// <summary>
    ///  �����������˵����
    /// </summary>
    /// <remarks>
    /// �Ϻ�������Ϣ�������޹�˾[2009-8-25]
    /// </remarks>
    [Table("SYSDept")]
    public class SYSDept : Entity
    {
        /// <summary>
        /// ��λid
        /// </summary>
        public long? SystemID { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// �������Ƽ��
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// �ϼ�����id
        /// </summary>
        public long? ParentID { get; set; }

        /// <summary>
        /// �㼶 1��ʼ����
        /// </summary>
        public int? Layer { get; set; }

        /// <summary>
        /// ·��
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public int? Sequence { get; set; }

        /// <summary>
        /// �����ϼ��쵼id
        /// </summary>
        public long? LeaderID { get; set; }

        /// <summary>
        /// ���Ÿ�����
        /// </summary>
        public long? ManagerID { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// �Ƿ�ͣ�� 1���� 0 ͣ��
        /// </summary>
        public int? IsStop { get; set; }


        /// <summary>
        /// �Ƿ�λ 0���� 1��λ
        /// </summary>
        public int? IsUnit { get; set; }


    }

}

