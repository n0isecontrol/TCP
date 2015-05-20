using System;
using System.Collections.Generic;

using System.Text;


using System.Data;
using MySql.Data.MySqlClient;


namespace XBase.Database
{
    public class DataEngine
    {
        static String mConnectionString = "";

        public DataEngine()
        {

        }

        public static void SetConnectionString(string vHost, string vDatabse, string vUser, string vPassword)
        {
            mConnectionString = "server=" + vHost + ";" ;
            mConnectionString += "uid=" + vUser + ";";
            mConnectionString += "pwd=" + vPassword + ";" ;
            mConnectionString += "database=" + vDatabse + ";";
        }

        public void ExecuteCommand(String vSQL)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(mConnectionString) )
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(vSQL, conn);
                    cmd.ExecuteNonQuery();
                }
            }
            catch(System.Exception ex )
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
        }

        public DataSet SelectCommand(String vSQL)
        {
            DataSet ds = new DataSet();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(mConnectionString))
                {   
                    conn.Open();                
                    MySqlDataAdapter adapter = new MySqlDataAdapter(vSQL, conn);
                    adapter.Fill(ds, "Tab1");
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }

            return ds;
        }
        

    }
}
