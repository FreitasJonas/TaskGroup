using System;
using System.Collections.Generic;
using System.Text;

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

        public bool ValidateUser(string login, string senha)
        {
            return true;
        }
    }
}
