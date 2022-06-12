using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
namespace excel_to_database
{
    internal class DatabaseConnection
    {
        private NpgsqlConnection conn;
        private StreamReader reader;
        public DatabaseConnection()
        {
            reader = new StreamReader("ConnectionSettings.ini");
            conn = new NpgsqlConnection(@reader.ReadLine());
            reader.Close();
            conn.Open();
        }
        public void closeConnection()
        {
            conn.Close();
        }
        public NpgsqlConnection GetConnection()
        {
            return conn;
        }
        public bool TestConnection()
        {
            if (conn.State == System.Data.ConnectionState.Open)
            {
                return true;
            }
            else return false;
        }
    }
}
