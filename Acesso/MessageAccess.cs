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
            throw new NotImplementedException();
        }

        public List<T> List(object model = null)
        {
            throw new NotImplementedException();
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
