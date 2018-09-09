
namespace BIStudio.Framework.Security.Organization
{
    using System;
    using System.Runtime.InteropServices;
    using System.Management;
    using BIStudio;
    using BIStudio.Framework.Data.Adapter;
    using BIStudio.Framework.Utils; 

    public static class Register
    { 
        public static bool  IsRegiste()
        {
            bool val = false;

            string serial = Convert.ToString(DBHelperProxy.ExecuteScalar("Select Code From Dept Where ID=-1"));
            if (string.IsNullOrEmpty(serial))
            {
                val =  false;
            }
            else
            {
                string unitname = Convert.ToString(DBHelperProxy.ExecuteScalar("Select Name From Dept Where ID=1"));
                if (ALEncrypt.Md5hash(unitname + "sudenggang") == serial)
                {
                    val = true;
                }
                else
                {
                    val = false;
                }

            }

            return val;
        }

        public static bool Registe(string serial, string unitName)
        {
            if (ALEncrypt.Md5hash(unitName + "sudenggang") != serial)
            {
                return false;
            }
            else
            {
                DBHelperProxy.ExecuteNonQuery(string.Format("Update Dept set Code='{0}',Name='{1}'  Where ID=-1;Update Dept set Name='{1}'  Where ID=1;", serial, unitName));
                //  DBHelperProxy.ExecuteNonQuery(string.Format("Update ContactDir set  Name='{0}'  Where DeptID=1;",unitName));
                return true;
            }
        }
        
    }
}
 
