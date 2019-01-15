
using System.Collections.Generic;
using Venue11.Domain.Mongo.Entities;

namespace Venue11.Domain.Mongo.Repositories
{
    public interface ICatalogRepository
    {
        IEnumerable<ProductLog> GetCatalog(string merchant);
        ProductLog GetProductLog(string id);
        ProductLog InsertLog(ProductLog log);
        void Delete(string merchant);
    
    }
}
