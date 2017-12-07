using System;
using MySql.Data.MySqlClient;

namespace SimpleServerTester
{
    public class BaseDataStore
    {
        public BaseDataStore()
        {
            
        }

        public MySqlConnection Connect(string database, string password, Action action)
        {
            string connectionString = $"database={database};UID=root;password={password};";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}
