using Dapper.Contrib.Extensions;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataAccess
{
    public class DataService<DbConnectionType> : IDataService
        where DbConnectionType : IDbConnection, new()
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public DataService(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory ??
                throw new ArgumentNullException(nameof(dbConnectionFactory));
        }

        public T Create<T>() where T : BaseModel, new() => new T();

        public T Show<T>(int id) where T : BaseModel
        {
            T value = default;
            using (DbConnectionType cn = _dbConnectionFactory.Get<DbConnectionType>())
            {
                SafelyOpen(cn);
                try
                {
                    value = cn.Get<T>(id);
                }
                finally
                {
                    SafelyClose(cn);
                }
            }
            return value;
        }

        public IList<T> Index<T>() where T : BaseModel
        {
            var data = new List<T>();
            using (DbConnectionType cn = _dbConnectionFactory.Get<DbConnectionType>())
            {
                SafelyOpen(cn);
                try
                {
                    data = cn.GetAll<T>()?.ToList();
                }
                finally
                {
                    SafelyClose(cn);
                }
            }
            return data;
        }

        public void Store<T>(T model) where T : BaseModel
        {
            if (model == null) return;
            if (model.IsNew() != true)
                throw new InvalidOperationException(
                    $"Usage of {nameof(Store)} method for an existing model!");

            using (DbConnectionType cn = _dbConnectionFactory.Get<DbConnectionType>())
            {
                SafelyOpen(cn);
                try
                {
                    cn.Insert(model);
                }
                finally
                {
                    SafelyClose(cn);
                }
            }
        }

        public void Update<T>(T model) where T : BaseModel
        {
            if (model == null) return;
            if (model.IsNew() == true)
                throw new InvalidOperationException(
                    $"Usage of {nameof(Update)} method for a new model!");

            using (DbConnectionType cn = _dbConnectionFactory.Get<DbConnectionType>())
            {
                SafelyOpen(cn);
                try
                {
                    cn.Update<T>(model);
                }
                finally
                {
                    SafelyClose(cn);
                }
            }
        }

        public void Destroy<T>(int id) where T : BaseModel
        {
            var model = Show<T>(id);
            if (model == null) return;

            using (DbConnectionType cn = _dbConnectionFactory.Get<DbConnectionType>())
            {
                SafelyOpen(cn);
                try
                {
                    cn.Delete(model);
                }
                finally
                {
                    SafelyClose(cn);
                }
            }
        }

        private void SafelyOpen(IDbConnection dbConnection)
        {
            if (dbConnection?.State == ConnectionState.Open) return;
            dbConnection?.Open();
        }

        private void SafelyClose(IDbConnection dbConnection)
        {
            if (dbConnection?.State == ConnectionState.Closed) return;
            dbConnection?.Close();
        }

    }
}
