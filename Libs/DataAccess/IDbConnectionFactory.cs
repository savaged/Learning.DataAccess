using System.Data;

namespace DataAccess
{
    public interface IDbConnectionFactory
    {
        T Get<T>() where T : IDbConnection, new();
    }
}