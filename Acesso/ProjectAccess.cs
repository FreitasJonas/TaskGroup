using Objetos;
using System;
using System.Collections.Generic;

namespace Acesso
{
    public class ProjectAccess<T> : DaoMySql, IDao<T> where T : Project
    {
        public ProjectAccess(string strConnection) : base(strConnection)
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

                Cmd.CommandText = "INSERT INTO projects (id_author, name, nm_desc, framework, dt_create) " +
                    "VALUES (@id_author, @name, @nm_desc, @framework, @dt_create);";

                Cmd.Parameters.AddWithValue("name", model.name);
                Cmd.Parameters.AddWithValue("id_author", model.authorId);
                Cmd.Parameters.AddWithValue("nm_desc", model.description);
                Cmd.Parameters.AddWithValue("framework", model.framework);
                Cmd.Parameters.AddWithValue("dt_create", DateTime.Now);

                if (Cmd.ExecuteNonQuery() > 0)
                {
                    return (int)Cmd.LastInsertedId;
                }
                else
                {
                    throw new Exception("Não foi possível retonar id do projeto");
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

        public List<T> List(object model = null)
        {
            try
            {
                OpenDb();

                var list = new List<Project>();

                Cmd.CommandText = "select * from projects";

                Reader = Cmd.ExecuteReader();

                while (Reader.Read())
                {
                    var project = new Project();

                    project.projectId = Reader.GetInt32("id_project");
                    project.name = Reader.GetString("name");
                    project.description = Reader.GetString("nm_desc");
                    project.framework = Reader.GetString("framework");
                    project.dateCreated = Reader.GetDateTime("dt_create");

                    list.Add(project);
                }

                return list as List<T>;
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

        public T Select(object projectId)
        {
            try
            {
                OpenDb();

                var project = Activator.CreateInstance<T>(); // new User();

                Cmd.CommandText = "select * from projects where id_project = @id_project";

                Cmd.Parameters.AddWithValue("id_project", projectId);

                Reader = Cmd.ExecuteReader();

                if (Reader.Read())
                {
                    project.projectId = Reader.GetInt32("id_project");
                    project.name = Reader.GetString("name");
                    project.description = Reader.GetString("nm_desc");
                    project.framework = Reader.GetString("framework");
                    project.dateCreated = Reader.GetDateTime("dt_create");
                }

                return project;
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

        public void InsertUserSubscribe(Project project, int userId)
        {
            try
            {
                OpenDb();

                Cmd.CommandText = "INSERT INTO project_users (id_project, id_user) VALUES (@id_project, @id_user);";

                Cmd.Parameters.AddWithValue("id_project", project.projectId);
                Cmd.Parameters.AddWithValue("id_user", userId);

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

        public void DeleteUserSubscribe(Project project, int userId)
        {
            try
            {
                OpenDb();

                Cmd.CommandText = "DELETE * FROM project_users where id_project = @id_project and id_user = @id_user;";

                Cmd.Parameters.AddWithValue("id_project", project.projectId);
                Cmd.Parameters.AddWithValue("id_user", userId);

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

        public void Update(T model)
        {
            try
            {
                OpenDb();

                Cmd.CommandText = "update projects set name = @name, nm_desc = @nm_desc, framework = @framework where id_project = @id_project";

                Cmd.Parameters.AddWithValue("name", model.name);
                Cmd.Parameters.AddWithValue("nm_desc", model.description);
                Cmd.Parameters.AddWithValue("framework", model.framework);
                Cmd.Parameters.AddWithValue("id_project", model.projectId);

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

        public void UpdateUsers(int projectId, params object[] users)
        {
            try
            {
                OpenDb();
                Transaction = Conn.BeginTransaction();
                Cmd.Parameters.AddWithValue("id_project", projectId);

                #region - Delete -

                Cmd.CommandText = "delete from project_users where id_project = @id_project";                

                Cmd.ExecuteNonQuery();

                #endregion
                
                #region - Insert -

                var query = "";

                for (int i = 0; i < users.Length; i++)
                {
                    query += string.Format("INSERT INTO project_users (id_project, id_user) VALUES (@id_project, @id_user_{0});", i);
                    Cmd.Parameters.AddWithValue("id_user_" + i, users[i].ToString());
                }

                Cmd.CommandText = query;
                Cmd.ExecuteNonQuery();

                #endregion

                Transaction.Commit();
            }
            catch (Exception e)
            {
                Transaction.Rollback();
                throw e;
            }
            finally
            {
                CloseDb();
            }
        }

        public List<User> ListUsers(int projectId)
        {
            try
            {
                OpenDb();

                var users = new List<User>();

                Cmd.CommandText = @"select * from users u inner join project_users p on p.id_user = u.id_user where p.id_project = @id_project;";
                Cmd.Parameters.AddWithValue("id_project", projectId);

                Reader = Cmd.ExecuteReader();

                while (Reader.Read())
                {
                    var _user = new User();

                    _user.userId = Reader.GetInt32("id_user");
                    _user.login = Reader.GetString("login");
                    _user.name = Reader.GetString("name");
                    _user.contact = Reader.GetString("contact");
                    _user.dateCreated = Reader.GetDateTime("dt_create");

                    users.Add(_user);
                }

                return users;
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
    }
}
