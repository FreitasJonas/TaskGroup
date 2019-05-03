using System;

namespace TaskGroupWeb.Models
{
    public class UserModel
    {
        public int userId { get; set; }
        public string login { get; set; }
        public string senha { get; set; }
        public string contact { get; set; }
        public DateTime dataDeCriacao { get; set; }
    }
}
