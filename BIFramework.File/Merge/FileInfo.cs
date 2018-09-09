using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.File
{
    /// <summary>
    /// 文件信息
    /// </summary>
    public class FileInfo
    {
        /// <summary>
        /// 附件标示
        /// </summary>
        public string AttachKey { get; set; }

        /// <summary>
        /// 上传标示
        /// </summary>
        public string UploadKey { get; set; }

        /// <summary>
        /// 文件标示
        /// </summary>
        public string FileID { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public int FileSize { get; set; }

        /// <summary>
        /// 文件最后修改时间
        /// </summary>
        public DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// 文件缓存路径
        /// </summary>
        internal string CacheFileFullName { get; set; }
    }
}
