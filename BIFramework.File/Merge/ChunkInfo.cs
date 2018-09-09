using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.File
{
    /// <summary>
    /// 文件分块信息
    /// </summary>
    public class ChunkInfo : FileInfo
    {
        /// <summary>
        /// 分块序号
        /// </summary>
        public int? Chunk { get; set; }

        /// <summary>
        /// 分块大小
        /// </summary>
        public int ChunkSize { get; set; }

        /// <summary>
        /// 分块数量
        /// </summary>
        public int? Chunks { get; set; }

        /// <summary>
        /// 已经合并的数量
        /// </summary>
        internal int Merged { get; set; }

        /// <summary>
        /// 分块资源流
        /// </summary>
        public string ChunkUrl { get; set; }

        /// <summary>
        /// 分块资源流
        /// </summary>
        public Stream ChunkStream { get; set; }
    }
}
