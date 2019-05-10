using Objetos;
using System;
using System.Collections.Generic;

namespace Acesso
{
    public class MessageAccess<T> : DaoMySql, IDao<T> where T : Message
    {
        public MessageAccess(string strConnection) : base(strConnection)
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

                Cmd.CommandText = "INSERT INTO messages (id_task, id_user, message, dt_create) VALUES (@id_task, @id_user, @message, @dt_create);";

                Cmd.Parameters.AddWithValue("id_task", model.taskId);
                Cmd.Parameters.AddWithValue("id_user", model.userId);
                Cmd.Parameters.AddWithValue("message", model.message);
                Cmd.Parameters.AddWithValue("dt_create", model.dateCreated);

                if (Cmd.ExecuteNonQuery() > 0)
                {
                    return (int)Cmd.LastInsertedId;
                }
                else
                {
                    throw new Exception("Não foi possível retonar id da mensagem");
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

        public List<T> List(object taskId)
        {
            try
            {
                OpenDb();

                var list = new List<Message>();

                Cmd.CommandText = "select * from messages where id_task = @id_task";
                Cmd.Parameters.AddWithValue("id_task", taskId);

                Reader = Cmd.ExecuteReader();

                while (Reader.Read())
                {
                    var message = new Message();

                    message.taskId = Reader.GetInt32("id_task");
                    message.messageID = Reader.GetInt32("id_message");
                    message.userId = Reader.GetInt32("id_user");
                    message.message = Reader.GetString("message");
                    message.dateCreated = Reader.GetDateTime("dt_create");

                    list.Add(message);
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

        public T Select(object model)
        {
            throw new NotImplementedException();
        }

        public void Update(T model)
        {
            throw new NotImplementedException();
        }
    }
}
