using System.ComponentModel.DataAnnotations;

namespace TaskGroupWeb.Models
{
    public class LoginModel
    {
        [Display(Name = "E-mail")]
        public string login { get; set; }

        [Display(Name = "Senha")]
        public string password { get; set; }
    }
}
