using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TodoApi.Models;
using TodoApi.Interface;
using System;

namespace TodoApi.Repository
{
    public class UserRepository : AdoRepository<User>, IUser
    {
        private static SqlConnection _connection;

        public UserRepository(string connectionString)
       : base(connectionString)
        {
        }

        public List<User> GetUsers()
        {
            using (var command = new SqlCommand("select Id id, Name name, DbName dbName, Status status, CreatedOn createdOn from Users"))
            {
                return GetRecords(command);
            }
        }

        public override User PopulateRecord(SqlDataReader reader)
        {
            return new User
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                DbName = reader.GetString(2),
                Status = reader.GetInt32(3),
                CreateOn = reader.GetDateTime(4)
            };
        }
    }
}