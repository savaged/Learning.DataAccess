using System;
using System.Data;

namespace DataAccess
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _dbConnectionString;

        public DbConnectionFactory(string dbConnectionString)
        {
            if (string.IsNullOrEmpty(dbConnectionString))
                throw new ArgumentNullException(nameof(dbConnectionString));
            _dbConnectionString = dbConnectionString;
        }

        public T Get<T>() where T : IDbConnection, new()
            => new T { ConnectionString = _dbConnectionString };

    }
}
