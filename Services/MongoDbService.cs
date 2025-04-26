using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace UniShare.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(IConfiguration configuration)
        {
            var settings = configuration.GetSection("MongoDbSettings");
            var connectionString = settings["ConnectionString"];
            var databaseName = settings["DatabaseName"];

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}
