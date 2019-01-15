
using MongoDB.Driver;
using Venue11.Domain.Mongo.Entities;

namespace Venue11.Domain.Mongo.Repositories
{
    public interface IMongoRepository<T> where T : Entity, new()
    {
        IMongoCollection<T> Collection { get; }
        SortDefinitionBuilder<T> Sort { get; }
        FilterDefinitionBuilder<T> Filter { get; }
        void Insert(T entity);

    }
}
