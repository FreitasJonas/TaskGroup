using System.Collections.Generic;

namespace Acesso
{
    public interface IDao<T>
    {
        void OpenDb();
        void CloseDb();

        int Insert(T model);
        T Select(object model);
        void Update(T model);
        void Delete(object model);
        List<T> List();
    }
}
