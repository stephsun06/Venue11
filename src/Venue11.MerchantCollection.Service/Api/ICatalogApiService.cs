using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venue11.Domain.Mongo.Entities;

namespace Venue11.MerchantCollection.Service.Api
{
    public interface ICatalogApiService
    {
        Task<Catalog> GetCatalog(string url , string apiKey);
    }
}
