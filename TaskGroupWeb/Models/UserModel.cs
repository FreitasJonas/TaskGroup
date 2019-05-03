using System;

namespace TaskGroupWeb.Models
{
    public class UserModel
    {
        public int userId { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string contact { get; set; }
        public DateTime dateCreated { get; set; }
    }
}
