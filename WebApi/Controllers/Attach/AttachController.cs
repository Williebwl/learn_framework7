using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using BIStudio.Framework;
using BIStudio.Framework.File;
using BIStudio.Framework.UI;
using Ionic.Zip;

namespace WebApi.Controllers.Attach
{
    public class AttachController : ApplicationService
    {
        IBiAttach _bo = CFAspect.Resolve<IBiAttach>();

        protected virtual string BasePath { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserFile"); } }

        protected virtual string TempPath { get { return Path.Combine(BasePath, "Cache"); } }

        #region 附件上传

        /// <summary>
        /// 附件上传
        /// </summary>
        /// <param name="info">附件信息</param>
        /// <returns>文件上传信息</returns>
        [HttpPost]
        public virtual async Task<AttachResultVM> PostFormData(string id)
        {
            if (!Request.Content.IsMimeMultipartContent("form-data")) throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            id = (id ?? string.Empty).Trim();

            if ("no".Equals(id) || string.IsNullOrEmpty(id)) id = Guid.NewGuid().ToString();

            var temp = Path.Combine(TempPath, DateTime.Today.ToString("yyMMdd"), id);

            try
            {
                if (!Directory.Exists(temp)) Directory.CreateDirectory(temp);

                var provider = new MultipartFormDataStreamProvider(temp);

                await Request.Content.ReadAsMultipartAsync(provider);

                var files = provider.FileData.AsParallel().Select(d =>
                {
                    return new SYSAttach
                    {
                        BindTableName = id,
                        FileName = provider.FormData["name"],
                        FileStream = File.OpenRead(d.LocalFileName),
                        FileSize = int.Parse(provider.FormData["size"]),
                        FileType = provider.FormData["type"],
                        Mode = 0,
                        Inputer = CFContext.User.UserName,
                        InputerID = CFContext.User.ID,
                        InputTime = DateTime.Now
                    };
                }).ToArray();

                return Scratch(id, files.Any() && _bo.Save(null, files), files);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
            finally
            {
                Directory.GetDirectories(TempPath).AsParallel().OrderByDescending(d => d).Skip(2).ForAll(d => { Directory.Delete(d, true); });
            }
        }

        /// <summary>
        /// 暂存上传文件
        /// </summary>
        /// <param name="id">上传标示</param>
        /// <param name="files">文件信息</param>
        /// <returns>上传文件信息</returns>
        protected virtual AttachResultVM Scratch(string id, bool state, SYSAttach[] files)
        {
            return new AttachResultVM
            {
                Key = id,
                State = state ? 0 : 1,
                Files = files.Select(d => new FileVM
                {
                    ID = d.ID,
                    FileName = d.FileName,
                    FileType = d.FileType
                }).ToArray()
            };
        }

        /// <summary>
        /// 保存附件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual bool SaveAttach([FromBody]AttachVM[] attachVM)
        {
            if (attachVM == null || !attachVM.Any()) throw new HttpResponseException(HttpStatusCode.BadRequest);

            var infos = _bo.GetInfos(attachVM.Select(d => d.AttachKey).ToArray());

            if (infos == null || !infos.Any()) return true;

            try
            {
                var ids = from d in attachVM
                          from b in d.FileIDs
                          select b;

                var q = from d in infos
                        from b in attachVM
                        where d.BindTableName == b.AttachKey && !b.FileIDs.Contains(d.ID.Value)
                        select new SYSAttach
                        {
                            ID = d.ID,
                            BindTableName = b.TableName,
                            BindTableID = b.TableID,
                            CustomType = b.CustomType
                        };

                return _bo.Save(string.Join(",", ids.ToArray()), q.ToArray());
            }
            finally
            {
                attachVM?.AsParallel().Select(d => Path.Combine(TempPath, d.AttachKey)).ForAll(d => { if (Directory.Exists(d)) Directory.Delete(d, true); });
            }
        }

        #endregion 附件上传

        #region 文件下载

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="tableName">业务表名</param>
        /// <param name="TableID">业务表id</param>
        [HttpGet]
        public virtual async Task<HttpResponseMessage> Download([FromUri]string tableName, [FromUri] long tableID, [FromUri] int customType)
        {
            var tag = Request.Headers.IfNoneMatch.FirstOrDefault();
            if (Request.Headers.IfModifiedSince.HasValue && tag != null && tag.Tag.Length > 0)
                return Request.CreateResponse(HttpStatusCode.NotModified);

            try
            {
                return SetCache(tableName + ":" + tableID + ":" + customType, await GetDownloadFile(tableName, tableID, customType));
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

        }

        protected virtual async Task<HttpResponseMessage> GetDownloadFile(string tableName, long tableID, int customType)
        {
            var infos = _bo.GetInfos(tableName, tableID, customType);

            if (infos == null || !infos.Any()) throw new HttpResponseException(HttpStatusCode.NotFound);

            return await GetDownloadFile(infos.ToArray(), string.Concat(tableName, tableID, customType));
        }

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="ID">文件IDs</param>
        [HttpGet]
        public virtual async Task<HttpResponseMessage> Download(string id)
        {
            var tag = Request.Headers.IfNoneMatch.FirstOrDefault();
            if (Request.Headers.IfModifiedSince.HasValue && tag != null && tag.Tag.Length > 0)
                return Request.CreateResponse(HttpStatusCode.NotModified);

            try
            {
                return SetCache(id, await GetAttachFile(id));
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        protected HttpResponseMessage SetCache(string tag, HttpResponseMessage result)
        {
            result.Content.Headers.Expires = new DateTimeOffset(DateTime.Now).AddHours(1);
            // 这里应该写入文件的存储日期
            result.Content.Headers.LastModified = new DateTimeOffset(DateTime.Now);
            result.Headers.CacheControl = new CacheControlHeaderValue() { Public = true, MaxAge = TimeSpan.FromHours(1) };
            // 设置Etag，
            result.Headers.ETag = new EntityTagHeaderValue("\"" + tag + "\"");

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual async Task<HttpResponseMessage> GetAttachFile(string id)
        {
            var infos = _bo.GetInfos(id);

            if (infos == null || !infos.Any()) throw new HttpResponseException(HttpStatusCode.NotFound);

            return await GetDownloadFile(infos, id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="fileInfos"></param>
        /// <returns></returns>
        protected virtual async Task<HttpResponseMessage> GetDownloadFile(IList<SYSAttach> fileInfos, string tag)
        {
            if (!fileInfos.Any()) throw new HttpResponseException(HttpStatusCode.NotFound);

            var result = Request.CreateResponse(HttpStatusCode.OK);

            var disposition = new ContentDispositionHeaderValue("attachment");

            if (fileInfos.Count == 1)
            {
                var file = fileInfos.First();

                Stream fileStream = GetFileStream(file, tag);

                fileStream.Seek(0, SeekOrigin.Begin);

                result.Content = new StreamContent(fileStream);

                disposition.Name = disposition.FileName = file.FileName;
            }
            else
            {
                disposition.Name = disposition.FileName = DateTime.Now.ToString("yyMMddHHmm") + ".zip";

                result.Content = await CreareZip(tag, fileInfos.ToArray());
            }

            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = disposition;

            return result;
        }

        public Stream GetFileStream(SYSAttach info, string tag)
        {
            string fileName = GetTempFileName(tag, info.ID.ToString());

            if (info.FileSize > 1024 * 1024 * 12 && File.Exists(fileName)) return new FileStream(fileName, FileMode.Open);

            Stream fileStream = info.FileSize > 1024 * 1024 * 12 ? new FileStream(fileName, FileMode.Create) : new MemoryStream() as Stream;

            _bo.ReadFile(stream => { stream.CopyTo(fileStream); }, info);

            return fileStream;
        }

        public string GetTempPath(string tag)
        {
            var date = DateTime.Today;

            var temp = Path.Combine(TempPath, date.ToString("yyMM"), date.ToString("dd"), tag);

            if (!Directory.Exists(temp)) Directory.CreateDirectory(temp);

            return temp;
        }

        public string GetTempFileName(string tag, string id)
        {
            return Path.Combine(GetTempPath(tag), id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected virtual async Task<StreamContent> CreareZip(string tag, params SYSAttach[] infos)
        {
            if (infos == null) throw new ArgumentNullException("infos");

            if (!infos.Any()) throw new ArgumentOutOfRangeException("infos");

            var zipFileName = GetTempFileName(tag, tag + ".zip");
            Stream zipFileStream = null;

            if (File.Exists(zipFileName)) zipFileStream = new FileStream(zipFileName, FileMode.Open);
            else
            {
                using (var zip = new ZipFile(Encoding.Default))
                {
                    var files = infos.Select(info =>
                      {
                          int i = 0;
                          string fileName = info.FileName, name = Path.GetFileName(fileName), ext = Path.GetExtension(fileName);

                          while (zip.EntryFileNames.Any(d => d.Equals(fileName))) fileName = name + "(" + (++i) + ")" + ext;

                          var fileStream = GetFileStream(info, tag);

                          fileStream.Seek(0, SeekOrigin.Begin);

                          zip.AddEntry(fileName, fileStream);

                          return fileStream is FileStream ? (fileStream as FileStream).Name : string.Empty;
                      }).ToArray();

                    zip.Save(zipFileStream = new FileStream(zipFileName, FileMode.Create));

                    Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            foreach (var file in files)
                            {
                                if (File.Exists(file)) File.Delete(file);
                            }
                        }
                        catch { }
                    });
                }
            }

            zipFileStream.Seek(0, SeekOrigin.Begin);

            return new StreamContent(zipFileStream);
        }

        #endregion 文件下载

        #region 获取文件信息

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="tableName">业务表名</param>
        /// <param name="TableID">业务表id</param>
        /// <returns>文件信息</returns>
        [HttpGet]
        public IList<FileVM> Get([FromUri]string tableName, [FromUri] long? tableID, [FromUri] int customType)
        {
            if (string.IsNullOrEmpty(tableName) || !(tableID > 0)) return null;

            return _bo.GetInfos(tableName, tableID.Value, customType).Map<SYSAttach, FileVM>().ToList();
        }

        [HttpGet]
        public AttachResultVM GetAttach([FromUri]string tableName, [FromUri] long? tableID, [FromUri] int customType)
        {
            var vm = new AttachResultVM { Key = Guid.NewGuid().ToString() };

            if (string.IsNullOrEmpty(tableName) || !(tableID > 0)) goto End;

            vm.Files = _bo.GetInfos(tableName, tableID.Value, customType).Map<SYSAttach, FileVM>().ToList();

            End: return vm;
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="ID">文件IDs</param>
        /// <returns>文件信息</returns>
        [HttpGet]
        public IList<FileVM> Get(string id)
        {
            return _bo.GetInfos(id).Map<SYSAttach, FileVM>().ToList();
        }

        #endregion 获取文件信息

        #region 附件删除

        /// <summary>
        /// 附件删除
        /// </summary>
        /// <param name="tableName">业务表名</param>
        /// <param name="TableID">业务表id</param>
        /// <returns>是否删除成功</returns>
        [HttpDelete]
        public bool Delete([FromUri]string tableName, [FromUri]long? tableID, [FromUri] int customType)
        {
            if (string.IsNullOrEmpty(tableName) || !(tableID > 0)) return false;

            return _bo.Remove(tableName, tableID.Value, customType);
        }

        /// 附件删除
        /// </summary>
        /// <param name="ID">文件IDs</param>
        /// <returns>是否删除成功</returns>
        [HttpDelete]
        public bool Delete(string id)
        {
            return _bo.Remove(id);
        }

        #endregion 附件删除
    }
}