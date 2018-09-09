using BIStudio.Framework.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BIStudio.Framework.File
{
    /// <summary>
    /// 文件处理类
    /// </summary>
    /// <remarks>
    /// 负责文件的创建及读取
    /// [2012-03-11]
    /// </remarks>
    public abstract class ALFile
    {
        #region 私有方法
        /// <summary>
        /// 将文件的URL路径转换为物理路径
        /// </summary>
        /// <param name="path">URL路径</param>
        /// <returns></returns>
        private static string MapPath(string path)
        {
            if (path.StartsWith("..") || path.StartsWith("~/") || path.StartsWith("/"))
                return ALHttpIO.MapPath(path);
            else
                return path;
        }
        #endregion

        #region 读取文件
        /// <summary>
        /// 读取指定路径的文本文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>文件内容</returns>
        public static string ReadFile(string path)
        {
            path = MapPath(path);
            if (!System.IO.File.Exists(path))
                return null;

            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(path, true))
            {
                sb.Append(sr.ReadToEnd());
                sr.Close();
            }
            string t = sb.ToString();

            return t;
        }
        #endregion

        #region 创建文件
        /// <summary>
        /// 向指定路径写入文本文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="fileContent">文件内容</param>
        public static void CreateFile(string path, string fileContent)
        {
            path = MapPath(path);

            FileInfo file = new FileInfo(path);
            if (!file.Directory.Exists)
                file.Directory.Create();

            using (FileStream fs = System.IO.File.Create(path))
            {
                byte[] bom = new byte[] { 0xEF, 0xBB, 0xBF };
                byte[] bytes = Encoding.UTF8.GetBytes(fileContent);

                //文件无签名
                if (bytes.Length < 3)
                {
                    fs.Write(bom, 0, 3);
                }
                else
                {
                    //文件签名不正确
                    byte[] bytesHead = new byte[3];
                    Buffer.BlockCopy(bytes, 0, bytesHead, 0, 3);
                    if (bytesHead != bom)
                        fs.Write(bom, 0, 3);
                }
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }
        }
        #endregion

        #region 删除文件
        /// <summary>
        /// 删除指定路径的文件
        /// </summary>
        /// <param name="path">文件路径</param>
        public static void DeleteFile(string path)
        {
            path = MapPath(path);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }
        #endregion

        #region 获取合法文件名
        /// <summary>
        /// 获取合法文件名
        /// </summary>
        /// <param name="dirPath">所在文件夹路径</param>
        /// <param name="fileName">文件名 ,xxx.zip</param>
        /// <param name="isCoverOrNew">覆盖还是新建</param>
        /// <returns></returns>
        public static string GetNewFilePath(string dirPath, string fileName, bool isCoverOrNew)
        {
            string name = fileName.Substring(0, fileName.LastIndexOf('.'));
            string extName = fileName.Substring(fileName.LastIndexOf('.'), fileName.Length - fileName.LastIndexOf('.'));
            string filePath = dirPath.Trim('\\') + "\\" + name + extName;
            if (isCoverOrNew) //若为覆盖,则直接返回文件路径
                return filePath;
            //否则计算文件名
            int count = 1;
            while (System.IO.File.Exists(filePath))
            {
                filePath = dirPath.Trim('\\') + "\\" + name + "(" + count + ")" + extName;
                count++;
            }
            return filePath;
        }
        #endregion

        #region 获取合法的文件夹名称
        /// <summary>
        /// 获取合法的文件夹名称
        /// </summary>
        /// <param name="rootPath">文件所在根路径</param>
        /// <param name="dirName">文件夹名称</param>
        /// <param name="isCoverOrNew">覆盖还是新建</param>
        /// <returns></returns>
        public static string GetNewDirPath(string rootPath, string dirName, bool isCoverOrNew)
        {
            string filePath = rootPath.Trim('\\') + "\\" + dirName;
            if (isCoverOrNew)
                return filePath;
            int count = 1;
            while (Directory.Exists(filePath))
            {
                filePath = rootPath.Trim('\\') + "\\" + dirName + "(" + count + ")";
                count++;
            }
            return filePath;
        }
        #endregion

        #region 压缩文件主方法

        /// <summary>
        /// 压缩文件,只支持zip压缩
        /// </summary>
        /// <param name="fileAbsolutePathList">文件绝对路径的列表</param>
        /// <param name="compressFileName">压缩后文件的名称</param>
        /// <param name="isCoverOrNew">true为覆盖,false为新建</param>
        /// <returns></returns>
        public static string CompressFiles(List<string> fileAbsolutePathList, List<string> fileNameList, string compressFileName, bool isCoverOrNew)
        {
            if (fileAbsolutePathList == null || fileAbsolutePathList.Count < 1)
                throw new Exception("至少传入一个文件或文件夹的绝对路径!");
            string firstPath = fileAbsolutePathList[0].Replace('/', '\\');
            string dirPath = firstPath.Substring(0, firstPath.LastIndexOf('\\') + 1);
            if (string.IsNullOrEmpty(compressFileName))
            {
                compressFileName = firstPath.Substring(firstPath.LastIndexOf('\\'));//取出最后一个\后面的字符串 作为文件名
                if (System.IO.File.Exists(firstPath))
                    compressFileName = compressFileName.Substring(0, compressFileName.LastIndexOf('.'));//去除文件名的扩展名
            }
            if (!Path.IsPathRooted(compressFileName))
                compressFileName = GetNewFilePath(dirPath, compressFileName + ".zip", isCoverOrNew);//获取合法的文件名

            ICSharpCode.SharpZipLib.Zip.ZipOutputStream zipOutput = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(new FileStream(compressFileName, FileMode.Create)); //新建压缩文件流 “ZipOutputStream”
            try
            {
                zipOutput.SetLevel(9); //压缩等级
                for (int i = 0; i < fileAbsolutePathList.Count; i++)
                {
                    if (fileNameList == null || fileNameList.Count != fileAbsolutePathList.Count)
                        appendStream(zipOutput, fileAbsolutePathList[i], dirPath);
                    else
                        appendStream(zipOutput, fileAbsolutePathList[i], fileNameList[i], dirPath);
                }
                zipOutput.Finish();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                zipOutput.Close();
                zipOutput.Dispose();
            }

            return compressFileName;
        }
        /// <summary>
        /// 压缩文件,只支持zip压缩
        /// </summary>
        /// <param name="fileAbsolutePathList">文件绝对路径的列表</param>
        /// <param name="compressFileName">压缩后文件的名称</param>
        /// <param name="isCoverOrNew">true为覆盖,false为新建</param>
        /// <returns></returns>
        public static string CompressFiles(List<string> fileAbsolutePathList, string compressFileName, bool isCoverOrNew)
        {
            return CompressFiles(fileAbsolutePathList, null, compressFileName, false);
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="fileAbsolutePathList">文件绝对路径的列表</param>
        /// <param name="compressFileName">压缩后文件的名称</param>
        /// <returns></returns>
        public static string CompressFiles(List<string> fileAbsolutePathList, string compressFileName)
        {
            return CompressFiles(fileAbsolutePathList, compressFileName, false);
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="fileAbsolutePathList">文件绝对路径的列表</param>
        /// <param name="isCoverOrNew">true为覆盖,false为新建</param>
        /// <returns></returns>
        public static string CompressFiles(List<string> fileAbsolutePathList, bool isCoverOrNew)
        {
            return CompressFiles(fileAbsolutePathList, null, isCoverOrNew);
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="fileAbsolutePathList">文件绝对路径的列表</param>
        /// <returns></returns>
        public static string CompressFiles(List<string> fileAbsolutePathList)
        {
            return CompressFiles(fileAbsolutePathList, null, false);
        }


        #region 写入流

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="zipStream">压缩流</param>
        /// <param name="filePath">文件或文件夹路径</param>
        /// <param name="rootDirPath">根路径(就是压缩后的文件存储的路径)</param>
        private static void appendStream(ICSharpCode.SharpZipLib.Zip.ZipOutputStream zipStream, string filePath, string fileName, string rootDirPath)
        {
            if (string.IsNullOrEmpty(filePath)) return;
            filePath = filePath.Replace('/', '\\');
            if (string.IsNullOrEmpty(filePath.Replace(rootDirPath, "").Trim('\\'))) return;
            if (Directory.Exists(filePath))//若为文件夹
            {
                DirectoryInfo dir = new DirectoryInfo(filePath);
                FileSystemInfo[] fileSystemList = dir.GetFileSystemInfos();
                if (fileSystemList == null || fileSystemList.Length < 1) //若该文件加下面没有子文件或文件夹,需创建空的文件夹
                {
                    ICSharpCode.SharpZipLib.Zip.ZipEntry zipEntry = new ICSharpCode.SharpZipLib.Zip.ZipEntry(filePath.Replace(rootDirPath, "").Trim('\\') + "\\"); //末尾“\\”用于文件夹的标记
                    zipStream.PutNextEntry(zipEntry);
                    return;
                }
                foreach (FileSystemInfo fileSystem in fileSystemList)
                    appendStream(zipStream, fileSystem.FullName, rootDirPath);
            }
            else if (System.IO.File.Exists(filePath))
            {
                FileStream fs = System.IO.File.OpenRead(filePath);
                BinaryReader br = new BinaryReader(fs);
                try
                {
                    FileInfo file = new FileInfo(filePath);//根据文件路径获取文件名的简便方法    
                    ICSharpCode.SharpZipLib.Zip.ZipEntry zipEntry = string.IsNullOrEmpty(fileName) ?
                        new ICSharpCode.SharpZipLib.Zip.ZipEntry(filePath.Replace(rootDirPath, "").Trim('\\')) :
                        new ICSharpCode.SharpZipLib.Zip.ZipEntry(fileName);

                    zipStream.PutNextEntry(zipEntry); //为压缩文件流提供一个容器
                    int packSize = 10240;
                    int maxCount = (int)Math.Ceiling((fs.Length + 0.0) / packSize);
                    for (int i = 0; i < maxCount; i++)
                    {
                        byte[] b = br.ReadBytes(packSize);//将文件流加入缓冲字节中
                        zipStream.Write(b, 0, b.Length); //写入字节
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    br.Close();
                    fs.Close();
                    fs.Dispose();
                }
            }
        }

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="zipStream">压缩流</param>
        /// <param name="filePath">文件或文件夹路径</param>
        /// <param name="rootDirPath">根路径(就是压缩后的文件存储的路径)</param>
        private static void appendStream(ICSharpCode.SharpZipLib.Zip.ZipOutputStream zipStream, string filePath, string rootDirPath)
        {
            appendStream(zipStream, filePath, null, rootDirPath);
        }
        #endregion

        #endregion

        #region 解压文件主方法
        /// <summary>
        /// 解压文件主方法 支持所有格式的解压
        /// </summary>
        /// <param name="filePath">压缩文件路径</param>
        /// <param name="unCompressDirName">解压目录名称</param>
        /// <param name="isCoverOrNew">true为覆盖,false为新建</param>
        /// <returns></returns>
        public static string UnCompressFiles(string filePath, string unCompressDirName, bool isCoverOrNew)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new Exception("请传入需解压的文件绝对路径(filePath)!");
            string suffixName = filePath.Substring(filePath.LastIndexOf('.') + 1).ToUpper();
            if (suffixName == "ZIP" || suffixName == "GZIP" || suffixName == "TAR" || suffixName == "BZIP2")
                return Zip.UnCompressFiles(filePath, unCompressDirName, isCoverOrNew);
            else
                return Rar.UnCompressFiles(filePath, unCompressDirName, isCoverOrNew);
        }

        /// <summary>
        /// 解压文件主方法
        /// </summary>
        /// <param name="filePath">压缩文件路径</param>
        /// <param name="unCompressDirName">解压目录名称</param>
        /// <returns></returns>
        public static string UnCompressFiles(string filePath, string unCompressDirName)
        {
            return UnCompressFiles(filePath, unCompressDirName, false);
        }


        /// <summary>
        /// 解压文件主方法
        /// </summary>
        /// <param name="filePath">压缩文件路径</param>
        /// <param name="isCoverOrNew">true为覆盖,false为新建</param>
        /// <returns></returns>
        public static string UnCompressFiles(string filePath, bool isCoverOrNew)
        {
            return UnCompressFiles(filePath, null, isCoverOrNew);
        }


        /// <summary>
        /// 解压文件主方法
        /// </summary>
        /// <param name="filePath">压缩文件路径</param>
        /// <returns></returns>
        public static string UnCompressFiles(string filePath)
        {
            return UnCompressFiles(filePath, null, false);
        }
        #endregion

        #region 文件下载
        public static void FileDownload(string filePath, long speed)
        {
            FileDownload(filePath, speed, null);
        }

        public static void FileDownload(string filePath, long speed, string downloadFileName)
        {
            FileDownload(filePath, speed, downloadFileName, null);
        }
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="filePath">所需下载文件的绝对路径</param>
        /// <param name="speed">限制速度以Byte为单位,若限制为200KB的下载速度则为1024 * 200</param>
        public static void FileDownload(string filePath, long speed, string downloadFileName, string mime)
        {
            HttpContext httpContext = HttpContext.Current;
            try
            {
                //验证：HttpMethod，为防止回传下载页面变成空白.只允许get下载
                switch (httpContext.Request.HttpMethod.ToUpper())
                {
                    //目前只支持GET和HEAD方法
                    case "GET":
                    case "HEAD":
                        break;
                    default:
                        httpContext.Response.StatusCode = 501;
                        return;
                }
                //验证：请求的文件是否存在
                if (!System.IO.File.Exists(filePath))
                {
                    httpContext.Response.StatusCode = 404;
                    return;
                }
                //定义局部变量
                long startBytes = 0;
                int packSize = 1024 * 10; //分块读取，每块10K bytes
                string fileName = downloadFileName;
                if (string.IsNullOrEmpty(fileName))
                    fileName = Path.GetFileName(filePath);
                FileStream myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(myFile);
                long fileLength = myFile.Length;

                int sleep = (int)Math.Ceiling(1000.0 * packSize / speed);//毫秒数：读取下一数据块的时间间隔
                string lastUpdateTiemStr = System.IO.File.GetLastWriteTimeUtc(filePath).ToString("r");
                string eTag = HttpUtility.UrlEncode(fileName, Encoding.UTF8) + lastUpdateTiemStr;//便于恢复下载时提取请求头;

                //验证：文件是否太大(2G)
                if (myFile.Length > Int32.MaxValue)
                {
                    httpContext.Response.StatusCode = 413;//请求实体太大
                    return;
                }
                //对应响应头ETag：文件名+文件最后修改时间
                if (httpContext.Request.Headers["If-Range"] != null)
                {
                    //----------上次被请求的日期之后被修改过--------------
                    if (httpContext.Request.Headers["If-Range"].Replace("\"", "") != eTag)
                    {
                        //文件修改过
                        httpContext.Response.StatusCode = 412;//预处理失败
                        return;
                    }
                }

                try
                {
                    //添加重要响应头、解析请求头、相关验证
                    httpContext.Response.Clear();
                    httpContext.Response.Buffer = false;
                    httpContext.Response.AddHeader("Content-MD5", ALEncrypt.Md5(myFile.ToString()));//用于验证文件
                    httpContext.Response.AddHeader("Accept-Ranges", "bytes");//重要：续传必须
                    httpContext.Response.AppendHeader("ETag", "\"" + eTag + "\"");//重要：续传必须
                    httpContext.Response.AppendHeader("Last-Modified", lastUpdateTiemStr);//把最后修改日期写入响应      
                    if (string.IsNullOrEmpty(mime))
                        httpContext.Response.ContentType = "application/octet-stream";//MIME类型：匹配任意文件类型
                    else
                        httpContext.Response.ContentType = mime;

                    if (httpContext.Request.Headers["USER-AGENT"].IndexOf("MSIE") < 0
                        && httpContext.Request.Headers["USER-AGENT"].IndexOf("like Gecko") < 0)
                        fileName = "=?UTF-8?B?" + ALEncrypt.Base64EnCode(fileName) + "?=";
                    else
                        fileName = HttpUtility.UrlEncode(fileName, Encoding.UTF8).Replace("+", "%20");

                    httpContext.Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                    httpContext.Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    httpContext.Response.AddHeader("Connection", "Keep-Alive");
                    httpContext.Response.ContentEncoding = Encoding.UTF8;
                    if (httpContext.Request.Headers["Range"] != null)
                    {
                        //------如果是续传请求，则获取续传的起始位置，即已经下载到客户端的字节数------
                        httpContext.Response.StatusCode = 206;//重要：续传必须，表示局部范围响应。初始下载时默认为200
                        string[] range = httpContext.Request.Headers["Range"].Split(new char[] { '=', '-' });//"bytes=1474560-"
                        startBytes = Convert.ToInt64(range[1]);//已经下载的字节数，即本次下载的开始位置  
                        if (startBytes < 0 || startBytes >= fileLength)
                            return;
                    }
                    //如果是续传请求，告诉客户端本次的开始字节数，总长度，以便客户端将续传数据追加到startBytes位置后
                    if (startBytes > 0)
                        httpContext.Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                    //向客户端发送数据块
                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    //分块下载，剩余部分可分成的块数
                    int maxCount = (int)Math.Ceiling((fileLength - startBytes + 0.0) / packSize);

                    //客户端未中断连接，并且数据没有传输完.则一直传输
                    for (int i = 0; i < maxCount && httpContext.Response.IsClientConnected; i++)
                    {
                        httpContext.Response.BinaryWrite(br.ReadBytes(packSize));
                        httpContext.Response.Flush();
                        if (sleep > 1) Thread.Sleep(sleep);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    br.Close();
                    myFile.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 文件下载,默认500KB的下载速度
        /// </summary>
        /// <param name="filePath">所需下载的文件的绝对路径</param>
        public static void FileDownload(string filePath)
        {
            FileDownload(filePath, 1024 * 500);
        }

        /// <summary>
        /// 打包下载
        /// </summary>
        /// <param name="packFolderPath">需要打包的文件夹所在路径</param>
        /// <param name="fileNames">需要打包的文件集合（文件名）</param>
        /// <param name="zipFileName">打包后显示的文件名</param>
        public static void FilePackDownload(string packFolderPath, List<string> fileNames, string zipFileName)
        {
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(System.Text.Encoding.Default))//解决中文乱码问题
            {
                HttpContext httpContext = HttpContext.Current;
                httpContext.Response.Clear();
                httpContext.Response.ContentType = "application/zip";

                //兼容火狐和IE11的浏览器下载
                string packFileName = "";
                if (httpContext.Request.Headers["USER-AGENT"].IndexOf("MSIE") < 0
                        && httpContext.Request.Headers["USER-AGENT"].IndexOf("like Gecko") < 0)
                    packFileName = "=?UTF-8?B?" + ALEncrypt.Base64EnCode(zipFileName) + "?=";
                else
                    packFileName = HttpUtility.UrlEncode(zipFileName, Encoding.UTF8);

                httpContext.Response.AddHeader("content-disposition", "filename=" + packFileName);
                zip.AddDirectory(packFolderPath);
                foreach (string item in fileNames)
                {
                    try
                    {
                        zip.AddFile(item);
                    }
                    catch (Exception err)
                    {
                        ALMessage.RegMsg("打包下载失败，具体原因：" + err.Message);
                    }
                }

                zip.Save(httpContext.Response.OutputStream);
            }
        }
        #endregion
    }
}
