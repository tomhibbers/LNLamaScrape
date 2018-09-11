using System;
using MongoDB.Driver;

namespace LNLamaScrape.DB
{
    public class DbConfig
    {
        internal string ConnectionString { get; }
        internal MongoClient Client { get; private set; }
        internal IMongoDatabase Database { get; private set; }
        internal string DbName { get; }
        internal string RepositoryName { get; }

        public DbConfig(string connectionString = null, string dbName = null, string repositoryName = null)
        {
            //"mongodb://localhost:27017"
            //LNLamaScrape
            ConnectionString = connectionString;
            DbName = dbName;
            RepositoryName = repositoryName;
        }

        public void Init()
        {
            Client = new MongoClient(ConnectionString);
            Database = Client.GetDatabase(DbName);
        }
    }
}
