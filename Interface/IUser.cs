using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Interface
{
    public interface IUser
    {
        List<User> GetUsers();
    }
}
