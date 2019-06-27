using Objetos;
using System;
using System.Collections.Generic;
using static Objetos.DbEnumerators;

namespace Acesso
{
    public class TaskAccess<T> : DaoMySql, IDao<T> where T : Task
    {
        public TaskAccess(string strConnection) : base(strConnection)
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

                Cmd.CommandText = "INSERT INTO tasks (id_task, id_project, cod_task, user_own, subject, description, status, dt_create, dt_sla, dt_finaly)" +
                    " VALUES (@id_task, @id_project, @cod_task, @user_own, @subject, @description, @status, @dt_create, @dt_sla, @dt_finaly);";

                Cmd.Parameters.AddWithValue("id_task", model.taskId);
                Cmd.Parameters.AddWithValue("id_project", model.projectId);
                Cmd.Parameters.AddWithValue("cod_task", model.taskCode);
                Cmd.Parameters.AddWithValue("user_own", model.userOwnId);
                Cmd.Parameters.AddWithValue("subject", model.subject);
                Cmd.Parameters.AddWithValue("description", model.description);
                Cmd.Parameters.AddWithValue("status", model.status);
                Cmd.Parameters.AddWithValue("dt_create", model.dateCreated);
                Cmd.Parameters.AddWithValue("dt_sla", model.dateSla);
                Cmd.Parameters.AddWithValue("dt_finaly", model.dateFinaly);

                if (Cmd.ExecuteNonQuery() > 0)
                {
                    return (int)Cmd.LastInsertedId;
                }
                else
                {
                    throw new Exception("Não foi possível retonar id da tarefa");
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

        public List<T> ListFromProject(int projectId)
        {
            try
            {
                OpenDb();

                var list = new List<Task>();

                Cmd.CommandText = @"select * from tasks t inner join users u on t.user_own = u.id_user where id_project = @id_project;";
                Cmd.Parameters.AddWithValue("id_project", projectId);

                Reader = Cmd.ExecuteReader();

                while (Reader.Read())
                {
                    var _task = new Task();

                    _task.taskId = Reader.GetInt32("id_task");
                    _task.projectId = Reader.GetInt32("id_project");
                    _task.taskCode = Reader.GetString("cod_task");
                    _task.userOwnId = Reader.GetInt32("user_own");
                    _task.subject = Reader.GetString("subject");
                    _task.description = Reader.GetString("description");
                    _task.status = (TaskStatus) Reader.GetInt32("status");
                    _task.dateCreated = Reader.GetDateTime("dt_create");
                    _task.dateSla = Reader.GetDateTime("dt_sla");
                    _task.dateFinaly = Reader.GetDateTime("dt_finaly");

                    _task.userOwn.userId = Reader.GetInt32("id_user");
                    _task.userOwn.login = Reader.GetString("login");
                    _task.userOwn.name = Reader.GetString("name");
                    _task.userOwn.contact = Reader.GetString("contact");
                    _task.userOwn.dateCreated = Reader.GetDateTime("dt_create");

                    list.Add(_task);
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

        public List<T> List(object model = null)
        {
            try
            {
                OpenDb();

                var list = new List<Task>();

                Cmd.CommandText = "select * from tasks";

                Reader = Cmd.ExecuteReader();

                while (Reader.Read())
                {
                    var _task = new Task();

                    _task.taskId      = Reader.GetInt32("id_task");
                    _task.projectId   = Reader.GetInt32("id_project");
                    _task.taskCode    = Reader.GetString("cod_task");
                    _task.userOwnId   = Reader.GetInt32("user_own");
                    _task.subject     = Reader.GetString("subject");
                    _task.description = Reader.GetString("description");
                    _task.status      = (TaskStatus) Reader.GetInt32("status");
                    _task.dateCreated = Reader.GetDateTime("dt_create");
                    _task.dateSla     = Reader.GetDateTime("dt_sla");
                    _task.dateFinaly = Reader.GetDateTime("dt_finaly");

                    list.Add(_task);
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

        public T Select(object taskId)
        {
            try
            {
                OpenDb();

                var _task = Activator.CreateInstance<T>(); 

                Cmd.CommandText = "select * from tasks where id_task = @id_task";

                Cmd.Parameters.AddWithValue("id_task", taskId);

                Reader = Cmd.ExecuteReader();

                if (Reader.Read())
                {
                    _task.taskId = Reader.GetInt32("id_task");
                    _task.projectId = Reader.GetInt32("id_project");
                    _task.taskCode = Reader.GetString("cod_task");
                    _task.userOwnId = Reader.GetInt32("user_own");
                    _task.subject = Reader.GetString("subject");
                    _task.description = Reader.GetString("description");
                    _task.status = (TaskStatus) Reader.GetInt32("status");
                    _task.dateCreated = Reader.GetDateTime("dt_create");
                    _task.dateSla = Reader.GetDateTime("dt_sla");
                    _task.dateFinaly = Reader.GetDateTime("dt_finaly");
                }

                return _task;
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

                Cmd.CommandText = "update tasks set " +
                    "id_project = @id_project, " +
                    "user_own = @user_own, " +
                    "cod_task = @cod_task, " +
                    "subject = @subject, " +
                    "description = @description, " +
                    "status = @status, " +
                    "dt_sla = @dt_sla, " +
                    "dt_finaly = @dt_finaly " +
                    "where id_task = @id_task";

                Cmd.Parameters.AddWithValue("id_task", model.taskId);
                Cmd.Parameters.AddWithValue("id_project", model.projectId);
                Cmd.Parameters.AddWithValue("cod_task", model.taskCode);
                Cmd.Parameters.AddWithValue("user_own", model.userOwnId);
                Cmd.Parameters.AddWithValue("subject", model.subject);
                Cmd.Parameters.AddWithValue("description", model.description);
                Cmd.Parameters.AddWithValue("status", model.status);
                Cmd.Parameters.AddWithValue("dt_sla", model.dateSla);
                Cmd.Parameters.AddWithValue("dt_finaly", model.dateFinaly);

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
    }
}
