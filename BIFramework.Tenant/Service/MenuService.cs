using System;
using System.Collections.Generic;
using System.Linq;

namespace BIStudio.Framework.Tenant
{
    using BIStudio.Framework.Data;
    using BIStudio.Framework.Domain;

    /// <summary>
    /// 菜单
    /// </summary>
    public class MenuService : DomainService, IMenuService
    {
        protected IRepository<SYSApp> _appBO;
        protected IRepository<SYSMenu> _menuBO;
        protected IRepository<SYSAppAccess> _appAccessBO;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public virtual IList<SYSMenu> GetRoot(long groupID)
        {
            var q = from d in _menuBO.Entities
                    from a in _appAccessBO.Entities
                    where a.GroupID == groupID
                    && _appBO.Entities.Any(b => b.ID == d.AppID && b.IsValid == 1)
                    && d.IsShow == true
                    && d.Layer == 0
                    && d.AppID == a.AppID
                    select d;

            return q.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupIDs"></param>
        /// <returns></returns>
        public virtual IList<SYSMenu> GetRoot(IList<long> groupIDs)
        {
            var q = from d in _menuBO.Entities
                    from a in _appAccessBO.Entities
                    where groupIDs.Any(b => a.GroupID == b)
                    && _appBO.Entities.Any(b => b.ID == d.AppID && b.IsValid == 1)
                    && d.IsShow == true
                    && d.Layer == 0
                    && d.AppID == a.AppID
                    select d;

            return q.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appID"></param>
        /// <returns></returns>
        public virtual IList<SYSMenu> GetInfoByAppId(long appID)
        {
            var q = from d in _menuBO.Entities
                    where d.AppID == appID
                    orderby d.Path, d.Sequence
                    select d;

            return q.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public virtual IList<SYSMenu> GetInfosByGroup(long groupID)
        {
            var q = from d in _menuBO.Entities
                    from a in _appAccessBO.Entities
                    where a.GroupID == groupID
                    && d.AppID == a.AppID
                    select d;

            return q.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupIDs"></param>
        /// <returns></returns>
        public virtual IList<SYSMenu> GetInfosByGroup(IList<long> groupIDs)
        {
            var q = from d in _menuBO.Entities
                    from a in _appAccessBO.Entities
                    where groupIDs.Any(b => a.GroupID == b)
                    && d.AppID == a.AppID
                    select d;

            return q.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public virtual IList<SYSMenu> GetRoute(long groupID)
        {
            var q = from d in _menuBO.Entities
                    from a in _appAccessBO.Entities
                    where a.GroupID == groupID
                    && d.AppID == a.AppID
                    orderby d.Path, d.Sequence
                    select d;

            return q.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupIDs"></param>
        /// <returns></returns>
        public virtual IList<SYSMenu> GetRoute(IList<long> groupIDs)
        {
            var q = from d in _menuBO.Entities
                    from a in _appAccessBO.Entities
                    where groupIDs.Any(b => a.GroupID == b)
                    && d.AppID == a.AppID
                    orderby d.Path, d.Sequence
                    select d;

            return q.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public virtual IList<SYSMenu> GetChildren(long groupID, long pid)
        {
            var q = from d in _menuBO.Entities
                    from a in _appAccessBO.Entities
                    where a.GroupID == groupID
                    && d.AppID == a.AppID
                    && d.ParentID == pid
                    orderby d.Path, d.Sequence
                    select d;

            return q.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupIDs"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public virtual IList<SYSMenu> GetChildren(IList<long> groupIDs, long pid)
        {
            var q = from d in _menuBO.Entities
                    from a in _appAccessBO.Entities
                    where groupIDs.Any(b => a.GroupID == b)
                    && d.AppID == a.AppID
                    && d.ParentID == pid
                    orderby d.Path, d.Sequence
                    select d;

            return q.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public virtual IList<SYSMenu> GetChildrens(long groupID, long pid)
        {
            var q = from d in _menuBO.Entities
                    from a in _appAccessBO.Entities
                    where a.GroupID == groupID
                    && d.AppID == a.AppID
                    && d.Path.Contains("," + pid + ",")
                    orderby d.Path, d.Sequence
                    select d;

            return q.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupIDs"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public virtual IList<SYSMenu> GetChildrens(IList<long> groupIDs, long pid)
        {
            var q = from d in _menuBO.Entities
                    from a in _appAccessBO.Entities
                    where groupIDs.Any(b => a.GroupID == b)
                    && d.AppID == a.AppID
                    && d.Path.Contains("," + pid + ",")
                    orderby d.Path, d.Sequence
                    select d;

            return q.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public virtual IList<SYSMenu> GetChildrensAndSelf(long groupID, long pid)
        {
            var q = from d in _menuBO.Entities
                    from a in _appAccessBO.Entities
                    where a.GroupID == groupID
                    && d.AppID == a.AppID
                    && (d.ID == pid || d.Path.Contains("," + pid + ","))
                    orderby d.Path, d.Sequence
                    select d;

            return q.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupIDs"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public virtual IList<SYSMenu> GetChildrensAndSelf(IList<long> groupIDs, long pid)
        {
            var q = from d in _menuBO.Entities
                    from a in _appAccessBO.Entities
                    where groupIDs.Any(b => a.GroupID == b)
                    && d.AppID == a.AppID
                    && (d.ID == pid || d.Path.Contains("," + pid + ","))
                    orderby d.Path, d.Sequence
                    select d;

            return q.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public virtual IList<SYSMenu> GetChildrenAndSelf(long groupID, long pid)
        {
            var q = from d in _menuBO.Entities
                    from a in _appAccessBO.Entities
                    where a.GroupID == groupID
                    && d.AppID == a.AppID
                    && (d.ID == pid || d.ParentID == pid)
                    orderby d.Path, d.Sequence
                    select d;

            return q.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupIDs"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public virtual IList<SYSMenu> GetChildrenAndSelf(IList<long> groupIDs, long pid)
        {
            var q = from d in _menuBO.Entities
                    from a in _appAccessBO.Entities
                    where groupIDs.Any(b => a.GroupID == b)
                    && d.AppID == a.AppID
                    && (d.ID == pid || d.ParentID == pid)
                    orderby d.Path, d.Sequence
                    select d;

            return q.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual IList<SYSMenu> GetInfos(string ids)
        {
            ids = (ids ?? string.Empty).Trim(',', ' ');

            if (string.IsNullOrEmpty(ids)) throw new ArgumentNullException("传入的ids无效！");

            return GetInfos(ids.Split(',').OfType<long>().ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual IList<SYSMenu> GetInfos(IList<long> ids)
        {
            var q = from d in _menuBO.Entities
                    where ids.Any(b => d.ID == b)
                    select d;

            return q.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual SYSMenu GetInfo(long id)
        {
            var q = from d in _menuBO.Entities
                    where d.ID == id
                    select d;

            return q.FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Delete(long id)
        {
            return _menuBO.Remove(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual bool Delete(string ids)
        {
            return _menuBO.Remove(new IDSpec<SYSMenu>(ids));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual bool Delete(IList<long> ids)
        {
            return _menuBO.Remove(new IDSpec<SYSMenu>(ids));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual long Save(SYSMenu info)
        {
            if (info.ParentID > 0)
            {
                var pInfo = GetInfo(info.ID.Value);

                if (pInfo != null)
                {
                    info.Path = string.Concat(pInfo.Path, ",", pInfo.ID.ToString(), ",");

                    info.Layer = ++pInfo.Layer;
                }
            }

            return (info.ID > 0 ? _menuBO.Modify(info) : _menuBO.Add(info)) ? info.ID.Value : 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public virtual bool Save(params SYSMenu[] infos)
        {
            if (infos == null || !infos.Any()) throw new ArgumentNullException("参数不能为空！");

            var result = true;

            using (var dbContext = BoundedContext.Create())
            {
                var bo = dbContext.Repository<SYSMenu>();

                foreach (var info in infos)
                {
                    result = Save(bo, info);

                    if (!result) break;
                }

                if (result) dbContext.Commit();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool Save(IRepository<SYSMenu> bo, SYSMenu info)
        {
            if (info.ParentID > 0)
            {
                var pInfo = GetInfo(info.ID.Value);

                if (pInfo != null)
                {
                    info.Path = string.Concat(pInfo.Path, ",", pInfo.ID.ToString(), ",");

                    info.Layer = ++pInfo.Layer;
                }
            }

            return info.ID > 0 ? _menuBO.Modify(info) : _menuBO.Add(info);
        }











        /*
        int unitID = 0;
        protected IRepository<SYSMenu> _moduleRepository;

        #region 保存排序

        public void SaveSequence(List<int> ids, List<int> values)
        {
            StringBuilder sbSql = new StringBuilder();
            for (int i = 0; i < ids.Count; i++)
            {
                sbSql.Append("update SYSModule set Sequence=" + values[i] + " where ID=" + ids[i] + ";");
            }

            _moduleRepository.UnitOfWork.Execute(DBBuilder.Define(sbSql.ToString()));
        }

        #endregion

        #region 删除用户的模块权限

        public void DeleteUserModules(List<int> ids)
        {
            StringBuilder sbSql = new StringBuilder();
            for (int i = 0; i < ids.Count; i++)
            {
                sbSql.Append("Delete From SYSModuleAccess where UserRoleID=" + ids[i] + ";");
            }

            _moduleRepository.UnitOfWork.Execute(DBBuilder.Define(sbSql.ToString()));
        }

        #endregion

        #region 获得所有模块列表
        /// <summary>
        /// 获得所有模块列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetList()
        {
            string strSql = "select * from SYSModule order by sequence";
            return _moduleRepository.UnitOfWork.ToDataTable(DBBuilder.Define(strSql));
        }
        #endregion

        #region 获得某一级下的所有模块子列表
        /// <summary>
        /// 获得某一级下的所有模块子列表
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public DataTable GetList(int parentID)
        {
            string strSql = "select * from SYSModule where ParentID=" + parentID + " order by sequence";
            return _moduleRepository.UnitOfWork.ToDataTable(DBBuilder.Define(strSql));
        }

        #region 获得某一级的模块子列表和自己
        /// <summary>
        /// 获得某一级的模块子列表和自己
        /// </summary>
        /// <param name="parentID">上级模块ID</param>
        /// <returns></returns>
        public DataTable GetLists(int parentID)
        {

            string strSql = "select * from SYSModule where ID = " + parentID + " or  ParentID=" + parentID + " order by  layer,sequence";
            DataTable dt = _moduleRepository.UnitOfWork.ToDataTable(DBBuilder.Define(strSql));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int sLen = (int)dt.Rows[i]["Name"].ToString().Length;
                int layer = (int)dt.Rows[i]["Layer"];
                dt.Rows[i]["Name"] = dt.Rows[i]["Name"].ToString().PadLeft((layer * 3) + sLen, '　');
            }
            return dt;
        }
        #endregion

        //public DataView GetList(int parentID, int userID, int UnitID)
        //{
        //    if (!Register.IsRegiste())
        //    {
        //        return new DataView();
        //    }
        //    DataView dv = new DataView(_moduleRepository.GetList(parentID));
        //    UserBO user = new UserBO();
        //    RoleBO role = new RoleBO();
        //    List<int> moduleIDs = user.GetModuleIDs(userID, role, UnitID);
        //    string strModuleIDs = "0";
        //    foreach (int id in moduleIDs)
        //    {
        //        strModuleIDs += "," + id;
        //    }

        //    dv.RowFilter = "ID in (" + strModuleIDs + ")";
        //    return dv;
        //}
        #endregion

        #region 返回拥有权限的第一个模块的ID
        ///// <summary>
        ///// 返回拥有权限的第一个模块的ID
        ///// </summary>
        ///// <param name="parentID"></param>
        ///// <param name="userID"></param>
        ///// <param name="UnitID"></param>
        ///// <returns></returns>
        //public int GetFirstModuleID(int parentID, int userID, int UnitID)
        //{
        //    DataView dv = new DataView(_moduleRepository.GetList(parentID));
        //    UserBO user = new UserBO();
        //    RoleBO role = new RoleBO();
        //    List<int> moduleIDs = user.GetModuleIDs(userID, role, UnitID);
        //    string strModuleIDs = "0";
        //    foreach (int id in moduleIDs)
        //    {
        //        strModuleIDs += "," + id;
        //    }

        //    dv.RowFilter = "ID in (" + strModuleIDs + ")";
        //    if (dv.Count > 0)
        //    {
        //        return int.Parse(dv[0]["ID"].ToString());
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}
        #endregion

        #region 获得模块图标的列表
        public DataTable GetIcons()
        {
            string strIcoPath = HttpContext.Current.Server.MapPath(@"~\images\Module");
            DirectoryInfo dirInfo = new DirectoryInfo(strIcoPath);
            FileInfo[] files = dirInfo.GetFiles();
            DataTable dtIcon = new DataTable();
            dtIcon.Columns.Add("url");
            foreach (FileInfo file in files)
            {
                DataRow dr = dtIcon.NewRow();
                if (file.Extension.ToLower() == ".gif" || file.Extension.ToLower() == ".jpg" || file.Extension.ToLower() == ".png")
                {
                    dr["url"] = "~\\images\\module\\" + file.Name;
                    dtIcon.Rows.Add(dr);
                }
            }
            return dtIcon;
        }
        #endregion

        #region 获得根ID
        /// <summary>
        /// 获得根ID
        /// </summary>
        /// <returns></returns>
        public int GetRootID()
        {
            string strSql = "select ID from SYSModule where ParentID=0";
            DataTable dt = _moduleRepository.UnitOfWork.ToDataTable(DBBuilder.Define(strSql));
            int iRootID;
            if (dt.Rows.Count > 0)
            {
                iRootID = (int)dt.Rows[0]["ID"];
            }
            else
            {
                strSql = "insert into SYSModule (Name, ParentID, Layer, Path, Sequence, IsPop, IsUtt, MainTable,UnitID) " +
                           "values('根模块',0,0,',',0,0,0,'SYSModule',1)";
                _moduleRepository.UnitOfWork.Execute(DBBuilder.Define(strSql));
                iRootID = GetRootID();
            }
            return iRootID;
        }
        #endregion

        #region 从模块权限表中清楚模块信息
        /// <summary>
        /// 从模块权限表中清楚模块信息
        /// </summary>
        /// <param name="moduleID"></param>
        public void ClearModule(List<int> lstModuleIDs)
        {
            string strIDs = "0";
            foreach (int id in lstModuleIDs)
            {
                strIDs += "," + id;
            }
            string strSQL = "Delete From SYSModuleAccess Where ModuleID in (" + strIDs + ")";
            _moduleRepository.UnitOfWork.Execute(DBBuilder.Define(strSQL));
        }
        #endregion

        #region 判断是否是通用模块
        /// <summary>
        /// 判断是否是通用模块
        /// </summary>
        /// <param name="moduleID"></param>
        /// <returns></returns>
        public bool IsCommonModule(int moduleID)
        {
            int temp;
            string strSQL = "Select UnitID From SYSModule Where ID=" + moduleID;
            temp = Convert.ToInt32(_moduleRepository.UnitOfWork.ToScalar(DBBuilder.Define(strSQL)));

            if (temp == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 获取模块信息

        public IEnumerable<SYSMenu> GetRootModules(long userID)
        {
            if (userID <= 0) throw new ArgumentOutOfRangeException("用户ID无效");

            var sql = @"CanShow=1
                        AND Layer=1
                        AND EXISTS ( SELECT 1
                                 FROM   SYSModuleAccess ma
                                 WHERE  ma.ModuleID = SYSModule.ID
                                        AND ma.SystemID = SYSModule.SystemID
                                        AND EXISTS ( SELECT 1
                                                     FROM   RoleUser ru
                                                     WHERE  ru.UserID = @UserID
                                                            AND ru.RoleID = ma.UserRoleID
                                                            AND ru.SystemID = ma.SystemID ) )
                         ";

            return _moduleRepository.GetAll(new Spec<SYSMenu>(sql, new { UserID = userID }), new SortExpression<SYSMenu>("Sequence"));
        }

        public IEnumerable<SYSMenu> GetModulesByUserID(long userID, long pid)
        {
            if (userID <= 0 || pid <= 0) return null;

            var sql = @"CanShow=1
                        AND EXISTS ( SELECT 1
                                 FROM   SYSModuleAccess ma
                                 WHERE  ma.ModuleID = SYSModule.ID
                                        AND ma.SystemID = SYSModule.SystemID
                                        AND EXISTS ( SELECT 1
                                                     FROM   RoleUser ru
                                                     WHERE  ru.UserID = @UserID
                                                            AND ru.RoleID = ma.UserRoleID
                                                            AND ru.SystemID = ma.SystemID ) )
                        AND (ID=" + pid.ToString() + " OR CHARINDEX('," + pid.ToString() + ",',','+ISNULL([Path],'')+',',0)>0) ";

            return _moduleRepository.GetAll(new Spec<SYSMenu>(sql, new { UserID = userID }), new SortExpression<SYSMenu>("Path"));
        }

        public IEnumerable<SYSMenu> GetModuleRouteByUserID(long userID)
        {
            if (userID <= 0) return null;

            var sql = @"EXISTS ( SELECT 1
                                  FROM   SYSModuleAccess ma
                                  WHERE  ma.ModuleID = SYSModule.ID
                                         AND ma.SystemID = SYSModule.SystemID
                                         AND EXISTS ( SELECT 1
                                                      FROM   RoleUser ru
                                                      WHERE  ru.UserID = @UserID
                                                             AND ru.RoleID = ma.UserRoleID
                                                             AND ru.SystemID = ma.SystemID ) )
                        AND (SYSModule.NavUrl IS NOT NULL OR SYSModule.ToolBarUrl IS NOT NULL OR SYSModule.ContainerUrl IS NOT NULL)";

            return _moduleRepository.GetAll(new Spec<SYSMenu>(sql, new { UserID = userID }), new SortExpression<SYSMenu>("Path"));
        }

        public PagedList<SYSMenu> GetModuleByPage(PagedQuery info)
        {
            return _moduleRepository.GetPaged(new AppMenuNameSpec(info.Key), info.PageIndex ?? 1, info.PageSize ?? 15);

        }

        #endregion 获取模块信息

        #region 数据维护

        public long Delete(long id)
        {
            try
            {
                _moduleRepository.Remove(id);
                return id;
            }
            catch { return 0; }
        }

        public string Delete(string ids)
        {
            try
            {
                _moduleRepository.Remove(new IDSpec<SYSMenu>(ids));
                return ids;
            }
            catch { return string.Empty; }
        }

        public SYSMenu GetInfo(long id)
        {
            return _moduleRepository.Get(id);
        }

        public long Save(SYSMenu info)
        {
            try
            {
                if (!(info.ID > 0)) _moduleRepository.Add(info);

                if (info.ParentID > 0)
                {
                    var pInfo = _moduleRepository.Get(info.ParentID.Value);
                    if (pInfo != null) info.Path = string.Concat(pInfo.Path, info.ID.ToString(), ",");
                }

                _moduleRepository.Modify(info);

                return info.ID.Value;
            }
            catch { return 0; }

        }

        #endregion 数据维护
         */

    }
}
