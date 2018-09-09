using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.File
{
    internal class Zip
    {
        internal static string UnCompressFiles(string filePath, string unCompressDirName, bool isCoverOrNew)
        {
            string dirPath = filePath.Substring(0, filePath.LastIndexOf('\\') + 1).Trim('\\'); //文件夹
            string fileName = filePath.Substring(filePath.LastIndexOf('\\')).Trim('\\');//文件名
            if (string.IsNullOrEmpty(unCompressDirName))
                unCompressDirName = fileName.Substring(0, fileName.LastIndexOf('.'));
            dirPath = ALFile.GetNewDirPath(dirPath, unCompressDirName, isCoverOrNew);
            bool isNewDir = false;
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
                isNewDir = true;
            }
            ZipInputStream zipInputStream = null;
            FileStream fs = null;
            try
            {
                zipInputStream = new ZipInputStream(System.IO.File.OpenRead(filePath)); //读取压缩文件，并用此文件流新建 “ZipInputStream”对象
                ZipEntry zipEntry = zipInputStream.GetNextEntry();//获取解压文件流中的项目。
                while (zipInputStream.CanDecompressEntry && zipEntry != null && zipEntry.CanDecompress)
                {
                    string zipFileName = zipEntry.Name.Replace('/', '\\');
                    if (zipFileName.IndexOf('\\') > -1)//有文件夹的需先创建文件夹
                        Directory.CreateDirectory(dirPath + "\\" + zipFileName.Substring(0, zipFileName.LastIndexOf('\\')));
                    if (!zipEntry.IsDirectory && zipEntry.Crc != 00000000L)
                    {
                        int i = 2048;
                        byte[] b = new byte[i]; //每次缓冲 2048 字节
                        i = zipInputStream.Read(b, 0, b.Length);//读取“ZipEntry”中的字节
                        fs = System.IO.File.Create(dirPath + "\\" + zipFileName); //新建文件流
                        while (i > 0)
                        {
                            fs.Write(b, 0, i); //将字节写入新建的文件流
                            i = zipInputStream.Read(b, 0, b.Length);
                        }
                        fs.Close();
                        fs.Dispose();
                    }
                    zipEntry = zipInputStream.GetNextEntry();
                }
            }
            catch (Exception ex)
            {
                if (isNewDir)
                    Directory.Delete(dirPath, true);
                if (ex.Message == "No password set.")
                    throw new Exception("不能解压设置密码的文件!");
                else
                    throw ex;

            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
                if (zipInputStream != null)
                {
                    zipInputStream.Close();
                    zipInputStream.Dispose();
                }
            }
            return dirPath;
        }
    }
}
