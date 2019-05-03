using Objetos;
using System;

namespace Acesso
{
    public class UserAcesso : DaoMySql
    {
        public UserAcesso(string strConnection) : base(strConnection)
        {

        }

        public override void Delete(object model)
        {
            throw new NotImplementedException();
        }

        public override void Insert(object model)
        {
            throw new NotImplementedException();
        }

        public override object Select(object model)
        {
            throw new NotImplementedException();
        }

        public override void Update(object model)
        {
            throw new NotImplementedException();
        }

        public User ValidateUser(User user, out bool validate)
        {
            try
            {
                OpenDb();

                var _user = new User();

                Cmd.CommandText = "select * from users where login = @login and password = @password";

                Cmd.Parameters.AddWithValue("login", user.login);
                Cmd.Parameters.AddWithValue("password", user.password);

                Reader = Cmd.ExecuteReader();

                while (Reader.Read())
                {
                    _user.userId = Reader.GetInt32("id_user");
                    _user.login = Reader.GetString("login");
                    _user.name = Reader.GetString("name");
                    _user.contact = Reader.GetString("contact");
                    _user.dateCreated = Reader.GetDateTime("dt_create");
                }

                validate = true;
                return _user;
            }
            catch (Exception e)
            {
                validate = false;
                return null;
            }
            finally
            {
                CloseDb();
            }
        }
    }
}
