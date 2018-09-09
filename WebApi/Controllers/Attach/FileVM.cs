using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using BIStudio.Framework.File;
using BIStudio.Framework.UI;

namespace WebApi.Controllers.Attach
{

    /// <summary>
    /// 文件信息
    /// </summary>
    public class FileVM : ViewModel
    {
        /// <summary>
        /// 文件标示
        /// </summary>
        public long? ID { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }
        
    }
}