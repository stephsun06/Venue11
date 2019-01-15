using Flurl;
using Flurl.Http;
using log4net;
using System;
using System.Threading.Tasks;
using Venue11.Domain.Mongo.Entities;

namespace Venue11.MerchantCollection.Service.Api
{
    class CatalogApiService : ICatalogApiService 
    {

        private static readonly ILog _log = LogManager.GetLogger(typeof(CatalogApiService));

        public async Task<Catalog> GetCatalog(string url , string apikey)
        {
            try
            {
                Url client = new Url(url);

                client.WithHeader("Accept", "application/json");

                var result = await client.WithOAuthBearerToken(apikey).GetJsonAsync<Catalog>();


                if (result == null) return null;

                return result;

            }
            catch (Exception ex)
            {
                return null;
            }


        }
    }
}
