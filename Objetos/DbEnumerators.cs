namespace Objetos
{
    public class DbEnumerators
    {
        public enum TaskStatus
        {
            Aberto = 0,
            Andamento = 1,
            Finalizado = 2
        }

        public enum ProjectStatus
        {
            Aberto = 0,
            Fechado = 1
        }

        public enum UserStatus
        {
            Ativo = 0,
            Bloqueado = 1
        }

        public enum UserAcesso
        {
            Administrador = 0,
            Usuario = 1
        }
    }
}
