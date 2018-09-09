

namespace BIStudio.Framework.Security.Organization
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;
    using BIStudio.Framework.Data.Adapter;
    using BIStudio.Framework.Utils;
    using BIStudio.Framework.Core;

    public class DeptBO : ISYSDept
    {
        int unitID =0;
        public DeptBO()
        { }

        public DeptBO(int unitID)
        {
            this.unitID = unitID;
        }

        #region �������� ��ɾ�Ĳ�

        #region ����͸���
        /// <summary>
        /// ����͸���
        /// </summary>
        /// <param name="deptInfo"></param>
        /// <returns></returns>
        public long Save(SYSDeptInfo deptInfo)
        {
            long id = deptInfo.ID.Value; 

            base.Save(deptInfo);

            if (deptInfo.IsUnit == 1)
            {
                deptInfo.UnitID = deptInfo.ID;
                deptInfo.ParentID = 0;
                deptInfo.Layer = 0;
                deptInfo.Path = ",-1," + deptInfo.ID + ",";
                
            }
            else
            {
                DeptInfo tempInfo = GetInfo<DeptInfo>(deptInfo.ParentID.Value);
                deptInfo.Layer = tempInfo.Layer + 1;
                deptInfo.Path = tempInfo.Path + deptInfo.ID + ",";
            }

            base.Save(deptInfo);

            if (id != 0)
            {   //����user��
                string strSql = "UPDATE " + DBHelperProxy.FormatTable("User") + " SET DeptName = " + DBHelperProxy.FormatParameter("Name") + " WHERE DeptID=" + DBHelperProxy.FormatParameter("ID") + ";UPDATE " + DBHelperProxy.FormatTable("User") + " SET SLDeptName = " + DBHelperProxy.FormatParameter("Name") + " WHERE SLDeptID=" + DBHelperProxy.FormatParameter("ID") + ";";
                DBHelperProxy.ExecuteScalar(strSql, DBHelperProxy.CreateParameter("Name", deptInfo.Name), DBHelperProxy.CreateParameter("ID", deptInfo.ID.Value));
            }

            if (id == 0 && deptInfo.IsUnit == 1)
            {
                //������λʱ�Զ�����һ����λ����Ա
                NewAdminOfUnit(deptInfo.ID.Value, deptInfo.ID.Value, deptInfo.Name); 
            }

            return deptInfo.ID.Value;
        }
        #endregion

        #region ��������
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="IDs"></param>
        /// <param name="values"></param>
        public void SaveSequence(List<int> ids, List<int> values)
        {
            StringBuilder sbSql = new StringBuilder();
            for (int i = 0; i < ids.Count; i++)
            {
                sbSql.Append("update Dept set Sequence=" + values[i] + " where ID=" + ids[i] + ";");
            }

            DBHelperProxy.ExecuteNonQuery(sbSql.ToString());
        }
        #endregion     

        #region ��ñ���λ���в����б�
        /// <summary>
        /// ��ñ���λ���в����б�
        /// </summary>
        /// <returns></returns>
        public DataTable GetList()
        {
            string strSql = "select * from Dept where ID>0 and UnitID=" + this.unitID.ToString() + "  order by sequence";
            return DBHelperProxy.GetDataSet(strSql).Tables[0]; 
        }

        /// <summary>
        /// ������в����б�
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllList()
        {
            string strSql = "select * from Dept where ID>0  order by sequence";
            return DBHelperProxy.GetDataSet(strSql).Tables[0];
        }

        /// <summary>
        /// ���ĳһ�������µ��Ӳ����б�
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public DataTable GetList(int parentID)
        {
            string strSql = "select * from Dept where ID>0 and ParentID=" + parentID + " order by sequence";
            return DBHelperProxy.GetDataSet(strSql).Tables[0];
        }
        #endregion

        #region ��ȡ���е�λ

        public DataTable GetUnits()
        {
            string strSql = "select * from Dept where isUnit = 1   order by sequence";
            return DBHelperProxy.GetDataSet(strSql).Tables[0];
        }

        #endregion

        #endregion
 
        #region ��������б�
        /// <summary>
        /// ��������б�
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable GetTreeList(int isUnit)
        {
            DataTable dt = GetList();
            DataTable newDt = dt.Clone();

            //��õ�λ�ĸ��ڵ�
            int parentID = 0;
 
            TreeTrans(newDt, dt, parentID, isUnit);
            return newDt;
        }
        #endregion

        #region �ݹ�--�����µ���
        /// <summary>
        /// �ݹ�--�����µ���
        /// </summary>
        /// <param name="newDt"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public void TreeTrans(DataTable newDt, DataTable dt, int parentID, int isUnit)
        {
            int Plot = 1;
            DataView dv = new DataView(dt);
            if (isUnit == 1)
            {
                dv.RowFilter = "UnitID =" + this.unitID + " or  ParentID=" + parentID;
            }
            else
            {
                dv.RowFilter = "ParentID=" + parentID + " And (UnitID=0 Or UnitID=" + this.unitID + ")";
            }
            foreach (DataRowView drv in dv)
            {
                if (isUnit == 1 && (int)drv["ParentID"] > 1)
                {
                    break;
                }
                DataRow dr = newDt.NewRow();
                for (int i = 0; i < newDt.Columns.Count; i++)
                {
                    if (newDt.Columns[i].ColumnName == "Name")
                    {
                        int sLen = (int)drv["Name"].ToString().Length;
                        int layer = (int)drv["Layer"];

                        //����ÿ��ģ����������3��ȫ�ǿո�
                        Plot = layer * 2;
                        dr[i] = drv[i].ToString().PadLeft(Plot + sLen, '��');
                    }
                    else
                    {
                        dr[i] = drv[i];
                    }
                }
                newDt.Rows.Add(dr);
                TreeTrans(newDt, dt, (int)dr["ID"], isUnit);
            }

        }
        #endregion
 
        #region ����Ȩ�޻�����в����б�
        /// <summary>
        /// ����Ȩ�޻�����в����б�
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataTable GetListByRight(int UserID)
        {
            DataTable dt = GetList();
            //����Ȩ�޿���
            return dt;
        }
        #endregion

        #region ����Ȩ�޻��ĳһ���Ĳ����б�
        public DataTable GetListByRight(int UserID, int ParentID)
        {
            DataTable dt = GetList(ParentID);
            //����Ȩ�޿���
            return dt;
        }
        #endregion
     
        #region ��ø�ID
        /// <summary>
        /// ��ø�ID
        /// </summary>
        /// <returns></returns>
        public static int GetRootID()
        {
            string strSql = "select ID from Dept where ParentID=0";
            DataTable dt = DBHelperProxy.GetDataSet(strSql).Tables[0];
            int iRootID;
            if (dt.Rows.Count > 0)
            {
                iRootID = (int)dt.Rows[0]["ID"];
            }
            else
            {
                strSql = "insert into Dept ( Name, ParentID, Layer, Path, Sequence, IsStop) " + "values('������',0,0,',',0,1)";
                DBHelperProxy.ExecuteNonQuery(strSql);
                iRootID = GetRootID();
            }
            return iRootID;
        }
        #endregion

        #region ��ò��������UnitID
        /// <summary>
        /// ��ò��������UnitID
        /// </summary>
        /// <param name="deptID"></param>
        /// <returns></returns>
        public int GetUnitID(int deptID)
        {
            string strSQL = "Select UnitID From Dept Where id=" + deptID;
            return Convert.ToInt32(DBHelperProxy.ExecuteScalar(strSQL));
        } 
        #endregion

        #region ��ø����ŵ�UnitID
        /// <summary>
        /// ��ø����ŵ�UnitID
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public int GetUpperUnitID(int parentID)
        {
            string strSQL = "Select UnitID From Dept Where id=" + parentID;
            return Convert.ToInt32(DBHelperProxy.ExecuteScalar(strSQL));
        } 
        #endregion
 
        #region ����UnitID��õ�λ����
        /// <summary>
        /// ����UnitID��õ�λ����
        /// </summary>
        /// <param name="unitID"></param>
        /// <returns></returns>
        public string GetUnitName(int unitID)
        {
            string strSQL = "Select Name From Dept Where (UnitID=0 or UnitID=" + unitID + ") And IsUnit=1";
            return DBHelperProxy.ExecuteScalar(strSQL).ToString();
        } 
        #endregion

        #region ���ò����Ƿ����û�
        /// <summary>
        /// ���ò����Ƿ����û�
        /// </summary>
        /// <param name="deptID"></param>
        /// <returns></returns>
        public bool IsHavUserInDept(int deptID)
        {
            int sCount = 0;
            string strSQL = "Select Count(*) From " + DBHelperProxy.FormatTable("User") + " Where DeptID=" + deptID;
            sCount = Convert.ToInt32(DBHelperProxy.ExecuteScalar(strSQL));
            return sCount > 0; 
        } 
        #endregion
  
        #region ��ȡ�ض���λ�µĲ���ID Name Sequence
        /// <summary>
        /// ��ȡ�ض���λ�µĲ���ID Name Sequence
        /// ����ר��
        /// </summary>
        /// <returns></returns>
        public DataTable GetDict()
        {
            string strSql = "select ID,Name,sequence from Dept where unitID=" + unitID + " order by sequence";
            return DBHelperProxy.GetDataSet(strSql).Tables[0];
        }
        #endregion

        #region ͨ��uid��ȡ���β��Ÿ����˵Ĳ�������
        /// <summary>
        /// ͨ��uid��ȡ���β��Ÿ����˵Ĳ�������
        /// </summary>
        /// <param name="leaderid"></param>
        /// <returns></returns>
        public string GetDeptName(int leaderid)
        {
            string strSQL = "select top 1 Name from Dept where LeaderID=" + leaderid.ToString();
            object obj = DBHelperProxy.ExecuteScalar(strSQL);
            if (obj != null && obj != DBNull.Value)
            {
                return obj.ToString();
            }
            return string.Empty;
        }
        #endregion

        #region �½���λʱ�Զ�����һ����λ����Ա
        /// <summary>
        /// �½���λʱ�Զ�����һ����λ����Ա
        /// </summary>
        /// <param name="deptID">����ID</param>
        /// <param name="unitID">��λID</param>
        public void NewAdminOfUnit(long deptID, long unitID, string unitName)
        {
            long newUserID;
            string userName = "admin" + unitID;
            string password = ALEncrypt.Md5hash(AppConfig.GetConfig("InitPassword"));
            newUserID = DBHelperProxy.GetMaxID("User");
            string strSQL = "Insert Into " + DBHelperProxy.FormatTable("User") + "(ID,UserName,LoginName," + DBHelperProxy.FormatField("Password") + ",DeptID,DeptName,UnitID,IsStop," + DBHelperProxy.FormatField("Sequence") + ") Values(" + newUserID.ToString() + ", '����Ա[" + unitID + "]','" + userName + "','" + password + "'," + deptID + ",'" + unitName + "'," + unitID + ",1,100);";

            DBHelperProxy.ExecuteScalar(strSQL);

            //��ӵ�����Ա��ɫ��
            AddUserToAdminRole(newUserID, unitID);
        }

        public void AddUserToAdminRole(long userID, long unitID)
        {
            RoleBO role = new RoleBO();
            long roleID = Convert.ToInt32(role.GetRoleIDByCode("Admin"));
            string strSQL = "Insert Into RoleUser(RoleID, UserID, UnitID) Values(" + roleID + "," + userID + "," + unitID + ")";
            DBHelperProxy.ExecuteNonQuery(strSQL);
        }
        #endregion
 
        #region ���·���ʵ�ֽӿ�

        #region ����idȡ��������
        /// <summary>
        /// ����idȡ��������
        /// </summary>
        /// <param name="deptID"></param>
        /// <returns></returns>
        public string GetDeptNameByID(int deptID)
        {
            string strSQL = "Select Name from " + DBHelperProxy.FormatTable("Dept") + " where ID=" + deptID.ToString();
            object name = DBHelperProxy.ExecuteScalar(strSQL);
            if (name != null && name != DBNull.Value)
            {
                return name.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        #region ����idsȡ���еĲ�������,������ʽ"1,2,3",����"һ��,����,����"
        /// <summary>
        /// ����idsȡ���еĲ�������,������ʽ"1,2,3",����"һ��,����,����"
        /// </summary>
        /// <param name="deptIDs"></param>
        /// <returns></returns>
        public string GetDeptNamesByIDs(string deptIDs)
        {
            string strSql = "select Name from " + DBHelperProxy.FormatTable("Dept") + " where id in (" + deptIDs + ")";
            DataTable dt = DBHelperProxy.GetDataTable(strSql);
            StringBuilder names = new StringBuilder();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    names.Append(dt.Rows[i]["Name"].ToString());
                }
                else
                {
                    names.Append("," + dt.Rows[i]["Name"].ToString());
                }
            }
            return names.ToString();

        }

        #endregion

        #region ���ݵ�λidȡ���в���
        /// <summary>
        /// ���ݵ�λidȡ���в���
        /// </summary>
        /// <param name="unitID"></param>
        /// <returns></returns>
        public DataTable GetDeptsByID(int unitID)
        {
            string strSql = "select * from " + DBHelperProxy.FormatTable("Dept") + " where UnitID =" + unitID + " order by Sequence ";
            return DBHelperProxy.GetDataTable(strSql);
        }

        #endregion

        #region ���ݲ���idȡ�����¼�����,�����¼����Ӳ���
        /// <summary>
        /// ���ݲ���idȡ�����¼�����,�����¼����Ӳ���
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public DataTable GetChildDeptsByID(int parentID)
        {
            string strSql = "select * from " + DBHelperProxy.FormatTable("Dept") + " where ParentID =" + parentID + " order by Sequence ";
            return DBHelperProxy.GetDataTable(strSql);
        }

        #endregion

        #region ���ݲ���idȡ�����쵼id
        /// <summary>
        /// ���ݲ���idȡ�����쵼id
        /// </summary>
        /// <param name="deptID"></param>
        /// <returns></returns>
        public void GetLeaderIDByDeptID(int deptID, out int leaderID, out string leaderName)
        {
            leaderID = 0;
            leaderName = "";

            string strSQL = "select LeaderID from " + DBHelperProxy.FormatTable("Dept") + " where ID =" + deptID;
            object leader = DBHelperProxy.ExecuteScalar(strSQL);

            if (leader != null && leader != DBNull.Value)
            {
                leaderID = (int)leader;
                IUser user = new UserBO();
                leaderName = user.GetUserNameByID(leaderID);
            }
        }
        #endregion

        #endregion 
 
    }
}
