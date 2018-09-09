
namespace BIStudio.Framework.Security.Organization
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;
    using System.Web;
    using BIStudio.Framework.Data.Adapter;

    public class RoleBO : DBGeneralProxy, IModuleAccess, IRole
    {
        int unitID = 0;

        public RoleBO()
        {
        }

        public RoleBO(int unitID)
        {
            this.unitID = unitID;
        }
 
        #region 获得角色列表
        /// <summary>
        /// 获得角色列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetList()
        {
            string strSql = "SELECT ID,Name,Code,TypeID,Flag,Sequence, UnitID,Remark FROM Role Where Code<>'Developor' And Code<>'System'  And Code<>'Audit'  And Code<>'Admin' And (UnitID =0 Or UnitID=" + this.unitID + ") order by sequence";
            return DBHelperProxy.GetDataSet(strSql).Tables[0];    
        }

        public DataTable GetList(long typeid)
        {
            string strSql = "SELECT ID,Name,Code,Sequence,TypeID,Flag, UnitID,Remark FROM Role Where Code<>'Developor' And Code<>'System'  And Code<>'Audit'  And Code<>'Admin' And (UnitID =0 Or UnitID=" + this.unitID + ")";
            if (typeid != 0)
            {
                strSql += " And TypeID=" + typeid;
            }
            strSql += " order by sequence";
            return DBHelperProxy.GetDataSet(strSql).Tables[0];
        }

        /// <summary>
        /// 获得某个用户的所有角色
        /// </summary>
        /// <returns></returns>
        public  List<int> GetUserRoles(int userID, int UnitID)
        {
            string strSql = "select RoleID from RoleUser where UserID=" + userID ;
            List<int> roleIDs = new List<int>();
            DataTable dt = DBHelperProxy.GetDataSet(strSql).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                roleIDs.Add((int)dr["RoleID"]);
            }
            return roleIDs;  
        }
 
        #endregion

        #region 通过角色代码获得角色ID
        /// <summary>
        /// 通过角色代码获得角色ID
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public int GetRoleIDByCode(string code)
        {
            string strSQL = "Select ID From Role Where Code='" + code + "'";
            return Convert.ToInt32(DBHelperProxy.ExecuteScalar(strSQL));
        }
        #endregion

        #region 删除角色
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="lstSelectIDs"></param>
        public void Delete(List<int> lstSelectIDs)
        {
            string strIDs = "0";
            foreach (int id in lstSelectIDs)
            {
                strIDs += "," + id;
            }
            string strSql = "delete from Role where id IN(" + strIDs + ");delete from RoleUser where RoleID IN(" + strIDs + ");";
            DBHelperProxy.ExecuteNonQuery(strSql);
        }
        #endregion

        #region 获得角色类型
        /// <summary>
        /// 获得角色类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetRoleTypes()
        {
            string strSql = "select * from RoleType Order By id asc";
            return DBHelperProxy.GetDataTable(strSql);
        }
        #endregion

        #region 获得角色模块ID集合
        /// <summary>
        /// 获得角色模块ID集合
        /// </summary>
        /// <param name="iRoleID"></param>
        /// <returns></returns>
        public List<int> GetModuleIDs(int iRoleID, int sUnitID)
        {
            string strSql = "select ModuleID from ModuleAccess where flag=1 and UserRoleID=" + iRoleID + " And UnitID=" + sUnitID;
            DataTable dt = DBHelperProxy.GetDataSet(strSql).Tables[0];
            List<int> lstModuleIDs = new List<int>();
            foreach (DataRow dr in dt.Rows)
            {
                lstModuleIDs.Add((int)dr["ModuleID"]);
            }
            return lstModuleIDs;
        }

        public List<int> GetModuleIDs(List<int> RoleIDs,int UnitID)
        {
            string strRoleIDs = "0";
            foreach (int id in RoleIDs)
            {
                strRoleIDs += "," + id;
            }
            string strSql = "select ModuleID from ModuleAccess where flag=1 and UserRoleID in (" + strRoleIDs + ") And (UnitID =0 Or UnitID=" + UnitID + ")";
            DataTable dt = DBHelperProxy.GetDataSet(strSql).Tables[0];
            List<int> ModuleIDs = new List<int>();
            foreach (DataRow dr in dt.Rows)
            {
                ModuleIDs.Add((int)dr["ModuleID"]);
            }
            return ModuleIDs;       
        }
        #endregion

        #region 保存角色模块集合
        /// <summary>
        /// 保存角色模块集合
        /// </summary>
        /// <param name="iRoleID"></param>
        /// <param name="lstModuleIDs"></param>
        public void SaveModuleIDs(int iRoleID, List<int> lstModuleIDs, int sUnitID)
        {
            StringBuilder sbSql = new StringBuilder("delete from ModuleAccess where flag=1 and UserRoleID =" + iRoleID + " And UnitID=" + sUnitID + "; ");

            foreach (int id in lstModuleIDs)
            {
                sbSql.Append("insert into ModuleAccess(UserRoleID,ModuleID,flag, UnitID) values(" + iRoleID + "," + id + ",1, " + sUnitID + "); ");
            }

            DBHelperProxy.ExecuteNonQuery(sbSql.ToString());
        }
        #endregion

        #region 获得角色用户
        /// <summary>
        /// 获得角色用户
        /// </summary>
        /// <param name="iRoleID"></param>
        /// <returns></returns>
        public List<int> GetUserIDs(int iRoleID)
        {
            string strSql = "select UserID from RoleUser where RoleID=" + iRoleID + " And (UnitID =0 Or UnitID=" + this.unitID + ")";
            DataTable dt = DBHelperProxy.GetDataSet(strSql).Tables[0];
            List<int> lstUserIDs = new List<int>();
            foreach (DataRow dr in dt.Rows)
            {
                lstUserIDs.Add((int)dr["UserID"]);
            }
            return lstUserIDs;
        }

        public List<int> GetUserIDs(string roleCode)
        {
            string strSql = "select U.UserID from Role R, RoleUser U where R.ID=U.RoleID And R.Code='" + roleCode + "' And (R.UnitID =0 Or R.UnitID=" + this.unitID + ")";
            DataTable dt = DBHelperProxy.GetDataSet(strSql).Tables[0];
            List<int> lstUserIDs = new List<int>();
            foreach (DataRow dr in dt.Rows)
            {
                lstUserIDs.Add((int)dr["UserID"]);
            }
            return lstUserIDs;
        }

        public DataTable GetRoleUsers(string roleCode)
        {
            string strSql = "select Us.ID,Us.UserName from Role R, RoleUser U," + DBHelperProxy.FormatTable("User") + " Us where R.ID=U.RoleID and U.UserID=Us.ID And R.Code='" + roleCode + "' And (R.UnitID =0 Or R.UnitID=" + this.unitID + ")";
            DataTable dt = DBHelperProxy.GetDataSet(strSql).Tables[0]; 
            return dt;
        }
        #endregion

        #region 保存角色用户
        /// <summary>
        /// 保存角色用户
        /// </summary>
        /// <param name="iRoleID"></param>
        /// <param name="lstUserIDs"></param>
        public void SaveUserIDs(int iRoleID, List<int> lstUserIDs, int sUnitID)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("delete from RoleUser where RoleID=" + iRoleID + " And UnitID=" + sUnitID);
            foreach (int iUserID in lstUserIDs)
            {
                sbSql.Append(";insert into RoleUser(RoleID,UserID, UnitID) values(" + iRoleID + "," + iUserID + ", " + sUnitID + ") ");
            }
            DBHelperProxy.ExecuteNonQuery(sbSql.ToString());
        }

        /// <summary>
        /// 保存角色用户到指定角色里
        /// </summary>
        /// <param name="roleCode"></param>
        /// <param name="userID"></param>
        /// <param name="sUnitID"></param>
        public void SaveRoleUser(string roleCode, int userID, int sUnitID)
        {
            string sbSql = "insert into RoleUser(RoleID,UserID, UnitID) values(" + Convert.ToInt32(DBHelperProxy.ExecuteScalar("Select ID From Role where code='" + roleCode + "'")) + "," + userID + ", " + sUnitID + ")";
            DBHelperProxy.ExecuteNonQuery(sbSql.ToString());
        }

        #endregion
 
        #region 判断是否是所有单位通用角色
        /// <summary>
        /// 判断是否是所有单位通用角色
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public bool IsCommonRole(int roleID)
        {
            string strSQL = "Select UnitID From Role Where ID=" + roleID;
            return (Convert.ToInt32(DBHelperProxy.ExecuteScalar(strSQL)) == 0);  
        } 
        #endregion

        #region 将用户从角色用户表中剔除
        /// <summary>
        /// 将用户从角色用户表中剔除
        /// </summary>
        /// <param name="DelUserIDS"></param>
        /// <param name="UnitID"></param>
        public void DelUserFromRole(List<int> delUserIDS, int unitID)
        {
            string strIDS = "0";
            foreach (int id in delUserIDS)
            {
                strIDS += "," + id;
            }
            string strSQL = "Delete From RoleUser Where UserID In (" + strIDS + ") And UnitID=" + unitID;
            DBHelperProxy.ExecuteNonQuery(strSQL);
        } 
        #endregion

        #region 接口实现

            public bool IsAllUser(int userID)
            {
                return this.IsInRole("AllUser", userID);
            }

            public bool IsAdmin(int userID)
            {
                return this.IsInRole("Admin", userID);
            }

            public bool IsUnitLeader(int userID)
            {
                return this.IsInRole("UnitLeader", userID);
            }

            public bool IsDeptLeader(int userID)
            {
                return this.IsInRole("DeptLeader", userID);
            }

            #region 根据用户ID和角色代码判断用户是否在这个角色中
            /// <summary>
            /// 根据用户ID和角色代码判断用户是否在这个角色中,苏登刚
            /// </summary>
            /// <param name="roleCode"></param>
            /// <param name="userID"></param>
            public bool IsInRole(string roleCode, int userID)
            {
                string strSQl = "select Code FROM Role R LEFT OUTER JOIN  RoleUser U ON R.ID = U.RoleID WHERE (R.Code = '" + roleCode + "') AND (U.UserID = " + userID + ")";
                return (DBHelperProxy.GetDataTable(strSQl).Rows.Count > 0);
            }
            #endregion

        #endregion
 
    }
}
