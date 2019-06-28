using Objetos;
using System;
using System.Collections.Generic;

namespace Acesso
{
    public class ParamAccess<T> : DaoMySql, IDao<T> where T : Param
    {
        public ParamAccess(string strConnection) : base(strConnection)
        {
        }

        public void Delete(object model)
        {
            throw new NotImplementedException();
        }

        public int Insert(T model)
        {
            throw new NotImplementedException();
        }

        public List<T> List(object param)

        {
            try
            {
                var listParam = new List<Param>();

                OpenDb();

                Cmd.CommandText = "select * from param where nm_nome like @nm_nome and nr_status = 0";
                Cmd.Parameters.AddWithValue("nm_nome", "%" + param.ToString() + "%");

                Reader = Cmd.ExecuteReader();

                while (Reader.Read())
                {
                    var _param = new Param();

                    _param.name = Reader.GetString("nm_nome");
                    _param.value = Reader.GetString("nm_valor");
                    _param.status = Reader.GetInt32("nr_status");
                    _param.description = Reader.GetString("nm_desc"); 

                    listParam.Add(_param);
                }

                return listParam as List<T>;
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

        public T Select(object param)
        {
            try
            {
                OpenDb();

                var _param = Activator.CreateInstance<T>();

                Cmd.CommandText = "select * from param where nm_nome = @nm_nome and nr_status = 0";
                Cmd.Parameters.AddWithValue("nm_nome", param.ToString());

                Reader = Cmd.ExecuteReader();

                if (Reader.Read())
                {
                    _param.name = Reader.GetString("nm_nome");
                    _param.value = Reader.GetString("nm_valor");
                    _param.status = Reader.GetInt32("nr_status");
                    _param.description = Reader.GetString("nm_desc");
                }

                return _param;
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
            throw new NotImplementedException();
        }
    }
}
