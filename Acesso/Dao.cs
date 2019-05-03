using MySql.Data.MySqlClient;
using System.Data;

namespace Acesso
{
    public abstract class DaoMySql : IDao
    {
        public MySqlCommand Cmd;
        public MySqlConnection Conn;
        public MySqlDataReader Reader;
        public MySqlDataAdapter Adapter;

        public DaoMySql(string strConnection)
        {
            Conn = new MySqlConnection(strConnection);
            Cmd = new MySqlCommand();
            Cmd.Connection = Conn;
        }

        public void OpenDb()
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();                
            }
        }

        public void CloseDb()
        {
            if(Conn.State == ConnectionState.Open)
            {
                Conn.Close();
            }
        }

        public abstract void Delete(object model);

        public abstract void Insert(object model);

        public abstract object Select(object model);

        public abstract void Update(object model);
    }
}
