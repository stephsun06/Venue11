using MongoDB.Driver;

namespace Venue11.Domain.Mongo
{
    public class MongoUnitOfWork : IMongoUnitOfWork
    {
        private readonly IMongoDatabase _database;

        public MongoUnitOfWork(IMongoDbConnection connection)
        {
            MongoUrl url = new MongoUrl(connection.GetConnectionString());

            MongoClient client = new MongoClient(url);

            _database = client.GetDatabase(url.DatabaseName);
        }

        public IMongoDatabase Context
        {
            get { return _database; }
        }
    }
}

