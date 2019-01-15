using System;

namespace Venue11.Domain.Mongo.Entities
{
    public class ProductLog : Entity
    {
        public DateTime ReceivdDate { get; set; }
        public string MerchantName { get; set; }
        public string GroupId { get; set; }
        public Catalog Catalog { get; set; }
    }
}
