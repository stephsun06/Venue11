
using System.Collections.Generic;


namespace Venue11.Domain.Mongo.Entities
{
    public class Product
    {

        public string part_number { get; set; }
        public string name { get; set; }
        public string brand { get; set; }
        public string merchant { get; set; }
        public string merchant_id { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string color { get; set; }
        public string[] image { get; set; }

        public string[] category { get; set; }
        public decimal reatil_price { get; set; }
        public decimal sale_price { get; set; }
        public decimal price { get; set; }
        public bool on_sale { get; set; }
        public bool onSale { get; set; }
        public decimal shipping_charge { get; set; }
        public decimal ship_flat_rate { get; set; }
        public List<Color> colors { get; set; }

        public Product()
        {
            colors = new List<Color>();
        }

    }
}
