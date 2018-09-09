
namespace BIStudio.Framework.Security.Organization
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;
    using System.Xml;
    using System.Data.Common;
    using BIStudio.Framework.Data.Adapter;

    public class DictBO : DBGeneralProxy, IDict
    {
        private int unitID;

        public DictBO()
        {
 
        }

        public DictBO(int unitID)
        {
            this.unitID = unitID;
 
        }
 
        #region 获取字典信息
        public DataTable GetAllDict()
        {
            string strSQL = "SELECT d.*, t.Name AS TypeName FROM DictDir d LEFT JOIN DictType t ON d.TypeID = t.ID where d.UnitID=0 OR d.UnitID=" + unitID.ToString();
            return DBHelperProxy.GetDataTable(strSQL);
        }

        public DataTable GetDict(int type)
        {
            string strSQL = "SELECT d.*, t.Name AS TypeName FROM DictDir d LEFT JOIN DictType t ON d.TypeID = t.ID where d.TypeID=" + type + " and ( d.UnitID=0 OR d.UnitID=" + unitID.ToString() + ")";
            return DBHelperProxy.GetDataTable(strSQL);
        }

        /// <summary>
        /// 获得类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetDictType()
        {
            string strSQL = "select * from DictType";
            DataTable dt = DBHelperProxy.GetDataTable(strSQL);
            return dt;
        }
        #endregion
 
        #region 根据条件返回DictItem表中的一批数据
      
        public DataSet GetDictItems(int dirID)
        {
            string strSQL = "select * from DictItem where DirID =" + dirID;
            return DBHelperProxy.GetDataSet(strSQL);
        }
        #endregion
 
        #region 获取字典信息
        public DataTable QueryDict(string str,long typeID)
        {
            string strSQL = " select id from DictDir where name='" + str + "' and TypeID =" + typeID;
            return DBHelperProxy.GetDataTable(strSQL);
        }
        #endregion
 
        #region 根据字典名获得所有项---生成下拉列表用
        /// <summary>
        /// 根据字典名获得所有项---生成下拉列表用
        /// </summary>
        /// <param name="dictName"></param>
        /// <returns></returns>
        public DataTable GetDictItemByName(string dictName)
        {
            DataTable dt = this.getDictItemByName(dictName);
            DataTable newDt = new DataTable();
            newDt.Columns.Add("Text");
            newDt.Columns.Add("Value");
            newDt.Columns.Add("ID");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = newDt.NewRow();
                row["Text"] = dt.Rows[i]["Name"];
                row["Value"] = dt.Rows[i]["Code"];
                row["ID"] = dt.Rows[i]["ID"];
                newDt.Rows.Add(row);
            }
            return newDt;
        } 
        #endregion

        public DataTable GetDictList(string dictName)
        {
            DataTable dt = this.getDictItemByName(dictName);
            return dt;
        }

        #region 通过类型名称和代码获取名称
        /// <summary>
        /// 通过类型名称获得字典
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public string GetItemName(string typeName, string code)
        {
            string strSQL = "Select i.Name From DictDir d,DictItem i Where i.DirID =d.ID and i.code ='" + code.ToString() + "' And d.Name='" + typeName + "'";
            DataTable dt = DBHelperProxy.GetDataTable(strSQL);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["name"].ToString();
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region 获取特定字典的绑定字典
        /// <summary>
        /// 获取特定字典的绑定字典
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IDictionary<int, string> GetItemDic(string dictname)
        {
            string strSQL = "select ID,Name from DictItem where DirID=(select top 1 ID from DictDir where Name='" + dictname + "')";

            DataTable dt = DBHelperProxy.GetDataTable(strSQL);
            Dictionary<int, string> aDic = new Dictionary<int, string>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = (DataRow)dt.Rows[i];
                    int id = Convert.ToInt32(row["ID"]);
                    string name = row["Name"].ToString();

                    if (aDic.ContainsKey(id) == false)
                    {
                        aDic.Add(id, name);
                    }
                }
            }
            return aDic;
        }
        #endregion
 
        public string GetName(int id)
        {
            string strSql = "select name from DictItem where id=" + id;
            return (string)DBHelperProxy.ExecuteScalar(strSql);
        }

        #region 字典导出导入

        public string ExportDictXml(string ids)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select DictType.Name, dictDir.Name,UnitID,Remark,DictItem.Name,DictItem.Code,Sequence from DictType left join DictDir on DictType.ID=DictDir.TypeID left join DictItem  on dictDir.ID=DictItem.DirID {0} order by TypeID,DirID,[Sequence] for xml auto;", string.IsNullOrEmpty(ids) ? "" : "where dictDir.ID in (" + ids + ")");

            DataTable dt = DBHelperProxy.GetDataTable(strSql.ToString());
            strSql = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                strSql.Append(dr[0].ToString());
            }
            return strSql.ToString();
        }

        public bool ImportDictXml(XmlDocument doc)
        {
            //因为专用数据字典表的，不想再建数据字典的数据实体了，所以用sql语句写死了
            XmlElement root = doc.DocumentElement;
            foreach (XmlNode nodeDictType in root.ChildNodes)
            {
                if (nodeDictType.Attributes.Count == 0) continue;
                string attDictTypeNameValue = nodeDictType.Attributes["Name"].Value;
                string strSql = "select ID from DictType where name=" + DBHelperProxy.FormatParameter("DictTypeName");
                var parDictTypeName = DBHelperProxy.CreateParameter("DictTypeName", attDictTypeNameValue);
                long dictTypeID = Convert.ToInt32(DBHelperProxy.ExecuteScalar(strSql, parDictTypeName));
                var parDictTypeID = DBHelperProxy.CreateParameter("DictTypeID", dictTypeID);
                if (dictTypeID <= 0)//if no exists then add 'DictType '
                {
                    dictTypeID = DBHelperProxy.GetMaxID("DictType");
                    parDictTypeID.Value = dictTypeID;
                    strSql = "insert into DictType(ID,Name) values (" + DBHelperProxy.FormatParameter("DictTypeID") + "," + DBHelperProxy.FormatParameter("DictTypeName") + ");";
                    if (DBHelperProxy.ExecuteNonQuery(strSql, parDictTypeID, parDictTypeName) == 0)
                        return false;
                }

                foreach (XmlNode nodeDictDir in nodeDictType.ChildNodes)
                {
                    if (nodeDictDir.Attributes.Count == 0) continue;
                    strSql = "if exists(select ID from DictDir where name=" + DBHelperProxy.FormatParameter("DictDirName") + " and TypeID=" + DBHelperProxy.FormatParameter("DictTypeID") + ")begin delete DictItem where DirID in (select ID from DictDir where name=" + DBHelperProxy.FormatParameter("DictDirName") + " and TypeID=" + DBHelperProxy.FormatParameter("DictTypeID") + ");delete DictDir where name=" + DBHelperProxy.FormatParameter("DictDirName") + " and TypeID=" + DBHelperProxy.FormatParameter("DictTypeID") + ";end;";//if exists then delete 'DictDir' and 'DictItem'
                    strSql += "insert into DictDir(ID,UnitID,Name,TypeID,Remark) values (" + DBHelperProxy.FormatParameter("DictDirID") + "," + DBHelperProxy.FormatParameter("UnitID") + "," + DBHelperProxy.FormatParameter("DictDirName") + "," + DBHelperProxy.FormatParameter("DictTypeID") + "," + DBHelperProxy.FormatParameter("Remark") + ");";
                    var parDictDirID = DBHelperProxy.CreateParameter("DictDirID", DBHelperProxy.GetMaxID("DictDir"));
                    var parDictDirName = getSqlParameter("DictDirName", nodeDictDir.Attributes["Name"]);
                    var parUnitID = getSqlParameter("UnitID", nodeDictDir.Attributes["UnitID"]);
                    var parDictDirRemark = getSqlParameter("Remark", nodeDictDir.Attributes["Remark"]);
                    if (DBHelperProxy.ExecuteNonQuery(strSql, parDictDirName, parDictDirID, parUnitID, parDictTypeID, parDictDirRemark) == 0)
                        return false;
                    foreach (XmlNode nodeDictItem in nodeDictDir.ChildNodes)
                    {
                        if (nodeDictItem.Attributes.Count == 0) continue;
                        strSql = "insert into DictItem (ID,DirID,Name,Code,Sequence) values (" + DBHelperProxy.FormatParameter("DictItemID") + "," + DBHelperProxy.FormatParameter("DictDirID") + "," + DBHelperProxy.FormatParameter("DictItemName") + "," + DBHelperProxy.FormatParameter("DictItemCode") + "," + DBHelperProxy.FormatParameter("DictItemSequence") + ");";
                        var parDictItemID = DBHelperProxy.CreateParameter("DictItemID", DBHelperProxy.GetMaxID("DictItem"));
                        var parDictItemName = getSqlParameter("DictItemName", nodeDictItem.Attributes["Name"]);
                        var parDictItemCode = getSqlParameter("DictItemCode", nodeDictItem.Attributes["Code"]);
                        var parDictItemSequence = getSqlParameter("DictItemSequence", nodeDictItem.Attributes["Sequence"]);
                        if (DBHelperProxy.ExecuteNonQuery(strSql, parDictItemID, parDictItemName, parDictDirID, parDictItemCode, parDictItemSequence) == 0)
                            return false;
                    }
                }
            }
            return true;
        }

        private DbParameter getSqlParameter(string name, XmlAttribute value)
        {
            DbParameter par;
            if (value == null)
                par = DBHelperProxy.CreateParameter(name, DBNull.Value);
            else
                par = DBHelperProxy.CreateParameter(name, value.Value);
            return par;
        }

        #endregion

        #region 实现接口

        #region 根据字典名称取所有item
        /// <summary>
        /// 根据字典名称取所有item
        /// </summary>
        /// <param name="dictName"></param>
        /// <returns></returns>
        public DataTable GetDictItemsByDict(string dictName)
        {
            DataTable dt = this.getDictItemByName(dictName);
            DataTable newDt = new DataTable();
            newDt.Columns.Add("Text");
            newDt.Columns.Add("Value");
            newDt.Columns.Add("ID");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = newDt.NewRow();
                row["Text"] = dt.Rows[i]["Name"];
                row["Value"] = dt.Rows[i]["Code"];
                row["ID"] = dt.Rows[i]["ID"];
                newDt.Rows.Add(row);
            }
            return newDt;
        }

        #endregion

        #region 根据字典名称和字典项的代码取字典项的名称
        /// <summary>
        /// 根据字典名称和字典项的代码取字典项的名称
        /// </summary>
        /// <param name="dictName"></param>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public string GetDictItemName(string dictName, string itemCode)
        {
            string strSQL = "Select i.Name From DictDir d,DictItem i Where i.DirID =d.ID and i.code ='" + itemCode + "' And d.Name='" + dictName + "'";
            DataTable dt = DBHelperProxy.GetDataTable(strSQL);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["name"].ToString();
            }
            else
            {
                return "";
            }
        }

        #endregion

        #endregion
 
        #region 通过类型名称获得字典
        /// <summary>
        /// 通过类型名称获得字典
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public DataTable GetDictByTypeName(string typeName)
        {
            string strSQL = "Select i.Name,i.Code From DictDir d,DictItem i Where i.DirID =d.ID And d.Name='" + typeName + "' Order By Sequence";
            DataTable dt = DBHelperProxy.GetDataTable(strSQL);
            return dt;
        }
        #endregion

        #region 根据字典名获得项
        /// <summary>
        /// 根据字典名获得项
        /// </summary>
        /// <param name="dictName"></param>
        /// <returns></returns>
        private DataTable getDictItemByName(string dictName)
        {
            string strSQL = "Select i.* From DictDir d, DictItem i Where d.id =i.DirID And d.Name='" + dictName + "' Order By Sequence";
            return DBHelperProxy.GetDataTable(strSQL);
        }
        #endregion
    }
}
