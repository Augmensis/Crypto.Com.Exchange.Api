using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Com.Exchange.Api.Models.Responses
{
    public class TickerResponse
    {
        public string InstrumentName { get; set; }

        public List<Ticker> Data { get; set; }
    }
}
