using System;

namespace Venue11.Domain.Entities
{
    public class Product
    {
        public virtual long Id { get; set; }
        public virtual string PartNumber { get; set; }
        public virtual string ProductName { get; set; }
        public virtual string Description { get; set; }
        public virtual int BrandId { get; set; }
        public virtual int MerchantId { get; set; }
        public virtual string ProductUrl { get; set; }
        public virtual decimal RetailPrice { get; set; }
        public virtual decimal SalePrice { get; set; }
        public virtual decimal Price { get; set; }
        public virtual bool OnSale { get; set; }
        public virtual decimal ShippingCharge { get; set; }
        public virtual decimal ShipFlatRate { get; set; }
        public virtual int CategoryId { get; set; }
        public virtual DateTime LastUpdated { get; set; }
    }
}
