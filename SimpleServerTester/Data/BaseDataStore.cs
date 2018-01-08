using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;

namespace SimpleServerTester.Data
{
    public abstract class BaseDataStore
    {
        protected string Database;
        protected string DatabaseUser;
        protected string DatabasePassword;

        protected IEnumerable<T> ExecuteSproc<T>(string sprocName, IList<object> parameters)
        {
            DynamicParameters dbParameters = new DynamicParameters();
            string serializedParameterList = "";

            for (int i = 0; i < parameters.Count; i++)
            {
                string parameterKey = "param" + i;
                dbParameters.Add(parameterKey, parameters[i]);
                serializedParameterList += ("@" + parameterKey + ",");
            }
            serializedParameterList = serializedParameterList.TrimEnd(',');

            using (IDbConnection connection = Connect())
            {
                string sprocCallString = $"CALL {sprocName} ({serializedParameterList})";
                return connection.Query<T>(sprocCallString, dbParameters);
            }
        }

        protected T ExecuteSprocScalar<T>(string sprocName, IList<object> parameters)
        {
            return ExecuteSproc<T>(sprocName, parameters).FirstOrDefault();
            /*DynamicParameters dbParameters = new DynamicParameters();
            string serializedParameterList = "";

            for (int i = 0; i < parameters.Count; i++)
            {
                string parameterKey = "param" + i;
                dbParameters.Add(parameterKey, parameters[i]);
                serializedParameterList += ("@" + parameterKey + ",");
            }
            serializedParameterList = serializedParameterList.TrimEnd(',');

            using (IDbConnection connection = Connect())
            {
                string sprocCallString = $"CALL {sprocName} ({serializedParameterList})";
                return connection.ExecuteScalar<T>(sprocCallString, dbParameters);
            }*/
        }

        private MySqlConnection Connect()
        {
            string connectionString = $"database={Database};UID={DatabaseUser};password={DatabasePassword};";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}
