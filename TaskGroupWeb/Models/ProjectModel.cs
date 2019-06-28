using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Objetos.DbEnumerators;

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

        [Display(Name = "Git")]
        public string _git { get; set; }

        public string git {
            get
            {
                if (string.IsNullOrEmpty(_git))
                {
                    return "";
                }
                else
                {
                    return _git;
                }
            }
            set
            {
                _git = value;
            }
        }

        [Display(Name = "Status*")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public ProjectStatus status { get; set; }

        [Display(Name = "Data de cadastro")]
        public DateTime dateCreated { get; set; }

        public List<TaskModel> tasks { get; set; }

        public List<UserModel> users { get; set; }

        public string usersSubscribeId { get; set; }
    }
}
