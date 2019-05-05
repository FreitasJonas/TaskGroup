using System;

namespace Objetos
{
    public class Project
    {
        public int projectId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string framework { get; set; }
        public DateTime dateCreated { get; set; }
    }
}
