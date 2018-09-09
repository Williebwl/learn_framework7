using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIStudio.Framework.UI;

namespace WebApi.Controllers.Attach
{
    /// <summary>
    /// 附件上传结果
    /// </summary>
    public class AttachResultVM : ViewModel
    {
        /// <summary>
        /// 附件标示
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 附件上传状态 0：成功；其它值则为失败
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 文件信息
        /// </summary>
        public IList<FileVM> Files { get; set; }
    }
}