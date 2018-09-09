namespace BIStudio.Framework.File
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Data.SqlClient;
    using System.Web;
    using System.IO;
    using System.Security.AccessControl;
    using System.Web.Caching;
    using BIStudio.Framework.Domain.Repository;
    using BIStudio.Framework.Utils;
    using BIStudio.Framework.Data.Adapter;
    using BIStudio.Framework.Domain.Dapper;
    /// <summary>
    /// 
    /// </summary>
    public class BiAttachHelper : Repository<SysAttachInfo>
    {
        private BiAttachHelper() { }
        /// <summary>
        /// 用于存储到硬盘的构造函数
        /// </summary>
        public BiAttachHelper(string filePath , int mode )
        {
            this._dirPath = ALHttpIO.MapPath("~/");
            this._filePath = filePath;
            this._mode = mode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="filePath"></param>
        /// <param name="mode"></param>
        public BiAttachHelper(string dirPath, string filePath, int mode)
        {
            this._dirPath = dirPath;
            this._filePath = filePath;
            this._mode = mode;
        }

        #region Property

        protected string _dirPath = null;
        protected string _filePath = null; //用于记录文件夹路径
        protected int _mode = 0; //存储模式（0-数据库存储；1-硬盘存储）

        #endregion

        #region 保存文件 ,当bindTalbeID为NULL时,为预保存
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindTalbeID"></param>
        /// <param name="bindTableName"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public SysAttachInfo SaveAttach(int? bindTalbeID, string bindTableName, HttpPostedFile file)
        {
            return SaveAttach(bindTalbeID, bindTableName, null, file);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindTalbeID"></param>
        /// <param name="bindTableName"></param>
        /// <param name="pageKey"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public SysAttachInfo SaveAttach(int? bindTalbeID, string bindTableName, string pageKey, HttpPostedFile file)
        {
            return SaveAttach(bindTalbeID, bindTableName, null, pageKey, file);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindTalbeID"></param>
        /// <param name="bindTableName"></param>
        /// <param name="customType"></param>
        /// <param name="pageKey"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public SysAttachInfo SaveAttach(int? bindTalbeID, string bindTableName, int? customType, string pageKey, HttpPostedFile file)
        {
            SysAttachInfo attach = GenerateFile(bindTalbeID, bindTableName, customType, pageKey, file);
            if (bindTalbeID.HasValue)
                this.Save(attach);
            else
                AddAttachToSession(attach);
            return attach;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindTalbeID"></param>
        /// <param name="bindTableName"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public List<SysAttachInfo> SaveAttach(int? bindTalbeID, string bindTableName, HttpFileCollection files)
        {
            return SaveAttach(bindTalbeID, bindTableName, null, files);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindTalbeID"></param>
        /// <param name="bindTableName"></param>
        /// <param name="pageKey"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public List<SysAttachInfo> SaveAttach(int? bindTalbeID, string bindTableName,string pageKey, HttpFileCollection files)
        {
            return SaveAttach(bindTalbeID, bindTableName, null, pageKey, files);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindTalbeID"></param>
        /// <param name="bindTableName"></param>
        /// <param name="customType"></param>
        /// <param name="pageKey"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public List<SysAttachInfo> SaveAttach(int? bindTalbeID, string bindTableName, int? customType, string pageKey, HttpFileCollection files)
        {
            List<SysAttachInfo> attachList = new List<SysAttachInfo>();
            foreach (HttpPostedFile file in files)
            {
                if (file == null || file.ContentLength < 0)
                    continue;
                attachList.Add(GenerateFile(bindTalbeID, bindTableName, customType, pageKey, file));
            }
            if (bindTalbeID.HasValue)
                this.Save(attachList.ToArray());
            else
                AddAttachToSession(attachList);
            return attachList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindTalbeID"></param>
        /// <param name="bindTableName"></param>
        /// <param name="customType"></param>
        /// <param name="pageKey"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        protected virtual SysAttachInfo GenerateFile(int? bindTalbeID, string bindTableName, int? customType, string pageKey, HttpPostedFile file)
        {
            SysAttachInfo attach = new SysAttachInfo()
            {
                BindTableID = bindTalbeID,
                BindTableName = bindTableName,
                FileName = Path.GetFileName(file.FileName),
                FileSize = file.ContentLength,
                FileType = file.ContentType,
                InputerID = ALCurrentUser.UserID,
                Inputer = ALCurrentUser.UserName,
                InputTime = DateTime.Now,
                CustomType = customType,
                Mode = this._mode,
                Key = bindTalbeID.HasValue ? null : Guid.NewGuid().ToString(),
                PageKey = pageKey
            };
            byte[] buffer = new byte[file.ContentLength];
            file.InputStream.Read(buffer, 0, buffer.Length);
            //根据mode进行不同方式存储 ,当bindTalbeID没有时,也需存储到硬盘
            if (this._mode == 1 || !bindTalbeID.HasValue)
            {
                string path = this.getServerFilePath(_filePath, bindTableName, customType);
                DirectoryInfo dir = new DirectoryInfo(this.MapPath(path));
                //判断目录是否存在
                if (!dir.Exists)
                    dir.Create();
                //判断文件是否存在
                string serverFileName = null;
                FileInfo[] fileList = null;
                int count = 0;
                do
                {
                    serverFileName = this.getServerFileName(attach.FileName, count);
                    fileList = dir.GetFiles(serverFileName);
                    count++;
                }
                while (fileList != null && fileList.Length > 0);
                attach.FilePath = path + "/" + serverFileName;
                File.WriteAllBytes(this.MapPath(attach.FilePath), buffer);
            }
            else
                attach.Content = buffer;
            return attach;
        }

        /// <summary>
        /// 使用指定规则生成文件名
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        protected virtual string getServerFileName(string fileName, params object[] parms)
        {
            return Path.GetFileNameWithoutExtension(fileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + parms[0]
                        + Path.GetExtension(fileName);
        }/// <summary>
        /// 使用指定规则生成文件目录
        /// </summary>
        protected virtual string getServerFilePath(string filePath, string bindTableName, int? customType)
        {
            return this._filePath + "/" + bindTableName + (customType.HasValue ? ("/" + customType.Value) : "");
        }

        #region 将未确定bindTalbeID的附件加入Session
        private void AddAttachToSession(List<SysAttachInfo> attachList)
        {
            List<SysAttachInfo> old = null;
            if (this.AttachInfos != null)
                old = this.AttachInfos;
            else
                old = new List<SysAttachInfo>();
            old.AddRange(attachList);
            this.AttachInfos = old;
        }

        private void AddAttachToSession(SysAttachInfo attach)
        {
            List<SysAttachInfo> list = new List<SysAttachInfo>();
            list.Add(attach);
            AddAttachToSession(list);
        }
        #endregion
        #endregion

        #region 将预保存的文件信息存入数据库
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindTalbeID"></param>
        /// <param name="bindTableName"></param>
        /// <param name="customType"></param>
        /// <param name="pageKey"></param>
        /// <returns></returns>
        public List<SysAttachInfo> ReSaveAttach(int bindTalbeID, string bindTableName, int? customType, string pageKey)
        {
            List<SysAttachInfo> list = null;
            if (this.AttachInfos != null)
                list = this.AttachInfos;
            if (list == null || list.Count < 1) return null;
            List<SysAttachInfo> saveList = new List<SysAttachInfo>();
            List<SysAttachInfo> oldList = new List<SysAttachInfo>();
            foreach (SysAttachInfo attach in list)
            {
                if (bindTableName.ToUpper() == attach.BindTableName.ToUpper()
                    && (!customType.HasValue || attach.CustomType == customType)
                    && attach.PageKey == pageKey)
                {
                    attach.BindTableID = bindTalbeID;
                    if (this._mode == 0)
                    {
                        //为数据库存储时,需删除硬盘上的文件
                        attach.Content = File.ReadAllBytes(this.MapPath(attach.FilePath));
                        string filepath = this.MapPath(attach.FilePath);
                        if (File.Exists(filepath))
                            File.Delete(filepath);
                        attach.FilePath = null;
                    }
                    saveList.Add(attach);
                }
                else
                    oldList.Add(attach);
            }
            if (oldList.Count > 0)
                this.AttachInfos = oldList;
            else
                this.AttachInfos = null;

            if (saveList.Count > 0)
            {
                this.Save(saveList.ToArray());
                return saveList;
            }
            else
                return null;
        }
        #endregion

        #region 查询
        /// <summary>
        /// 根据附件ID获取附件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SysAttachInfo GetSysAttach(int id)
        {
            return new BiAttachHelper().GetInfo(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static List<SysAttachInfo> GetSysAttach(List<int> ids)
        {
            string sql = string.Format("SELECT * FROM SysAttach WHERE id in ({0})",ALConvert.ListToString<int>(ids,','));
            return DBHelperProxy.GetDataTable(sql).ToList<SysAttachInfo>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindTableName"></param>
        /// <param name="bindTalbeID"></param>
        /// <param name="customType"></param>
        /// <returns></returns>
        public static List<SysAttachInfo> GetSysAttachList(string bindTableName, int? bindTalbeID, int? customType)
        {
            string sql = "SELECT * FROM SysAttach WHERE 1=1";
            if (!string.IsNullOrEmpty(bindTableName))
                sql += " AND BindTableName = '" + bindTableName + "'";
            if (bindTalbeID.HasValue)
                sql += " AND BindTableID = " + bindTalbeID.Value;
            if (customType.HasValue)
                sql += " AND CustomType = " + customType.Value;
            return DBHelperProxy.GetDataTable(sql).ToList<SysAttachInfo>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindTableName"></param>
        /// <param name="bindTalbeID"></param>
        /// <returns></returns>
        public static List<SysAttachInfo> GetSysAttachList(string bindTableName, int bindTalbeID)
        {
            return GetSysAttachList(bindTableName, bindTalbeID, null);
        }

        #endregion


        #region 删除
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindTableID"></param>
        /// <param name="bindTableName"></param>
        /// <param name="customType"></param>
        public void DeleteAttach(int? bindTableID, string bindTableName, int? customType)
        {
            if (string.IsNullOrEmpty(bindTableName))
                throw new Exception("BindTableName不允许为空");
            List<SysAttachInfo> list = BiAttachHelper.GetSysAttachList(bindTableName, bindTableID, customType);
            foreach (SysAttachInfo attach in list)
            {
                if (attach.Mode == 1)
                {
                    string filepath = this.MapPath(attach.FilePath);
                    if (File.Exists(filepath))
                        File.Delete(filepath);
                }
            }
            string sql = "DELETE FROM SysAttach WHERE BindTableName = '" + bindTableName + "'";
            if (bindTableID.HasValue)
                sql += " AND BindTableID = " + bindTableID;
            if (customType.HasValue)
                sql += " AND CustomType = " + customType;
            DBHelperProxy.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 删除永久附件
        /// </summary>
        /// <param name="id"></param>
        public virtual void DeleteAttach(int id)
        {
            SysAttachInfo attach = this.GetInfo(id);
            if (attach == null) return;
            if (attach.Mode == null) return;
            if (attach.Mode == 1)
            {
                string filepath = this.MapPath(attach.FilePath);
                if (File.Exists(filepath))
                    File.Delete(filepath);
            }
            this.Delete(id);
        }
        /// <summary>
        /// 删除临时附件
        /// </summary>
        /// <param name="bindTableName"></param>
        /// <param name="customType"></param>
        /// <param name="key"></param>
        public void DeleteAttach(string bindTableName, int? customType, string key)
        {
            List<SysAttachInfo> attachList = this.AttachInfos;
            List<SysAttachInfo> newList = new List<SysAttachInfo>();
            foreach (SysAttachInfo attach in attachList)
            {
                if (attach.Key == key && attach.InputerID == ALCurrentUser.UserID
                    && attach.BindTableName == bindTableName
                    && (!customType.HasValue || attach.CustomType == customType))
                {
                    string filepath = this.MapPath(attach.FilePath);
                    if (File.Exists(filepath))
                        File.Delete(filepath);
                    continue;
                }
                newList.Add(attach);
            }
            this.AttachInfos = newList;
        }
        #endregion

        /// <summary>
        /// 当前用户ID
        /// </summary>
        public string SessionID { get; set; }
        private string sessionPath = "~/UserFile/Temp/" + DateTime.Now.ToString("yyyyMM") + "/{0}.json";
        /// <summary>
        /// 临时附件存储地址
        /// </summary>
        public string SessionPath
        {
            get
            {
                return this.MapPath(string.Format(this.sessionPath, this.SessionID));
            }
        }
        /// <summary>
        /// 临时文件存储
        /// </summary>
        public List<SysAttachInfo> AttachInfos
        {
            get
            {
                if (this.SessionID == null)
                    return HttpContext.Current.Session["SysAttach"] as List<SysAttachInfo>;
                else
                {
                    //解决文件上传后，SESSION超时无法保存附件的问题
                    FileInfo file = new FileInfo(this.SessionPath);
                    if (!file.Directory.Exists)
                        file.Directory.Create();

                    if (file.Exists)
                        return ALSerialize.JsonDeserialize<List<SysAttachInfo>>(File.ReadAllText(file.FullName));
                    else
                        return null;
                }
            }
            set
            {
                if (this.SessionID == null)
                    HttpContext.Current.Session["SysAttach"] = value;
                else
                {
                    //解决文件上传后，SESSION超时无法保存附件的问题
                    FileInfo file = new FileInfo(this.SessionPath);
                    if (!file.Directory.Exists)
                        file.Directory.Create();

                    if (value == null)
                    {
                        if (file.Exists)
                            file.Delete();
                    }
                    else
                        File.WriteAllText(file.FullName, ALSerialize.JsonSerialize<List<SysAttachInfo>>(value));
                }
            }
        }

        #region 返回物理路径
        /// <summary>
        /// 根据相对路径（结合绝对地址），获取物理路径
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <returns></returns>
        protected string MapPath(string path)
        {
            switch (this._mode)
            {
                case 0:
                case 1:
                    return ALHttpIO.MapPath(path, _dirPath);
                case 2:
                    return HttpContext.Current.Server.MapPath(path);
                default:
                    return path;
            }
        } 
        #endregion
    }
}
