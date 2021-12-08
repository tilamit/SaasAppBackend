using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TodoApi.Models;

namespace TodoApi.Repository
{
    public abstract class AdoRepository<T> where T : class
    {
        string connectionString = "Data Source=.;Initial Catalog=DemoApp;Trusted_Connection=True";

        private static SqlConnection _connection;
        public AdoRepository(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public virtual T PopulateRecord(SqlDataReader reader)
        {
            return null;
        }
        protected List<T> GetRecords(SqlCommand command)
        {
            var list = new List<T>();
            command.Connection = _connection;
            _connection.Open();

            try
            {
                var reader = command.ExecuteReader();

                try
                {
                    while (reader.Read())
                        list.Add(PopulateRecord(reader));
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }
            }
            finally
            {
                _connection.Close();
            }
            return list;
        }

        protected int ExecuteQuery(SqlCommand command)
        {
            int val = 0;

            command.Connection = _connection;
            _connection.Open();

            try
            {
                command.CommandTimeout = 0;
                val = command.ExecuteNonQuery();
            }
            finally
            {
                _connection.Close();
            }

            return val;
        }

        protected int ExecuteToCreateDb(string dbName, SqlCommand command)
        {
            string constr = "Server=tcp:saas-sql-server.database.windows.net,1433;Initial Catalog=" + dbName + ";Persist Security Info=False;User ID=SaasDb;Password=ATat0128;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=300";
            SqlConnection _connectionDb = new SqlConnection(constr); 
            
            int val = 0;

            command.Connection = _connectionDb;
            _connectionDb.Open();

            try
            {
                command.CommandTimeout = 0;
                val = command.ExecuteNonQuery();
            }
            finally
            {
                _connectionDb.Close();
            }

            return val;
        }
    }
}