using BIStudio.Framework.UI;

namespace WebApi.Controllers.Tag
{
    public class TagGroupVM : ViewModel
    {
        /// <summary>
        /// DataEntity主键
        /// </summary>
        public long? ID { get; set; }

        /// <summary>
        /// 标签组名称,必须
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 标签组前缀,必须
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
        public int? Sequence { get; set; }

        /// <summary>
        /// 访问频率
        /// </summary>
        public int? Views { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}