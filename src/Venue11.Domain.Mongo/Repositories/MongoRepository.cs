using MongoDB.Driver;
using MongoDB.Bson;
using System;
using Venue11.Domain.Mongo.Entities;

namespace Venue11.Domain.Mongo.Repositories
{
    public class MongoRepository<T> : IMongoRepository<T> where T : Entity, new()
    {
        private IMongoDatabase _database;

        private string _collectionName;

        public MongoRepository(IMongoUnitOfWork unitofWork, string collectname)
        {
            if (unitofWork == null) throw new NullReferenceException("UnitOfWork");

            _database = unitofWork.Context;
            _collectionName = collectname;

        }

        public FilterDefinitionBuilder<T> Filter
        {
            get { return Builders<T>.Filter; }
        }

        public SortDefinitionBuilder<T> Sort
        {
            get { return Builders<T>.Sort; }
        }

        public IMongoCollection<T> Collection
        {
            get { return _database.GetCollection<T>(_collectionName); }
        }

        public void Insert(T entity)
        {
            Collection.InsertOne(entity);
        }

    }
}