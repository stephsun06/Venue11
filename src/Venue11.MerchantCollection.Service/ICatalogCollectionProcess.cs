
using Venue11.Domain.Commands.MerchantCollection;

namespace Venue11.MerchantCollection.Service
{
    public interface ICatalogCollectionProcess
    {
        void Run(GetCatalog getCatalog);
    }
}
