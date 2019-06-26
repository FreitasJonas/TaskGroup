using MySql.Data.MySqlClient;
using System.Data;

namespace Acesso
{
    public abstract class DaoMySql
    {
        protected MySqlCommand Cmd;
        protected MySqlConnection Conn;
        protected MySqlDataReader Reader;
        protected MySqlDataAdapter Adapter;
        protected MySqlTransaction Transaction;

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
                Cmd.Parameters.Clear();
            }
        }
    }
}
