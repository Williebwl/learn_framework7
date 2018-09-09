
namespace BIStudio.Framework.Security.Organization
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;
    using System.Xml;
    using BIStudio.Framework.Data.Adapter;

    public class EventLogBO : DBGeneralProxy, IEventLog
    {
        public EventLogBO()
        {
        }

        public long Save(OperateLogInfo operateLogInfo)
        {
            base.Save(operateLogInfo);
            return operateLogInfo.ID.Value;
        }

        public long Save(LoginLogInfo loginLogInfo)
        {
            base.Save(loginLogInfo);
            return loginLogInfo.ID.Value;
        }

        public static long Save(int unitid, OperateEnum.OperateType operateType, int userID, string userName, string tableBind, string operateContent)
        {
            long id = DBHelperProxy.GetMaxID("OperateLog");
            DBHelperProxy.ExecuteNonQuery(string.Format("INSERT INTO OperateLog(ID,UnitID,OperateType,UserID,UserName,TableBind,OperateTime,OperateContent) VALUES({0},{1},{2},{3},'{4}','{5}','{6}','{7}')  ",id,unitid,Convert.ToInt32(operateType), userID, userName, tableBind, DateTime.Now, operateContent));
            return id;
        }

        public DataTable GetLogStat(string xmlpath)
        {
            return GetLogStat();
        }

        public DataTable GetLogStat()
        {
            DataTable dtTotal = new DataTable("LogTotal");

            DataColumn dc = new DataColumn("Desc");
            dtTotal.Columns.Add(dc);
            dc = new DataColumn("Total");
            dtTotal.Columns.Add(dc);

            string sql = "select * from " + DBHelperProxy.FormatFunction("gettables");
            DataTable dt = DBHelperProxy.GetDataTable(sql);

            DataRow drr;

            foreach (DataRow dr in dt.Rows)
            {
                drr = dtTotal.NewRow();
                drr["Desc"] = dr["table_name"];
                drr["Total"] = DBHelperProxy.ExecuteScalar(string.Format("select count(*)  as total from " + DBHelperProxy.FormatTable("{0}"), dr["Name"].ToString()));
                dtTotal.Rows.Add(drr);
            }

            return dtTotal;
        }

        public DataTable GetLoginLogs()
        {
            return DBHelperProxy.GetDataTable("Select LoginTime,UserID,L.UserName as LoginName,L.IP,U.UserName,U.DeptName From LoginLog L  Inner Join " + DBHelperProxy.FormatTable("User") + " U On L.UserID=U.ID Order By L.ID Desc");
        }

        public DataTable GetLoginLogs(string year, string month, string deptid, string username)
        {
            string sql = "Select LoginTime,UserID,U.LoginName,L.IP,U.UserName,U.DeptName From LoginLog L  Inner Join " + DBHelperProxy.FormatTable("User") + " U On L.UserID=U.ID Where 1=1";
            if (!string.IsNullOrEmpty(year))
            {
                sql += " and " + DBHelperProxy.FormatFunction("getyear", "LoginTime") + " =" + year;
            }

            if (!string.IsNullOrEmpty(month))
            {
                sql += " and " + DBHelperProxy.FormatFunction("getmonth", "LoginTime") + " =" + month;
            }

            if (!string.IsNullOrEmpty(deptid))
            {
                sql += " and U.DeptID=" + deptid;
            }

            if (!string.IsNullOrEmpty(username))
            {
                sql += " and U.UserName='" + username + "'";
            }

            sql += " Order By L.ID Desc";
            return DBHelperProxy.GetDataTable(sql);
        }

        public DataTable GetLoginStat(string year, string month, string deptid)
        {
            string sql = "Select Count(*) as Total,U.LoginName,U.UserName,U.DeptName from LoginLog L Inner Join " + DBHelperProxy.FormatTable("User") + " U On L.UserID=U.ID Where 1=1";
            if (!string.IsNullOrEmpty(year))
            {
                sql += " and " + DBHelperProxy.FormatFunction("getyear", "LoginTime") + " =" + year;
            }

            if (!string.IsNullOrEmpty(month))
            {
                sql += " and " + DBHelperProxy.FormatFunction("getmonth", "LoginTime") + " =" + month;
            }

            if (!string.IsNullOrEmpty(deptid))
            {
                sql += " and U.DeptID=" + deptid;
            }

            sql += " Group by L.UserID,U.LoginName,U.Username,U.DeptName  Order by Total desc";
            
            return DBHelperProxy.GetDataTable(sql);
        }
         
        public DataTable GetOperateLogs()
        {
            return DBHelperProxy.GetDataTable("Select  ID,UnitID,OperateType,UserID,UserName,TableBind,OperateTime,OperateContent From " + DBHelperProxy.FormatTable("OperateLog") + " Order By ID Desc");
        }

        public DataTable GetOperateLogs(string tablename, string oType, string year, string month)
        {
            string sql = "Select  ID,UnitID,OperateType,UserID,UserName,TableBind,OperateTime,OperateContent From " + DBHelperProxy.FormatTable("OperateLog") + " Where 1=1 ";

            if (!string.IsNullOrEmpty(tablename))
            {
                sql += "  and TableBind='" + tablename + "'";
            }

            if (!string.IsNullOrEmpty(oType))
            {
                sql += "  and OperateType=" + oType;
            }

            if (!string.IsNullOrEmpty(year))
            {
                sql += " and " + DBHelperProxy.FormatFunction("getyear", "OperateTime") + " =" + year;
            }

            if (!string.IsNullOrEmpty(month))
            {
                sql += " and " + DBHelperProxy.FormatFunction("getmonth", "OperateTime") + " =" + month;
            }

            sql += " Order By ID Desc";

            return DBHelperProxy.GetDataTable(sql);
        }

        public DataTable GetOperateLogs(string tablename, string oType)
        {
            string sql = "Select  ID,UnitID,OperateType,UserID,UserName,TableBind,OperateTime,OperateContent From " + DBHelperProxy.FormatTable("OperateLog") + " Where 1=1 ";

            if (!string.IsNullOrEmpty(tablename))
            {
                sql += "  and TableBind='" + tablename + "'";
            }

            if (!string.IsNullOrEmpty(oType))
            {
                sql += "  and OperateType=" + oType;
            }
             
            sql += " Order By ID Desc";

            return DBHelperProxy.GetDataTable(sql);
        }

        public DataTable GetOperateLogs(string tablename)
        {
            return GetOperateLogs(tablename, "","","");
        }
    }
}
