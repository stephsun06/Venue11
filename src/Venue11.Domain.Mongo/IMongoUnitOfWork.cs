using MongoDB.Driver;

namespace Venue11.Domain.Mongo
{
    public interface IMongoUnitOfWork
    {
        IMongoDatabase Context { get; }
    }
}