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

        public List<T> List()
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
