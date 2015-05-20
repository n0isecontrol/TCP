using System;
using System.Collections;
using System.Collections.Generic;

using System.Text;


using System.Data;
using XBase.Database;

namespace XBase.App.Model
{
    public class MUser
    {
        public int Id;
        public string Name;
        public string Password;
        public string IpAddress;        
        public string Mail;
        public string Phone;
        public string Value1;
        public string Value2;
        public string Value3;

        public MUser()
        {
            Id = 0;
            Name = "";
            Password = "";
            IpAddress = "";            
            Mail = "";
            Phone = "";
            Value1= "";
            Value2 = "";
            Value3 = "";
        }

        public void Create(DataEngine vEngine)
        {
            string strSQL = "INSERT INTO MUser ( Name, Password, IpAddress, Mail, Phone, Value1, Value2, Value3 ) VALUES (";
            strSQL += "  '" + Name + "' ";
            strSQL += ", '" + Password + "' ";
            strSQL += ", '" + IpAddress + "' ";
            strSQL += ", '" + Mail + "' ";
            strSQL += ", '" + Phone + "' ";
            strSQL += ", '" + Value1 + "' ";
            strSQL += ", '" + Value2 + "' ";
            strSQL += ", '" + Value3 + "' ";
            strSQL += " ) ";

            vEngine.ExecuteCommand(strSQL);
        }

        public void Update(DataEngine vEngine )
        {
            string strSQL = "UPDATE MUser SET ";
            strSQL += "  Name= '" + Name + "' ";
            strSQL += ", Password = '" + Password + "' ";
            strSQL += ", IpAddress = '" + IpAddress + "' ";
            strSQL += ", Mail = '" + Mail + "' ";
            strSQL += ", Phone = '" + Phone + "' ";
            strSQL += ", Value1 = '" + Value1 + "' ";
            strSQL += ", Value2 = '" + Value2 + "' ";
            strSQL += ", Value3 = '" + Value3 + "' ";
            strSQL += " WHERE ";
            strSQL += " Id = " + Id + " ";

            vEngine.ExecuteCommand(strSQL);
        }

        public static ArrayList Select(DataEngine vEngine, string vWhere)
        {
            string sql = "SELECT * FROM MUser ";

            if( String.IsNullOrEmpty(vWhere) == false )
            {
                sql += " WHERE " + vWhere;
            }

            DataSet ds = vEngine.SelectCommand(sql);

            ArrayList tResult = new ArrayList();

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    MUser tUser = new MUser();

                    tUser.Id        = (int)row["Id"];
                    tUser.Name      = row["Name"] as string;
                    tUser.Password  = row["Password"] as string;
                    tUser.IpAddress = row["IpAddress"] as string;
                    tUser.Mail      = row["Mail"] as string;
                    tUser.Phone     = row["Phone"] as string;
                    tUser.Value1    = row["Value1"] as string;
                    tUser.Value2    = row["Value2"] as string;
                    tUser.Value3    = row["Value3"] as string;

                    tResult.Add(tUser);
                }
            }

            return tResult;
            
        }

        public void Delete(DataEngine vEngine)
        {
            string strSQL = "DELETE FROM MUser ";
            strSQL += " WHERE ";
            strSQL += " Id = " + Id + " ";

            vEngine.ExecuteCommand(strSQL);
        }



        public static ArrayList GetUser(DataEngine vEngine, string vUserName )
        {
            string strWhere = " Name = '" + vUserName + "' ";

            return Select(vEngine, strWhere);
        }
    }
}
