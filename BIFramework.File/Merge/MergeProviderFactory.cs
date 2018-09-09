using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using io = System.IO;

namespace BIStudio.Framework.File
{
    /// <summary>
    /// 合并文件
    /// </summary>
    internal class MergeProviderFactory
    {
        Mutex _mutex = new Mutex();
        Mutex _rmutex = new Mutex();

        /// <summary>
        /// 文件缓存目录
        /// </summary>
        protected string BaseUrl { get { return io.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserFile", "Cache"); } }

        /// <summary>
        /// 文件合并信息
        /// </summary>
        protected IDictionary<string, MergeInfo> MergeInfo = new Dictionary<string, MergeInfo>();

        /// <summary>
        /// 合并文件
        /// </summary>
        /// <param name="chunk">文件分段信息</param>
        /// <returns>文件信息</returns>
        internal virtual FileInfo Merge(ChunkInfo chunk)
        {
            return RefreshMergeInfo(new MergeProvider(CreateUrl(chunk)).Merge(chunk));
        }

        /// <summary>
        /// 刷新文件合并信息
        /// </summary>
        /// <param name="chunk">文件分段信息</param>
        /// <returns>文件分段信息</returns>
        protected virtual ChunkInfo RefreshMergeInfo(ChunkInfo chunk)
        {
            if (!chunk.Chunks.HasValue || chunk.Chunks == 1) return chunk;

            MergeInfo mergeInfo;
            var key = string.Concat(chunk.AttachKey, chunk.UploadKey, chunk.FileID).GetHashCode().ToString() + chunk.FileID;

            _rmutex.WaitOne();

            try
            {
                if (!MergeInfo.TryGetValue(key, out mergeInfo)) MergeInfo[key] = mergeInfo = new MergeInfo { Expires = DateTime.Now.AddHours(8), Merged = 1 };
                else { mergeInfo.Merged++; mergeInfo.Expires = DateTime.Now.AddHours(8); }

                if (chunk.Chunks <= mergeInfo.Merged)
                {
                    MergeInfo.Remove(key);
                    return chunk;
                }

                return null;
            }
            finally
            {
                _rmutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// 创建文件缓存路径
        /// </summary>
        /// <param name="chunk">文件分段信息</param>
        /// <returns>文件缓存路径</returns>
        protected virtual string CreateUrl(ChunkInfo chunk)
        {
            try
            {
                Task.Factory.StartNew(Clear);
            }
            catch { }

            _mutex.WaitOne();

            try
            {
                var url = GetUrl(DateTime.Today, chunk.AttachKey, chunk.UploadKey);

                if (io.Directory.Exists(url)) io.Directory.CreateDirectory(url);

                return url;
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// 清理缓存和文件合并信息
        /// </summary>
        internal virtual void Clear()
        {
            try
            {
                DateTime begin = DateTime.Today.AddDays(-10), end = begin.AddDays(7);

                if (begin.Year != end.Year || begin.Month != end.Month)
                {
                    var url = io.Path.Combine(BaseUrl, begin.ToString("yyMM"));

                    if (io.Directory.Exists(url)) io.Directory.Delete(url, true);

                    begin = end.AddDays(-(end.Day - 1));
                }

                while (begin <= end)
                {
                    try
                    {
                        var url = GetUrl(begin);

                        if (io.Directory.Exists(url)) io.Directory.Delete(url, true);
                    }
                    catch { }

                    begin = begin.AddDays(1);
                }

                var q = from d in MergeInfo
                        where d.Value.Expires < DateTime.Now
                        select d;

                foreach (var info in q) MergeInfo.Remove(info);
            }
            catch { }
        }

        /// <summary>
        /// 获取文件合并缓存地址
        /// </summary>
        /// <param name="date">当前日期</param>
        /// <param name="attachKey">附件标示</param>
        /// <param name="uploadKey">文件上传标示</param>
        /// <returns>文件合并缓存地址</returns>
        protected virtual string GetUrl(DateTime date, string attachKey, string uploadKey)
        {
            return io.Path.Combine(BaseUrl, date.ToString("yyMM"), date.ToString("dd"), attachKey, uploadKey);
        }

        /// <summary>
        /// 获取文件缓存地址
        /// </summary>
        /// <param name="date">当前日期</param>
        /// <returns>文件缓存地址</returns>
        protected virtual string GetUrl(DateTime date)
        {
            return io.Path.Combine(BaseUrl, date.ToString("yyMM"), date.ToString("dd"));
        }
    }
}
