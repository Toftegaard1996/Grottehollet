using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Grottehollet.Class
{
    public class DBConnection
    {
        public string ConnectionString = "Data Source=DESKTOP-GE1IOSV;Initial Catalog=GrotteholletDB;Integrated Security=True";
        public string Name;
        public SqlConnection cnn;
        public SqlDataReader reader;
        public SqlCommand cmd;

        public DBConnection()
        {
            cnn = new SqlConnection(ConnectionString);
        }


    }
}
