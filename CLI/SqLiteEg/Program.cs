using Autofac;
using DataAccess;
using Model;
using Newtonsoft.Json;
using System;
using System.Data.SQLite;

namespace SqLiteEg
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = GetContainer();
            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<Program>();
                app.Run();
            }
        }

        static void Feedback(string s)
        {
            Console.WriteLine(s);
        }

        static IContainer GetContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<DbConnectionFactory>().As<IDbConnectionFactory>()
                .WithParameter("dbConnectionString", _dbConnectionString);
            builder.RegisterType<DataService<SQLiteConnection>>().As<IDataService>();
            builder.RegisterType<Program>();
            return builder.Build();
        }

        private const string _dbConnectionString = "Data Source=.\\mydb.db;Version=3;";
        private readonly IDataService _dataService;

        public Program(IDataService dataService)
        {
            _dataService = dataService ??
                throw new ArgumentNullException(nameof(dataService));
        }

        private void Run()
        {
            var index = _dataService.Index<ProgrammingLanguage>();
            foreach (var item in index)
            {
                Feedback(JsonConvert.SerializeObject(item));
            }
        }

    }
}
