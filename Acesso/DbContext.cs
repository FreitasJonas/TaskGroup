using Objetos;

namespace Acesso
{
    public class DbContext
    {
        public UserAccess<User> DbUser { get; set; }
        public ProjectAccess<Project> DbProject { get; set; }
        public ParamAccess<Param> DbParam { get; set; }   
        public TaskAccess<Task> DbTask { get; set; }
        public MessageAccess<Message> DbMessage { get; set; }

        public DbContext(string strConnection)
        {
            DbUser = new UserAccess<User>(strConnection);
            DbProject = new ProjectAccess<Project>(strConnection);
            DbParam = new ParamAccess<Param>(strConnection);
            DbTask = new TaskAccess<Task>(strConnection);
            DbMessage = new MessageAccess<Message>(strConnection);
        }
    }
}
