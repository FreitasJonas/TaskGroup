using Objetos;

namespace Acesso
{
    public class DbContext
    {
        public UserAccess<User> DbUser { get; set; }
        public ProjectAccess<Project> DbProject { get; set; }

        public DbContext(string strConnection)
        {
            DbUser = new UserAccess<User>(strConnection);
            DbProject = new ProjectAccess<Project>(strConnection);
        }
    }
}
