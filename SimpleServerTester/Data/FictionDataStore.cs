using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleServerTester.Data.DataAccessObjects;

namespace SimpleServerTester.Data
{
    public class FictionDataStore : BaseDataStore
    {
        public FictionDataStore()
        {
            Database = "Game";
            DatabaseUser = "root";
            DatabasePassword = "";         
        }

        public int? CreateUser(string username, string password)
        {
            return ExecuteSprocScalar<int?>("CreateUser", new List<object> {username, password});
        }

        public UserDao GetUserByUsername(string username)
        {
            return ExecuteSprocScalar<UserDao>("GetUserByUsername", new List<object> {username});
        }
    }
}
