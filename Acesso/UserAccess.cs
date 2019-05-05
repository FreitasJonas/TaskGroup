using Objetos;
using System;
using System.Collections.Generic;

namespace Acesso
{
    public class UserAccess<T> : DaoMySql, IDao<T> where T : User
    {
        public UserAccess(string strConnection) : base(strConnection)
        {

        }

        public void Delete(object model)
        {
            throw new NotImplementedException();
        }

        public int Insert(T model)
        {
            try
            {
                OpenDb();

                Cmd.CommandText = "INSERT INTO users (login, pass, name, contact, dt_create) VALUES " +
                                  "(@login, @pass, @name, @contact, @dt_create);";

                Cmd.Parameters.AddWithValue("login", model.login);
                Cmd.Parameters.AddWithValue("pass", model.password);
                Cmd.Parameters.AddWithValue("name", model.name);
                Cmd.Parameters.AddWithValue("contact", model.contact);
                Cmd.Parameters.AddWithValue("dt_create", DateTime.Now);

                if(Cmd.ExecuteNonQuery() > 0)
                {
                    return (int)Cmd.LastInsertedId;
                }
                else
                {
                    throw new Exception("Não foi possível retonar id do usuário");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseDb();
            }
        }

        public T Select(object idUser) 
        {
            try
            {
                OpenDb();

                var _user = Activator.CreateInstance<T>(); // new User();

                Cmd.CommandText = "select * from users where id_user = @id_user";

                Cmd.Parameters.AddWithValue("id_user", idUser);
                
                Reader = Cmd.ExecuteReader();

                if (Reader.Read())
                {
                    _user.userId = Reader.GetInt32("id_user");
                    _user.login = Reader.GetString("login");
                    _user.name = Reader.GetString("name");
                    _user.password = Reader.GetString("pass");
                    _user.contact = Reader.GetString("contact");
                    _user.dateCreated = Reader.GetDateTime("dt_create");
                }

                return _user;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseDb();
            }
        }

        public void Update(T model)
        {
            try
            {
                OpenDb();

                Cmd.CommandText = "update users set name = @name, pass = @pass, contact = @contact where id_user = @id_user";

                Cmd.Parameters.AddWithValue("name", model.name);
                Cmd.Parameters.AddWithValue("pass", model.password);
                Cmd.Parameters.AddWithValue("contact", model.contact);
                Cmd.Parameters.AddWithValue("id_user", model.userId);

                Cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseDb();
            }
        }

        public User ValidateUser(string login, string password, out bool validate)
        {
            try
            {
                OpenDb();

                var _user = new User();

                Cmd.CommandText = "select * from users where login = @login and pass = @pass";

                Cmd.Parameters.AddWithValue("login", login);
                Cmd.Parameters.AddWithValue("pass", password);

                Reader = Cmd.ExecuteReader();

                if (Reader.Read())
                {
                    _user.userId = Reader.GetInt32("id_user");
                    _user.login = Reader.GetString("login");
                    _user.name = Reader.GetString("name");
                    _user.contact = Reader.GetString("contact");
                    _user.dateCreated = Reader.GetDateTime("dt_create");

                    validate = true;
                }
                else {
                    validate = false;
                }

                return _user;
            }
            catch (Exception e)
            {
                validate = false;
                throw e;
            }
            finally
            {
                CloseDb();
            }
        }

        public User FindFromLogin(string login, out bool isValid)
        {
            try
            {
                OpenDb();

                var _user = new User();

                Cmd.CommandText = "select * from users where login = @login";

                Cmd.Parameters.AddWithValue("login", login);

                Reader = Cmd.ExecuteReader();

                if (Reader.Read())
                {
                    _user.userId = Reader.GetInt32("id_user");
                    _user.login = Reader.GetString("login");
                    _user.name = Reader.GetString("name");
                    _user.contact = Reader.GetString("contact");
                    _user.dateCreated = Reader.GetDateTime("dt_create");

                    isValid = false;
                }
                else
                {
                    isValid = true;
                }

                return _user;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseDb();
            }
        }

        public List<T> List()
        {
            try
            {
                OpenDb();

                var list = new List<User>();

                Cmd.CommandText = "select * from users";

                Reader = Cmd.ExecuteReader();

                while (Reader.Read())
                {
                    var _user = new User();

                    _user.userId = Reader.GetInt32("id_user");
                    _user.login = Reader.GetString("login");
                    _user.name = Reader.GetString("name");
                    _user.contact = Reader.GetString("contact");
                    _user.dateCreated = Reader.GetDateTime("dt_create");

                    list.Add(_user);
                }

                return list as List<T>;
            }
            catch(Exception e)
            {
                throw e;
            }
            finally
            {
                CloseDb();
            }
        }
    }
}
