using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Interface
{
    public interface ITeam
    {
        List<TeamDetails> GetAllTeams();
        int CreateDb(string id, User aUser);
        void AddUser(string dbName, User aUser);
        void ChangePayType(User aUser);
    }
}
