

using System.Collections.Generic;

namespace Venue11.Domain.Mongo.Entities
{
    public class Catalog 
    {
        public Metadata metadata { get; set; }

        public Navigation navigation { get; set; }

        public List<Product> products { get; set; }

        public Catalog()
        {
            metadata = new Metadata();
            navigation = new Navigation();
            products = new List<Product>();
        }
    }
}
