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
    internal class MergeProvider
    {
        private static MergeProviderFactory _factory;

        internal static MergeProviderFactory Factory { get { return _factory ?? (_factory = new MergeProviderFactory()); } }

        protected string _url;

        /// <summary>
        /// 合并文件
        /// </summary>
        /// <param name="url">文件缓存地址</param>
        internal MergeProvider(string url)
        {
            _url = url;
        }

        /// <summary>
        /// 合并文件
        /// </summary>
        /// <param name="chunk">文件分段信息</param>
        /// <returns>文件分段信息</returns>
        internal virtual ChunkInfo Merge(ChunkInfo chunk)
        {
            if (string.IsNullOrEmpty(chunk.ChunkUrl) || !io.File.Exists(chunk.ChunkUrl)) throw new io.FileNotFoundException("没有找到文件分段！");

            using (var fileStream = new io.FileStream(chunk.CacheFileFullName = io.Path.Combine(_url, chunk.FileID), io.FileMode.OpenOrCreate))
            using (var stream = chunk.ChunkStream ?? io.File.OpenRead(chunk.ChunkUrl))
            {
                if (chunk.Chunk.HasValue) fileStream.Seek((chunk.ChunkSize * (chunk.Chunk ?? 0)), io.SeekOrigin.Begin);

                int readLen = 0;
                var buffer = new byte[8 * 1024];

                while ((readLen = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fileStream.Write(buffer, 0, readLen);
                    fileStream.Flush();
                }

                return chunk;
            }
        }
    }
}
