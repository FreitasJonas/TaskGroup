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

        public bool ValidateUser(User model)
        {
            return true;
        }
    }
}
