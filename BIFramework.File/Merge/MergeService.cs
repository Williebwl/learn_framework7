using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using io = System.IO;

namespace BIStudio.Framework.File
{
    /// <summary>
    /// 合并文件
    /// </summary>
    public class MergeService
    {
        /// <summary>
        /// 合并文件
        /// </summary>
        /// <param name="chunk">需要合并的文件片段信息</param>
        /// <returns>合并后的文件信息</returns>
        public static FileInfo Merge(ChunkInfo chunk)
        {
            return MergeProvider.Factory.Merge(chunk);
        }

        /// <summary>
        /// 根据文件信息获取文件流
        /// </summary>
        /// <param name="info">文件信息</param>
        /// <returns>文件流</returns>
        public static io.Stream GetFileStream(FileInfo info)
        {
            if (info == null) throw new ArgumentNullException("文件信息不能为空！");

            if (string.IsNullOrEmpty(info.CacheFileFullName) || !io.File.Exists(info.CacheFileFullName)) throw new ArgumentException("该文件信息无效！");

            return io.File.OpenRead(info.CacheFileFullName);
        }

        /// <summary>
        /// 根据文件信息移动文件到指定目录
        /// </summary>
        /// <param name="info">文件信息</param>
        /// <param name="destFileName">目标目录</param>
        public static void Move(FileInfo info, string destFileName)
        {
            if (info == null) throw new ArgumentNullException("文件信息不能为空！");

            if (string.IsNullOrEmpty(info.CacheFileFullName) || !io.File.Exists(info.CacheFileFullName)) throw new ArgumentException("该文件信息无效！");

            io.File.Move(info.CacheFileFullName, destFileName);
        }
    }
}
