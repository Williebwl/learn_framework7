namespace BIStudio.Framework.Security.Organization
{
	using System;
    using System.Text;
	using System.Data;
	using System.Collections.Generic;
    using BIStudio.Framework.Data.Adapter;
	/// <summary>
	///  在这里添加类说明。
	/// </summary>
	/// <remarks>
	/// 上海拜特信息技术有限公司[2011-2-22]
	/// </remarks>
	/// <remarks>
	/// </remarks>
 
	public class ActionSourceBO:DBGeneralProxy
	{  
        private int unitID = 0;
		public ActionSourceBO()
		{ 

		}

        public ActionSourceBO(int unitID)
        {
            this.unitID = unitID;
        }
 
        public DataTable GetActionSourceBelongs()
        {
            return DBHelperProxy.GetDataTable("Select  ID,Name,Code,UnitID From ActionSourceBelong");
        }

        public DataTable GetActionSources(int bid)
        {
            return DBHelperProxy.GetDataTable("Select  ID,BelongID,Name,Code,CodeExt,UnitID From ActionSource Where BelongID=" + bid);
        }

        public void Delete(string ids)
        {
            this.Delete<ActionSourceBelongInfo>(ids); 
            this.DeleteByForeignKey<ActionSourceInfo>(ids);
        }

        public void SaveAccess(List<int> rids,int role,int unitid)
        {
            StringBuilder sbSql = new StringBuilder("delete from ActionSourceAccess where RoleID =" + role + " And UnitID=" + unitid + "; ");

            foreach (int id in rids)
            {
                sbSql.Append("insert into ActionSourceAccess(RoleID,SourceID, UnitID) values(" + role + "," + id + "," + unitid + "); ");
            }

            DBHelperProxy.ExecuteNonQuery(sbSql.ToString());
        }

        public List<int> GetRoleAccess(int role, int unitid)
        {
            List<int> rids = new List<int>();
            string sql =  "select SourceID from ActionSourceAccess where RoleID =" + role + " And UnitID=" + unitid + "; ";
            DataTable dt = DBHelperProxy.GetDataTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                rids.Add(int.Parse(dr[0].ToString()));
            }

            return rids;
        }
	}
}