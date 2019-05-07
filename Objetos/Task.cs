using System;
using static Objetos.DbEnumerators;

namespace Objetos
{
    public class Task
    {
        public int      taskId      { get; set; }
        public int      projectId   { get; set; }
        public string   taskCode    { get; set; }
        public int      userOwnId   { get; set; }
        public string   subject     { get; set; }
        public string   description { get; set; }
        public TaskStatus      status      { get; set; }
        public DateTime dateCreated { get; set; }
        public DateTime dateSla     { get; set; }
        public DateTime dateFinaly  { get; set; }

        public User userOwn { get; set; }

        public Task()
        {
            userOwn = new User();
        }
    }
}
