using Model;
using System.Collections.Generic;

namespace DataAccess
{
    public interface IDataService
    {
        T Create<T>() where T : BaseModel, new();
        void Destroy<T>(int id) where T : BaseModel;
        IList<T> Index<T>() where T : BaseModel;
        T Show<T>(int id) where T : BaseModel;
        void Store<T>(T model) where T : BaseModel;
        void Update<T>(T model) where T : BaseModel;
    }
}