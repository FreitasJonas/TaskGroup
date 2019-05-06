using System;
using System.Collections.Generic;

namespace Objetos
{
    public class Project
    {
        public int projectId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string framework { get; set; }
        public DateTime dateCreated { get; set; }

        public List<Task> tasks { get; set; }

        public Project()
        {
            tasks = new List<Task>();
        }
    }
}
