namespace Acesso
{
    public class DbContext
    {
        public UserAcesso userAcesso { get; set; }

        public DbContext(string strConnection)
        {
            userAcesso = new UserAcesso(strConnection);
        }
    }
}
