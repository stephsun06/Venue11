

namespace Venue11.Domain.Mongo
{
    public class Size
    {
        public string size { get; set; }
        public long id { get; set; }
        public string merchant_sku { get; set; }
        public string sku { get; set; }
        public string upc { get; set; }
        public bool active { get; set; }
    }
}
