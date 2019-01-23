
using Venue11.MerchantCollection.Commands;

namespace Venue11.MerchantCollection.Service
{
    public interface ICatalogCollectionProcess
    {
        void Run(GetCatalog getCatalog);
    }
}
