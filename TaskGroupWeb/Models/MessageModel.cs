using System;

namespace TaskGroupWeb.Models
{
    public class MessageModel
    {
        public int messageID { get; set; }
        public int taskId { get; set; }
        public int userId { get; set; }
        public string message { get; set; }
        public DateTime dateCreated { get; set; }
    }
}
