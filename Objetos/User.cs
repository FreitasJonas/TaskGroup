using System;
using static Objetos.DbEnumerators;

namespace Objetos
{
    public class User
    {
        public int userId { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string contact { get; set; }
        public UserStatus status { get; set; }
        public UserAcesso acesso { get; set; }
        public DateTime dateCreated { get; set; }
    }
}
