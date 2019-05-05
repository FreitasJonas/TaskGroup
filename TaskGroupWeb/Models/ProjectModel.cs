using System;
using System.ComponentModel.DataAnnotations;

namespace TaskGroupWeb.Models
{
    public class ProjectModel
    {
        [Display(Name = "ProjectID")]
        public int projectId { get; set; }

        [Display (Name = "Nome*")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string name { get; set; }

        [Display(Name = "Descrição*")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string description { get; set; }

        [Display(Name = "Framework*")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string framework { get; set; }

        [Display(Name = "Data de cadastro")]
        public DateTime dateCreated { get; set; }
    }
}
