// Robert Mccormick
// Frameworks
// Term 3
// RobertMcCormick_CE10

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CE10
{
    public class DBConnection
    {
        

        //item select
        public string Password { get; set; }

        // null mysqlconnection for get
        private MySqlConnection connection = null;
        public MySqlConnection Connection
        {
            get { return connection; }
        }

        private static DBConnection _instance = null;

        // null DBconnection for get
        public static DBConnection Instance()
        {
            if (_instance == null)
                _instance = new DBConnection();
            return _instance;
        }


        // same process as CE09
        public bool IsConnect()
        {
            bool result = true;
            if (Connection == null)
            {
                //connection string for calling data to run program
                try
                {
                    connection = new MySqlConnection(@"server= " + GetConnectionString() + " ; userid=dbsAdmin; password=password; database= example_dbs1; port=8889");
                    connection.Open();
                    result = true;
                }catch(Exception ex)
                {
                    result = false;
                }
            }

            return result;
        }

        public void Close()
        {
            connection.Close();
        }

        public string GetConnectionString()
        {
            return File.ReadAllText(@"C:\\VFW\\connect.txt");
        }
    }
}
