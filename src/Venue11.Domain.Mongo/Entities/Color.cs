

using System.Collections.Generic;

namespace Venue11.Domain.Mongo.Entities
{
    public class Color
    {
        public long id { get; set; }
        public string color { get; set; }
        public decimal retail_price { get; set; }
        public decimal sales_price { get; set; }
  
        public string[] image { get; set; }

        public List<Size> sizes { get; set; }

        public Color()
        {
            sizes = new List<Size>();
        }
    }
}
