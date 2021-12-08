using System;
using System.Collections.Generic;

namespace TodoApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string DbName { get; set; }
        public string userType { get; set; }
        public bool userStatus { get; set; }
        public DateTime CreateOn { get; set; }
    }
}