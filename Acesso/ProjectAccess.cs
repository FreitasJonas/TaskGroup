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

                Cmd.CommandText = "INSERT INTO projects (name, nm_desc, framework, dt_create) " +
                    "VALUES (@name, @nm_desc, @framework, @dt_create);";

                Cmd.Parameters.AddWithValue("name", model.name);
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
    }
}
