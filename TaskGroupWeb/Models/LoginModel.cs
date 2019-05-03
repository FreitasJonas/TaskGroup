using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskGroupWeb.Models
{
    public class LoginModel
    {
        public int userId { get; set; }
        public string login { get; set; }
        public string senha { get; set; }
        public DateTime dataDeCriacao { get; set; }
    }
}
