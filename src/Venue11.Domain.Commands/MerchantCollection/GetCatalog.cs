

using System;

namespace Venue11.Domain.Commands.MerchantCollection
{
    public class GetCatalog
    {
        public int Id { get; set; }
        public string MerchantName { get; set; }
        public string Url { get; set; }
        public string ApiKey { get; set; }
        public Guid GroupKey { get; set; }
        public DateTime RequestedDate { get; set; }
    }
}
