using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venue11.Domain.Entities
{
    public class Merchant
    {
        public int id { get; set; }
        public string merchant_name { get; set; }
        public string url { get; set; }
        public DateTime requested { get; set; }
        
    }
}
