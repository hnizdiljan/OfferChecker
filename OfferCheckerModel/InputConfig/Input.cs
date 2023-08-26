using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfferCheckerModel.InputConfig
{
    public class Input
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public Offer CheapestOffer { get; set; }
        public DateTime DiscoveryDateTime { get; set; }
    }
}
