using System;
using System.ComponentModel.DataAnnotations;

namespace TaskGroupWeb.Models
{
    public class TaskModel
    {
        public int taskId { get; set; }

        [Display(Name = ("Projeto*"))]
        [Required(ErrorMessage = "Campo obrigatório")]
        public int projectId { get; set; }

        [Display(Name = ("Código"))]
        public string taskCode { get; set; }

        [Display(Name = ("Dono*"))]
        [Required(ErrorMessage = "Campo obrigatório")]
        public int userOwnId { get; set; }

        [Display(Name = ("Assunto*"))]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string subject { get; set; }

        [Display(Name = ("Descrição*"))]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string description { get; set; }

        [Display(Name = ("Status*"))]
        [Required(ErrorMessage = "Campo obrigatório")]
        public int status { get; set; }

        [Display(Name = ("Date de cadastro"))]
        public DateTime dateCreated { get; set; }

        [Display(Name = ("Data de vencimento*"))]
        [Required(ErrorMessage = "Campo obrigatório")]
        public DateTime dateSla { get; set; }

        [Display(Name = ("Data finalizado"))]
        public DateTime dateFinaly { get; set; }
    }
}
