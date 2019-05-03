namespace Acesso
{
    public interface IDao
    {
        void OpenDb();
        void CloseDb();

        void Insert(object model);
        object Select(object model);
        void Update(object model);
        void Delete(object model);
    }
}
