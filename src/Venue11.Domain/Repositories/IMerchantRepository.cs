
using System.Collections.Generic;
using Venue11.Domain.Entities;

namespace Venue11.Domain.Repositories
{
    public interface IMerchantRepository
    {
        IEnumerable<Merchant> GetMerchants();
        void UpdateMerchant(Merchant entity);
    }
}
