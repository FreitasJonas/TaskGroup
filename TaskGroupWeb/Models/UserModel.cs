using System;
using System.ComponentModel.DataAnnotations;

namespace TaskGroupWeb.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "Campo obrigatório!")]
        public int userId { get; set; }

        [Display(Name = "E-mail*")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Valor informado não é um e-mail")]
        public string login { get; set; }

        [Display(Name = "Senha*")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public string password { get; set; }

        [Display(Name = "Confirmar senha*")]
        [Compare("password", ErrorMessage = "Senhas não correspondem!")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public string confirmPassword { get; set; }

        [Display(Name = "Nome*")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public string name { get; set; }

        [Display(Name = "Contato*")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public string contact { get; set; }

        [Display(Name = "Data de cadastro")]
        public DateTime dateCreated { get; set; }
    }
}
