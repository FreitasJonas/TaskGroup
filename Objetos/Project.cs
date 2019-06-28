using System;
using System.Collections.Generic;
using static Objetos.DbEnumerators;

namespace Objetos
{
    public class Project
    {
        public int projectId { get; set; }
        public int authorId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string framework { get; set; }
        public DateTime dateCreated { get; set; }
        public string git { get; set; }
        public ProjectStatus status { get; set; }

        public List<Task> tasks { get; set; }

        public Project()
        {
            tasks = new List<Task>();
        }
    }
}
