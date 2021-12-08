using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TodoApi.Models;
using TodoApi.Interface;
using System;

namespace TodoApi.Repository
{
    public class TeamRepository : AdoRepository<TeamDetails>, ITeam
    {
        public TeamRepository(string connectionString)
       : base(connectionString)
        {
        }

        public List<TeamDetails> GetAllTeams()
        {
            using (var command = new SqlCommand("select * from Users"))
            {
                return GetRecords(command);
            }
        }

        public void AddUser(string dbName, User aUser)
        {
            int status = 0;

            if (aUser.userType == "Paid")
            {
                status = 1;
            }

            var insertCommand = "insert into Users (DbName, Name, PayType, Status, CreatedOn) values ('" + dbName + "', '" + aUser.Name + "', '" + aUser.userType + "', '" + status + "', '" + DateTime.Now + "')";

            using (var command = new SqlCommand(insertCommand))
            {
                ExecuteQuery(command);
            }
        }

        public void ChangePayType(User aUser)
        {
            string updateCommand = "";

            if (aUser.userStatus == false)
            {
                updateCommand = "update Users set PayType = @PayType, Status = @Status where DbName = @DbName";

                using (var command = new SqlCommand(updateCommand))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@PayType", aUser.userType);
                    command.Parameters.AddWithValue("@Status", 0);
                    command.Parameters.AddWithValue("@DbName", aUser.DbName);

                    ExecuteQuery(command);
                }

                if (aUser.userType == "Paid" && (aUser.DbName != "" || aUser.DbName != null))
                {
                    var deleteCommand = "exec ('drop database ' + @databaseName)";
                    using (var command = new SqlCommand(deleteCommand))
                    {
                        command.Parameters.Add("@databaseName", SqlDbType.Text);
                        command.Parameters["@databaseName"].Value = aUser.DbName;

                        ExecuteQuery(command);
                    }
                }
            }
            else
            {
                updateCommand = "update Users set DbName = @DbName, PayType = @PayType, Status = @Status where Id = @Id";

                Guid id = Guid.NewGuid();
                string myString = id.ToString().Replace("-", string.Empty);

                string dbName = "SaasDb" + myString;

                using (var command = new SqlCommand(updateCommand))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Id", aUser.Id);
                    command.Parameters.AddWithValue("@PayType", aUser.userType);
                    command.Parameters.AddWithValue("@Status", 1);
                    command.Parameters.AddWithValue("@DbName", dbName);

                    ExecuteQuery(command);
                }

                CreateDb(dbName, aUser);
            }
        }

        public int CreateDb(string id, User aUser)
        {
            string dbName = "";

            if (id == "")
            {
                dbName = "SaasDb" + id;
            }
            else
            {
                dbName = id;
            }

            int val = 0;
            int valCreate = 0;
            int valInsert = 0;

            var createDatabaseQuery = "exec ('create database ' + @databaseName)";
            using (var command = new SqlCommand(createDatabaseQuery))
            {
                command.Parameters.Add("@databaseName", SqlDbType.Text);
                command.Parameters["@databaseName"].Value = dbName;

                val = ExecuteQuery(command);
            }

            if (val == -1)
            {
                var createCommand = "create table Users (Id int IDENTITY(1,1) PRIMARY KEY, DbName nvarchar(100), Name nvarchar(200) NOT NULL, Status int, UserType nvarchar(10))";

                using (var command = new SqlCommand(createCommand))
                {
                    valCreate = ExecuteToCreateDb(dbName, command);
                }
            }

            if (valCreate == -1)
            {
                var insertCommand = "insert into Users (DbName, Name, Status, UserType) values ('" + dbName + "', '" + aUser.Name + "', 1, '" + aUser.userType + "')";

                using (var command = new SqlCommand(insertCommand))
                {
                    valInsert = ExecuteToCreateDb(dbName, command);
                }
            }

            return 1;
        }

        public override TeamDetails PopulateRecord(SqlDataReader reader)
        {
            return new TeamDetails
            {
                TeamId = reader.GetInt32(0),
                TeamName = reader.GetString(1)
            };
        }
    }
}