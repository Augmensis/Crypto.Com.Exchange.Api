using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Com.Exchange.Api.Models.Responses
{
    public class BookResponse
    {
        public string InstrumentName { get; set; }
        public int Depth { get; set; }

        // NOTE: The books are currently returned as a single item in an array for some reason.
        public IEnumerable<OrderBook> Data { get; set; } 
        public OrderBook Book => Data.FirstOrDefault();

    }
}
