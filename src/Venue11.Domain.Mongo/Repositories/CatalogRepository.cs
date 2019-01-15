

using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using Venue11.Domain.Mongo.Entities;

namespace Venue11.Domain.Mongo.Repositories
{
    public class CatalogRepository : MongoRepository<ProductLog>, ICatalogRepository
    {

        public CatalogRepository(IMongoUnitOfWork unitofwork) : base(unitofwork, "ProductLog") { }


        public void Delete(string merchant)
        {
            Collection.DeleteOneAsync(Filter.Eq(x => x.MerchantName, merchant));
        }

        public IEnumerable<ProductLog> GetCatalog(string groupId)
        {
            return Collection.FindSync(Filter.Eq(x => x.Id, groupId)).ToList();
        }

        public ProductLog GetProductLog(string id)
        {
            return Collection.FindSync(Filter.Eq(x => x.Id, id)).FirstOrDefault();
        }

        public ProductLog InsertLog(ProductLog log)
        {
             Collection.InsertOne(log);

            return log;
        }
    }
}
