namespace BIStudio.Framework.Tenant
{
    using BIStudio.Framework.Domain;
    using BIStudio.Framework.Data;

    /// <summary>
    /// ģ��
    /// </summary>
    [Table("SYSMenu")]
    public class SYSMenu : Entity, ITenantAudited
    {
        /// <summary>
        ///  �����ṩ��ID
        /// </summary>
        public long? SystemID { get; set; }

        /// <summary>
        ///  �����ṩ��ID
        /// </summary>
        public long? AppID { get; set; }

        /// <summary>
        /// ģ������
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// ģ����
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MenuCode { get; set; }

        /// <summary>
        /// ���ڵ�ID
        /// </summary>
        public long? ParentID { get; set; }

        /// <summary>
        /// ���ڵڼ��㣬��һ��Ϊ1�����5��
        /// </summary>
        public int? Layer { get; set; }

        /// <summary>
        /// ·������ʽΪ,1,2,3,4,5,
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// ͼ��
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// ͼ�걳��
        /// </summary>
        public string IconBackGround { get; set; }

        /// <summary>
        /// ��Ҫ˵��
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public int? Sequence { get; set; }

        /// <summary>
        /// �Ƿ���ʾ
        /// </summary>
        public bool? IsShow { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string NavUrl { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public string ToolBarUrl { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string ContainerUrl { get; set; }

        /// <summary>
        /// ·�ɱ�ʾ
        /// </summary>
        public string PageRoute { get; set; }

        /// <summary>
        /// ��ʾģʽ : 0:Ĭ�� ; 1:��ҳ�� ;2:�Զ��嵼�� ;3: �����˵�����
        /// </summary>
        public int? DisplayModeID { get; set; }

        /// <summary>
        /// ��ʾģʽ��Ĭ�� / ��ҳ�� / �Զ��嵼�� / �����˵�����
        /// </summary>
        public string DisplayMode { get; set; }

        /// <summary>
        /// �Ƿ񵯳�
        /// </summary>
        public bool? IsPopUp { get; set; }

        /// <summary>
        /// �ⲿ��ַ
        /// </summary>
        public string OutsideUrl { get; set; }
    }
}